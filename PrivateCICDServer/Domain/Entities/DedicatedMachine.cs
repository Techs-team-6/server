namespace Domain.Entities;

public class DedicatedMachine
{
    public Guid Id { get; set; }
    public Guid TokenId { get; set; }
    
    public string Label { get; set; }
    public string Description { get; set; }
    public DedicatedMachineState State { get; set; }

    public enum DedicatedMachineState
    {
        Offline,
        Online,
    }
}