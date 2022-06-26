using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;

namespace DMConnect.Server;

public interface IMachineAgentEventListener
{
    void OnMachineAgentLeave(MachineAgentClient machineAgent);

    void MachineAgentTryAuth(AuthDto authDto);

    DedicatedMachine MachineAgentTryRegister(RegisterDto registerDto);
    
    void OnMachineAgentAuth(MachineAgentClient machineAgent);

    void OnMachineAgentAction(IDedicateMachineDto action);
}