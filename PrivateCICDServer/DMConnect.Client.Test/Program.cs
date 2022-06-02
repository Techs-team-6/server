using System.Net;
using DMConnect.Share;
using DMConnect.Client;
using Domain.Dto.DedicatedMachineDto;

var client = new RemoteDedicatedMachineHub(new IPEndPoint(IPAddress.Loopback, 50050), new DedicatedMachineAgent(),
    new RegisterDto("8I2BVKBW662NZMF7TBPGJOBF777E1F7Q", "Lighty", "Description is cool, but no"));
client.Start();

// client.ConsoleWrite("Hello from client");
// client.ConsoleWriteLine(", hello from client and line");

Thread.Sleep(10000);

class DedicatedMachineAgent : IDedicatedMachineAgent 
{
}