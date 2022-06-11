using System.Net.Sockets;
using DMConnect.Share;
using DMConnect.Share.Tools;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Domain.Tools;
using Microsoft.Extensions.Logging;

namespace DMConnect.Server;

public class MachineAgentClient : IDedicatedMachineAgent
{
    private readonly ILogger<MachineAgentClient> _logger;
    private readonly IDedicatedMachineService _machineService;
    private readonly TcpClient _client;
    private readonly Action<MachineAgentClient> _onMachineAgentLeave;
    private readonly Stream _stream;
    private readonly Thread _thread;

    public Guid Id { get; private set; }

    public MachineAgentClient(ILogger<MachineAgentClient> logger, IDedicatedMachineService machineService,
        TcpClient client, Action<MachineAgentClient> onMachineAgentLeave, CancellationToken cancellationToken)
    {
        _logger = logger;
        _machineService = machineService;
        _client = client;
        _onMachineAgentLeave = onMachineAgentLeave;
        _stream = new CancellableStreamWrapper(client.GetStream(), cancellationToken);
        _thread = new Thread(Serve);
    }

    public void Start()
    {
        _thread.Start();
    }

    public void Join()
    {
        if (_thread.ThreadState != ThreadState.Unstarted)
            _thread.Join();
    }

    private void Serve()
    {
        try
        {
            _logger.LogInformation("Started");
            AuthLoop();
            ServeLoop();
        }
        catch (Exception e)
        {
            if (ExceptionUtils.IsOperationCanceled(e))
            {
                _logger.LogInformation("Caught cancel operation");
            }
            else
            {
                _logger.LogError(e, "Exception acquired");                
            }
        }
        finally
        {
            _client.Close();
            _onMachineAgentLeave.Invoke(this);
            if (Id != Guid.Empty)
                _machineService.SetState(new SetStateDto(Id, DedicatedMachineState.Offline));
            _logger.LogInformation("Stopped");
        }
    }

    private void AuthLoop()
    {
        while (Id == Guid.Empty)
        {
            switch (_stream.ReadActionDto())
            {
                case AuthDto authDto:
                    try
                    {
                        _machineService.AuthMachine(authDto);
                        Id = authDto.Id;
                    }
                    catch (AuthException e)
                    {
                        _stream.WriteActionDto(new AuthResultDto(false, Guid.Empty));
                        _logger.LogInformation("Auth failed: {reason}", e.Message);
                    }
                    break;
                case RegisterDto registerDto:
                    try
                    {
                        var dedicatedMachine = _machineService.RegisterMachine(registerDto);
                        Id = dedicatedMachine.Id;
                    }
                    catch (InvalidTokenException e)
                    {
                        _stream.WriteActionDto(new AuthResultDto(false, Guid.Empty));
                        _logger.LogInformation("Register failed: {reason}", e.Message);
                    }
                    break;
                default:
                    _stream.WriteActionDto(new AuthResultDto(false, Guid.Empty));
                    _logger.LogInformation("Auth failed");
                    break;
            }
        }

        _stream.WriteActionDto(new AuthResultDto(true, Id));
        _logger.LogInformation("Auth successful");
        _machineService.SetState(new SetStateDto(Id, DedicatedMachineState.Online));
    }

    private void ServeLoop()
    {
        while (true)
        {
            var action = _stream.ReadActionDto();
            switch (action)
            {
                case InstanceStdOutDto instanceStdOutDto:
                    throw new NotImplementedException();
                case InstanceStdErrDto instanceStdErrDto:
                    throw new NotImplementedException();
                default:
                    throw new Exception("Unexpected action: " + action);
            }
        }
    }

    public void StartInstance(StartInstanceDto dto)
    {
        _stream.WriteActionDto(dto);
    }
}