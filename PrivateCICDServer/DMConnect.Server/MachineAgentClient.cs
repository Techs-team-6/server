using System.Net.Sockets;
using DMConnect.Share;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Domain.Tools;

namespace DMConnect.Server;

public class MachineAgentClient : IDedicatedMachineAgent
{
    private readonly IDedicatedMachineService _machineService;
    private readonly TcpClient _client;
    private readonly Action<MachineAgentClient> _onMachineAgentLeave;
    private readonly Stream _stream;
    private readonly Thread _thread;

    public Guid Id { get; private set; }

    public MachineAgentClient(IDedicatedMachineService machineService,
        TcpClient client, Action<MachineAgentClient> onMachineAgentLeave)
    {
        _machineService = machineService;
        _client = client;
        _onMachineAgentLeave = onMachineAgentLeave;
        _stream = client.GetStream();
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
            AuthLoop();
            ServeLoop();
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            Console.Error.WriteLine("Disconnecting");
        }
        finally
        {
            _client.Close();
            _onMachineAgentLeave.Invoke(this);
            if (Id != Guid.Empty)
                _machineService.SetState(new SetStateDto(Id, DedicatedMachineState.Offline));
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
                    catch (AuthException)
                    {
                        _stream.WriteActionDto(new AuthResultDto(false, Guid.Empty));
                    }

                    break;
                case RegisterDto registerDto:
                    try
                    {
                        var dedicatedMachine = _machineService.RegisterMachine(registerDto);
                        Id = dedicatedMachine.Id;
                    }
                    catch (InvalidTokenException)
                    {
                        _stream.WriteActionDto(new AuthResultDto(false, Guid.Empty));
                    }

                    break;
                default:
                    _stream.WriteActionDto(new AuthResultDto(false, Guid.Empty));
                    break;
            }
        }

        _stream.WriteActionDto(new AuthResultDto(true, Id));
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