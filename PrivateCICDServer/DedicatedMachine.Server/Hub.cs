using System.Net;
using System.Net.Sockets;
using Domain.Services;

namespace DedicatedMachine.Server;

public class Hub
{
    private IDedicatedMachineService _machineService;

    public Hub(IDedicatedMachineService service, int port)
    {
        _machineService = service;
        new Thread(() => Start(port)).Start();
    }

    private void Start(int port)
    {
        var tcpListener = new TcpListener(IPAddress.Loopback, port);
        tcpListener.Start();
        Console.WriteLine("Started tcp listener");
        while (true)
        {
            Console.WriteLine("Accept client");
            var client = tcpListener.AcceptTcpClient();
            Console.WriteLine("Accepted");
            var machineAgent = new RemoteDedicatedMachineAgent(this, client);
            // new Thread(() => ServeClient(client)).Start();
        }
    }

    // private void ServeClient(TcpClient client)
    // {
    //     var remoteAgent = new RemoteDedicatedMachineAgent(this, client);
    // }

    public Guid Register(string tokenStr, string label, string description)
    {
        var machine = _machineService.RegisterMachine(tokenStr, label, description);
        return machine.Id;
    }
    
    public Guid Authenthicate(Guid id, string tokenStr)
    {
        if (!_machineService.AuthMachine(id, tokenStr))
            throw new Exception("Wrong credentials");
        return id;
    }
}