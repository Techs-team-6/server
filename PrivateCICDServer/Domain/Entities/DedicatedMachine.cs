using Domain.Tools;

namespace Domain.Entities;

public class DedicatedMachine : IHaveId
{
    public DedicatedMachine(Guid tokenId, string label, string description)
        : this(Guid.NewGuid(), tokenId, label, description, DedicatedMachineState.Offline)
    {
    }

    public DedicatedMachine(Guid id, Guid tokenId, string label, string description, DedicatedMachineState state)
    {
        Id = id;
        TokenId = tokenId;
        Label = label;
        Description = description;
        State = state;
    }

    public Guid Id { get; set; }
    public Guid TokenId { get; set; }
    public string Label { get; set; }
    public string Description { get; set; }
    public DedicatedMachineState State { get; set; }
}