namespace Domain.Entities;

public class InstanceConfig
{
    public InstanceConfig(Guid instanceId, Build build, DedicatedMachine dedicatedMachine, string startString)
    {
        InstanceId = instanceId;
        Build = build;
        DedicatedMachine = dedicatedMachine;
        StartString = startString;
    }

    public Guid InstanceId { get; private init; }
    public Build Build { get; set; }
    public DedicatedMachine DedicatedMachine { get; set; }
    public string StartString { get; set; }
}