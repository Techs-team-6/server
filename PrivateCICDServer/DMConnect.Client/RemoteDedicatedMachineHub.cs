using System.Net;
using System.Net.Sockets;
using DMConnect.Share;
using Domain.Dto.DedicatedMachineDto;
using Action = Domain.Dto.DedicatedMachineDto.Action;

namespace DMConnect.Client;

public class RemoteDedicatedMachineHub : IDedicatedMachineHub
{
    public Guid MachineId { get; }
    
    private readonly IDedicatedMachineAgent _agent;
    private readonly TcpClient _tcpClient;
    private readonly Stream _stream;
    private readonly Thread _thread;

    private RemoteDedicatedMachineHub(IPEndPoint endPoint, IDedicatedMachineAgent agent)
    {
        _agent = agent;
        _tcpClient = new TcpClient();
        _tcpClient.Connect(endPoint);
        _stream = _tcpClient.GetStream();
        _thread = new Thread(Connect);
    }
    
    public RemoteDedicatedMachineHub(IPEndPoint endPoint, IDedicatedMachineAgent agent, RegisterDto dto) :
        this(endPoint, agent)
    {
        _stream.Write(dto);
        MachineId = _stream.Read<Guid>();
    }
    
    public RemoteDedicatedMachineHub(IPEndPoint endPoint, IDedicatedMachineAgent agent, AuthDto dto) :
        this(endPoint, agent)
    {
        _stream.Write(dto);
        MachineId = _stream.Read<Guid>();
    }

    public void Start()
    {
        _thread.Start();
    }
    
    private void Connect()
    {
        try
        {
            while (true)
            {
                var action = _stream.ReadAction();
                switch (action)
                {
                    case Action.StartInstance:
                        _agent.StartInstance(_stream.Read<StartInstanceDto>());
                        break;
                    default:
                        throw new Exception("Unknown action : " + action);
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

    public void InstanceOutErr(InstanceStdOutDto dto)
    {
        _stream.Write(dto);
    }

    public void InstanceStdErr(InstanceStdErrDto dto)
    {
        _stream.Write(dto);
    }
}