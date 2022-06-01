using System.Net;
using System.Net.Sockets;
using DedicatedMachine.Share;
using DedicatedMachine.Share.Dto;

namespace DedicatedMachine.Client;

public class RemoteDedicatedMachineHub : IDedicatedMachineHub
{
    private readonly IDedicatedMachineAgent _agent;
    private readonly TcpClient _tcpClient;
    private readonly Stream _stream;

    private RemoteDedicatedMachineHub(IPEndPoint endPoint, IDedicatedMachineAgent agent)
    {
        _agent = agent;
        _tcpClient = new TcpClient();
        _tcpClient.Connect(endPoint);
        _stream = _tcpClient.GetStream();        
    }
    
    public RemoteDedicatedMachineHub(IPEndPoint endPoint, IDedicatedMachineAgent agent, RegisterDto dto) :
        this(endPoint, agent)
    {
        Register(dto);
        new Thread(() => Start()).Start();
    }
    
    public RemoteDedicatedMachineHub(IPEndPoint endPoint, IDedicatedMachineAgent agent, AuthDto dto) :
        this(endPoint, agent)
    {
        Authenthicate(dto);
        new Thread(() => Start()).Start();
    }

    private void Start()
    {
        try
        {
            while (true)
            {
                var action = _stream.ReadClientAction();
                switch (action)
                {
                    case ClientAction.ConsoleWrite:
                    {
                        var dto = _stream.Read<MessageDto>();
                        _agent.ConsoleWrite(dto.Value);
                        break;
                    }
                    case ClientAction.ConsoleWriteLine:
                    {
                        var dto = _stream.Read<MessageDto>();
                        _agent.ConsoleWriteLine(dto.Value);
                        break;
                    }
                    default:
                    {
                        Console.Error.WriteLine("Unknown action : " + action);
                        break;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
        }
        finally
        {
            _tcpClient.Close();
        }
        
    }

    public void Register(RegisterDto dto)
    {
        lock (this)
        {
            _stream.WriteAction(HubAction.Register);
            _stream.Write(dto);
        }
    }

    public void Authenthicate(AuthDto dto)
    {
        lock (this)
        {
            _stream.WriteAction(HubAction.Authenthicate);
            _stream.Write(dto);
        }
    }

    public void ConsoleWrite(string value)
    {
        lock (this)
        {
            _stream.WriteAction(HubAction.ConsoleWrite);
            _stream.Write(new MessageDto(value));
        }
    }

    public void ConsoleWriteLine(string value)
    {
        lock (this)
        {
            _stream.WriteAction(HubAction.ConsoleWriteLine);
            _stream.Write(new MessageDto(value));
        }
    }
}