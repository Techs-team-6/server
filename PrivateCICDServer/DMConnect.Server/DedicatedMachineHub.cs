using System.Net;
using System.Net.Sockets;
using Domain.Services;

namespace DMConnect.Server;

public class DedicatedMachineHub
{
    private readonly IDedicatedMachineService _machineService;
    private readonly int _port;
    private readonly Thread _thread;
    private readonly List<MachineAgentClient> _clients = new();

    private readonly CancellationTokenSource _cancellationTokenSource;

    public DedicatedMachineHub(IDedicatedMachineService service, int port)
    {
        _machineService = service;
        _port = port;
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
        _thread.Join();
    }

    private async void ListenLoop()
    {
        var tcpListener = new TcpListener(IPAddress.Loopback, _port);
        tcpListener.Start();
        Console.WriteLine($"{GetType().Name} began listening port {_port}");
        
        try
        {
            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync(_cancellationTokenSource.Token);

                var machineAgent = new MachineAgentClient(
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
            Console.WriteLine("Caught cancel operation");
        }
        finally
        {
            tcpListener.Stop();
            Console.WriteLine("Stopped");
        }
    }

    private void OnMachineAgentLeave(MachineAgentClient client)
    {
        _clients.Remove(client);
    }
}