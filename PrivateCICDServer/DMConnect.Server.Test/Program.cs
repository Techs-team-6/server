using DMConnect.Server;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;

var hub = new DedicatedMachineHub(new DedicatedMachineService(), 50050);

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
        return null;
    }
}