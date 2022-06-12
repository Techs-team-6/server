namespace Domain.Dto.DedicatedMachineDto;

public class InstanceStdOutDto : IDedicateMachineDto
{
    public readonly Guid InstanceId;
    public readonly string Message;
    
    public InstanceStdOutDto(Guid instanceId, string message)
    {
        InstanceId = instanceId;
        Message = message;
    }
}