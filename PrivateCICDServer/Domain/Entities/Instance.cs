using Domain.States;

namespace Domain.Entities;
public class Instance
{
    public Guid Id { get; set; } = Guid.NewGuid();
    // public Guid ProjectId => Project.Id;
    // public Project Project { get; set; }
    public InstanceConfig InstanceConfig { get; set; }
    public InstanceState State { get; private set; }

    public List<InstanceStateChange> StateChanges { get; } = new();

    public void ChangeInstanceState(InstanceState newState)
    {
        var change = new InstanceStateChange
        {
            Id = Guid.NewGuid(),
            PreviousState = State,
            CurrentState = State,
            ChangeDate = DateTime.Now,
        };
        State = newState;
        StateChanges.Add(change);
    }
}