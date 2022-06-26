using System.Net;
using DMConnect.Share;
using DMConnect.Client;
using Domain.Dto.DedicatedMachineDto;

var port = int.Parse(args[1]);
var endPoint = new IPEndPoint(IPAddress.Loopback, port);

var token = args[0];
var client = new DedicatedMachineHubClient(endPoint, new RegisterDto(
    token,
    "Lightly",
    "Description is cool, but no"));

var machineAgent = new MachineAgent();
client.SetMachineAgent(machineAgent);

client.Start();
Thread.Sleep(10000);

internal class MachineAgent : IDedicatedMachineAgent 
{
    public void StartInstance(StartInstanceDto dto)
    {
        throw new NotImplementedException();
    }
}