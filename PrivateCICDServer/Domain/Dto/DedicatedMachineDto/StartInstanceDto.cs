namespace Domain.Dto.DedicatedMachineDto;

public class StartInstanceDto : IDedicateMachineDto
{
    public readonly Guid InstanceId;
    public readonly string BuildUrl;
    public readonly string StartScript;

    public Action Action => Action.StartInstance;
    
    public StartInstanceDto(Guid instanceId, string buildUrl, string startScript)
    {
        InstanceId = instanceId;
        BuildUrl = buildUrl;
        StartScript = startScript;
    }
}