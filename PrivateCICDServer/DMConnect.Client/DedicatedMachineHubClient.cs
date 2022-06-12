using System.Net;
using System.Net.Sockets;
using DMConnect.Share;
using DMConnect.Share.Tools;
using Domain.Dto.DedicatedMachineDto;
using Domain.Tools;

namespace DMConnect.Client;

public class DedicatedMachineHubClient : IDedicatedMachineHub
{
    public const string MachineIdFileName = "machineId.txt";

    private readonly IPEndPoint _endPoint;
    private readonly RegisterDto _credentials;
    private readonly TcpClient _tcpClient;
    private readonly Thread _thread;
    private Stream _stream = null!;

    public Guid MachineId { get; private set; }
    private IDedicatedMachineAgent _agent = null!;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public DedicatedMachineHubClient(IPEndPoint endPoint, RegisterDto credentials)
    {
        _endPoint = endPoint;
        _credentials = credentials;
        _tcpClient = new TcpClient();
        _thread = new Thread(Run);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void SetMachineAgent(IDedicatedMachineAgent agent)
    {
        _agent = agent;
    }

    public void Start()
    {
        if (_agent is null)
            throw new Exception("SetMachineAgent must be called before Start");
        _tcpClient.Connect(_endPoint);
        _stream = new CancellableStreamWrapper(_tcpClient.GetStream(), _cancellationTokenSource.Token);
        Auth();
        _thread.Start();
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
        if (_thread.ThreadState == ThreadState.Running)
            _thread.Join();
    }

    private void Run()
    {
        try
        {
            Console.WriteLine("Started");
            ReadActionLoop();
        }
        catch (Exception e)
        {
            if (ExceptionUtils.IsOperationCanceled(e))
            {
                Console.WriteLine("Caught cancel operation");
            }
            else
            {
                Console.Error.WriteLine(e);                
            }
        }
        finally
        {
            _tcpClient.Close();
            Console.WriteLine("Stopped");
        }
    }

    private void ReadActionLoop()
    {
        while (true)
        {
            var action = _stream.ReadActionDto();
            switch (action)
            {
                case StartInstanceDto startInstanceDto:
                    _agent.StartInstance(startInstanceDto);
                    break;
                default:
                    throw new Exception("Unexpected action: " + action);
            }
        }
    }

    private void Auth()
    {
        if (!TryAuth() && !TryRegister())
        {
            throw new AuthException("Could not login into hub: wrong credentials");
        }
    }

    private bool TryAuth()
    {
        if (!File.Exists(MachineIdFileName))
            return false;

        var id = Guid.Parse(File.ReadAllText(MachineIdFileName));
        _stream.WriteActionDto(new AuthDto(id, _credentials.TokenString));

        var authResult = (_stream.ReadActionDto() as AuthResultDto)!;
        if (!authResult.IsSuccessful)
        {
            File.Delete(MachineIdFileName);
            return false;
        }

        MachineId = authResult.DedicatedMachineId;
        return true;
    }

    private bool TryRegister()
    {
        _stream.WriteActionDto(_credentials);
        var authResult = (_stream.ReadActionDto() as AuthResultDto)!;
        if (!authResult.IsSuccessful)
            return false;

        MachineId = authResult.DedicatedMachineId;
        File.WriteAllText(MachineIdFileName, authResult.DedicatedMachineId.ToString());
        return true;
    }

    public void InstanceStdOut(InstanceStdOutDto dto)
    {
        _stream.WriteActionDto(dto);
    }

    public void InstanceStdErr(InstanceStdErrDto dto)
    {
        _stream.WriteActionDto(dto);
    }
}