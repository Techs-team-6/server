namespace Domain.Entities.Instances;

public class InstanceConfig
{
    public InstanceConfig(Guid buildId, Guid dedicatedMachineId, string startString)
    {
        BuildId = buildId;
        DedicatedMachineId = dedicatedMachineId;
        StartString = startString;
    }

    public Guid BuildId { get; set; }
    public Guid DedicatedMachineId { get; set; }
    public string StartString { get; set; }
}