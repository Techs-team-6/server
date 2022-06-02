namespace Domain.Dto.DedicatedMachineDto;

public class InstanceStdErrDto : IDedicateMachineDto
{
    public readonly Guid InstanceId;
    public readonly string Message;

    public Action Action => Action.InstanceStdErr;
    
    public InstanceStdErrDto(Guid instanceId, string message)
    {
        InstanceId = instanceId;
        Message = message;
    }
}