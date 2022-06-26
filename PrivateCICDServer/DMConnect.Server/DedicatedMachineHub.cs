using System.Net;
using System.Net.Sockets;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace DMConnect.Server;

public class DedicatedMachineHub : IMachineAgentEventListener
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<DedicatedMachineHub> _logger;
    private readonly IDedicatedMachineService _machineMachineService;
    private readonly IInstanceService _instanceService;
    private readonly int _port;
    private readonly Thread _thread;
    private readonly List<MachineAgentClient> _clients = new();

    private readonly CancellationTokenSource _cancellationTokenSource;

    public DedicatedMachineHub(ILoggerFactory loggerFactory, IDedicatedMachineService machineService,
        IInstanceService instanceService, int port)
    {
        _loggerFactory = loggerFactory;
        _machineMachineService = machineService;
        _instanceService = instanceService;
        _port = port;
        _logger = _loggerFactory.CreateLogger<DedicatedMachineHub>();
        _thread = new Thread(ListenLoop);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Start()
    {
        _thread.Start();
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
        if (_thread.ThreadState != ThreadState.Unstarted)
            _thread.Join();
        lock (_clients)
        {
            while (_clients.Count != 0)
                Monitor.Wait(_clients);
        }
    }

    private async void ListenLoop()
    {
        var tcpListener = new TcpListener(IPAddress.Loopback, _port);
        tcpListener.Start();
        _logger.LogInformation("Started to listen to port {Port}", _port);

        try
        {
            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync(_cancellationTokenSource.Token);
                _logger.LogInformation("Accepted tcp client");

                var machineAgent = new MachineAgentClient(_loggerFactory.CreateLogger<MachineAgentClient>(),
                    this,
                    client,
                    _cancellationTokenSource.Token);
                _clients.Add(machineAgent);
                machineAgent.Start();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Caught cancel operation");
        }
        finally
        {
            tcpListener.Stop();
            _logger.LogInformation("Stopped");
        }
    }

    public void OnMachineAgentLeave(MachineAgentClient machineAgent)
    {
        if (machineAgent.Id != Guid.NewGuid())
            _machineMachineService.SetState(new SetStateDto(machineAgent.Id, DedicatedMachineState.Offline));
        lock (_clients)
        {
            _clients.Remove(machineAgent);
            Monitor.Pulse(_clients);
        }
    }

    public void MachineAgentTryAuth(AuthDto authDto)
    {
        _machineMachineService.AuthMachine(authDto);
    }

    public DedicatedMachine MachineAgentTryRegister(RegisterDto registerDto)
    {
        return _machineMachineService.RegisterMachine(registerDto);
    }

    public void OnMachineAgentAuth(MachineAgentClient machineAgent)
    {
        _machineMachineService.SetState(new SetStateDto(machineAgent.Id, DedicatedMachineState.Online));
    }

    public void OnMachineAgentAction(IDedicateMachineDto action)
    {
        switch (action)
        {
            case InstanceStdOutDto stdOutDto:
            case InstanceStdErrDto stdErrDto:
                throw new NotImplementedException();
            case InstanceSetStateDto setStateDto:
                _instanceService.ChangeInstanceState(setStateDto.InstanceId, setStateDto.InstanceState);
                break;
            default:
                throw new Exception("Unexpected action: " + action);
        }
    }
}