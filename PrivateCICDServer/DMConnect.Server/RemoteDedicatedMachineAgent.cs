using System.Net.Sockets;
using DMConnect.Share;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Action = Domain.Dto.DedicatedMachineDto.Action;

namespace DMConnect.Server;

public class RemoteDedicatedMachineAgent : IDedicatedMachineAgent
{
    private readonly TcpClient _client;
    private readonly Stream _stream;
    private readonly IDedicatedMachineService _machineService;
    private readonly Thread _thread;

    private Guid _id;

    public RemoteDedicatedMachineAgent(IDedicatedMachineService machineService, TcpClient client)
    {
        _machineService = machineService;
        _client = client;
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
            AuthOperation();

            while (true)
            {
                var action = _stream.ReadAction();
                switch (action)
                {
                    // case HubActio.ConsoleWrite:
                    // {
                    //     var dto = _stream.Read<StringDto>();
                    //     Console.Write(dto.Value);
                    //     // ConsoleWrite(dto.Value);
                    //     break;
                    // }
                    // case HubAction.ConsoleWriteLine:
                    // {
                    //     var dto = _stream.Read<StringDto>();
                    //     Console.WriteLine(dto.Value);
                    //     // ConsoleWriteLine(dto.Value);
                    //     break;
                    // }
                    default:
                        throw new Exception("Wrong action : " + action);
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            Console.Error.WriteLine("Disconnecting");
        }
        finally
        {
            _client.Close();
            if (_id != Guid.Empty)
                _machineService.SetState(new SetStateDto(_id, DedicatedMachineState.Offline));
        }
    }

    private void AuthOperation()
    {
        var action = _stream.ReadAction();
        if (action == Action.Register)
        {
            var dto = _stream.Read<RegisterDto>();
            var dedicatedMachine = _machineService.RegisterMachine(dto);
            
            _id = dedicatedMachine.Id;
            _machineService.SetState(new SetStateDto(_id, DedicatedMachineState.Online));
        }
        else if (action == Action.Authenthicate)
        {
            var dto = _stream.Read<AuthDto>();
            if (!_machineService.AuthMachine(dto))
                throw new Exception("Wrong credentials");
            
            _id = dto.Id;
            _machineService.SetState(new SetStateDto(_id, DedicatedMachineState.Online));
        }
        else
        {
            throw new Exception("Authenthication is required");
        }
    }
}