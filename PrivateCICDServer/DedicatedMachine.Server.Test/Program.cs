
using DedicatedMachine.Server;
using Domain.Services;

var hub = new Hub(new DedicatedMachineService(), 50050);

Console.WriteLine("Sleeping");
Thread.Sleep(1000_000);

internal class DedicatedMachineService : IDedicatedMachineService
{
    public Domain.Entities.DedicatedMachine RegisterMachine(string tokenStr, string label, string description)
    {
        return new Domain.Entities.DedicatedMachine
        {
            Id = Guid.Empty
        };
    }

    public bool AuthMachine(Guid id, string tokenStr)
    {
        return true;
    }

    public void SetState(Guid serverId, Domain.Entities.DedicatedMachine.DedicatedMachineState state)
    {
        Console.WriteLine("Set state");
    }
}