using Domain.Entities;

namespace Domain.Services;

public interface IDedicatedMachineService
{
    DedicatedMachine RegisterMachine(string tokenStr, string label, string description);
    
    bool AuthMachine(Guid id, string tokenStr);

    void SetState(Guid serverId, DedicatedMachine.DedicatedMachineState state);
}