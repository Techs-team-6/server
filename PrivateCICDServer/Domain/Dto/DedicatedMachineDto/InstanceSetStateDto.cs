using Domain.Entities.Instances;

namespace Domain.Dto.DedicatedMachineDto;

public class InstanceSetStateDto : IDedicateMachineDto
{
    public readonly Guid InstanceId;
    public readonly InstanceState InstanceState;

    public InstanceSetStateDto(Guid instanceId, InstanceState instanceState)
    {
        InstanceId = instanceId;
        InstanceState = instanceState;
    }
}