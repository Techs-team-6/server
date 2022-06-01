using System.Net;
using DedicatedMachine.Client;
using DedicatedMachine.Share;
using DedicatedMachine.Share.Dto;

var client = new RemoteDedicatedMachineHub(new IPEndPoint(IPAddress.Loopback, 50050), new DedicatedMachineAgent(),
    new RegisterDto("asdasdaopsdTOkeN", "Lighty", "Description is cool, but no"));

client.ConsoleWrite("Hello from client");
client.ConsoleWriteLine(", hello from client and line");

Thread.Sleep(10000);

class DedicatedMachineAgent : IDedicatedMachineAgent 
{
    public void ConsoleWrite(string value)
    {
        Console.Write(value);
    }

    public void ConsoleWriteLine(string value)
    {
        Console.WriteLine(value);
    }
}