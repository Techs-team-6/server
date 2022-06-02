using Domain.Entities;

namespace Domain.Dto.DedicatedMachineDto;

public class SetStateDto
{
    public readonly Guid ServerId;
    public readonly DedicatedMachineState State;

    public SetStateDto(Guid serverId, DedicatedMachineState state)
    {
        ServerId = serverId;
        State = state;
    }
}