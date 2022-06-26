using System.Net.Sockets;
using DMConnect.Share;
using DMConnect.Share.Tools;
using Domain.Dto.DedicatedMachineDto;
using Domain.Tools;
using Microsoft.Extensions.Logging;

namespace DMConnect.Server;

public class MachineAgentClient : IDedicatedMachineAgent
{
    private readonly ILogger<MachineAgentClient> _logger;
    private readonly IMachineAgentEventListener _eventListener;
    private readonly TcpClient _client;
    private readonly Stream _stream;
    private readonly Thread _thread;

    public Guid Id { get; private set; }

    public MachineAgentClient(ILogger<MachineAgentClient> logger, IMachineAgentEventListener eventListener,
        TcpClient client, CancellationToken cancellationToken)
    {
        _logger = logger;
        _eventListener = eventListener;
        _client = client;
        _stream = new CancellableStreamWrapper(client.GetStream(), cancellationToken);
        _thread = new Thread(Serve);
    }

    public void Start()
    {
        _thread.Start();
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
            _eventListener.OnMachineAgentLeave(this);
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
                        _eventListener.MachineAgentTryAuth(authDto);
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
                        var dedicatedMachine = _eventListener.MachineAgentTryRegister(registerDto);
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
        _eventListener.OnMachineAgentAuth(this);
    }

    private void ServeLoop()
    {
        while (true)
        {
            _eventListener.OnMachineAgentAction(_stream.ReadActionDto());
        }
    }

    public void StartInstance(StartInstanceDto dto)
    {
        _stream.WriteActionDto(dto);
    }
}