namespace Domain.Entities;

public class InstanceConfig
{
    // public Guid Id { get; set; }
    // public Guid InstanceId { get; private set; }
    // public Guid BuildId { get; set; }
    public Build Build { get; set; }
    public DedicatedMachine DedicatedMachine { get; set; }
    public string StartString { get; set; }
}