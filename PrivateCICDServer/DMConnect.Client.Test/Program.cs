using System.Net;
using DMConnect.Share;
using DMConnect.Client;
using Domain.Dto.DedicatedMachineDto;

var endPoint = new IPEndPoint(IPAddress.Loopback, 50050);
var machineAgent = new DedicatedMachineAgent();

RemoteDedicatedMachineHub client;
var token = "Q5P5CVCW57Z5QKFN5C5JH1CZVWDMRYRY";
var fileName = "machineId.txt";

if (File.Exists(fileName))
{
    var id = Guid.Parse(File.ReadAllText(fileName));
    client = new RemoteDedicatedMachineHub(endPoint, machineAgent, new AuthDto(id, token));
}
else
{
    client = new RemoteDedicatedMachineHub(endPoint, machineAgent, new RegisterDto(
        token,
        "Lighty",
        "Description is cool, but no"));
    File.WriteAllText(fileName, client.MachineId.ToString());
}

client.Start();

Thread.Sleep(10000);

class DedicatedMachineAgent : IDedicatedMachineAgent 
{
    public void StartInstance(StartInstanceDto dto)
    {
        throw new NotImplementedException();
    }
}