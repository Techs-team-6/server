using System.Net;
using System.Net.Sockets;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace DMConnect.Server;

public class DedicatedMachineHub
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<DedicatedMachineHub> _logger;
    private readonly IDedicatedMachineService _machineService;
    private readonly int _port;
    private readonly Thread _thread;
    private readonly List<MachineAgentClient> _clients = new();

    private readonly CancellationTokenSource _cancellationTokenSource;

    public DedicatedMachineHub(ILoggerFactory loggerFactory, IDedicatedMachineService service, int port)
    {
        _loggerFactory = loggerFactory;
        _machineService = service;
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
                    _machineService,
                    client,
                    OnMachineAgentLeave,
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

    private void OnMachineAgentLeave(MachineAgentClient client)
    {
        lock (_clients)
        {
            _clients.Remove(client);
            Monitor.Pulse(_clients);
        }
    }
}