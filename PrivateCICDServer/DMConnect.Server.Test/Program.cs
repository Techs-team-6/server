using DMConnect.Server;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Microsoft.Extensions.Logging;

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var hub = new DedicatedMachineHub(loggerFactory, new DedicatedMachineService(), null!, 50050);
hub.Start();
hub.Start();

Console.WriteLine("Sleeping");
Thread.Sleep(1000_000);

internal class DedicatedMachineService : IDedicatedMachineService
{
    public DedicatedMachine RegisterMachine(RegisterDto dto)
    {
        return new DedicatedMachine { Id = Guid.NewGuid() };
    }

    public void AuthMachine(AuthDto dto)
    {
    }

    public void SetState(SetStateDto dto)
    {
        Console.WriteLine("Set state: " + Enum.GetName(dto.State));
    }

    public List<DedicatedMachine> List()
    {
        return new List<DedicatedMachine>();
    }

    public DedicatedMachine GetDedicatedMachine(Guid id)
    {
        throw new NotImplementedException();
    }
}