using System.Net;
using System.Net.Sockets;
using Domain.Services;

namespace DMConnect.Server;

public class DedicatedMachineHub
{
    private readonly IDedicatedMachineService _machineService;

    public DedicatedMachineHub(IDedicatedMachineService service, int port)
    {
        _machineService = service;
        new Thread(() => Start(port)).Start();
    }

    private void Start(int port)
    {
        var tcpListener = new TcpListener(IPAddress.Loopback, port);
        tcpListener.Start();
        Console.WriteLine($"{GetType().Name} began listening port {port}");
        while (true)
        {
            var client = tcpListener.AcceptTcpClient();
            var machineAgent = new RemoteDedicatedMachineAgent(_machineService, client);
            machineAgent.Start();
        }
    }
}