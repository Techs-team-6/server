using Domain.States;

namespace Domain.Entities;
public class Instance
{
    private readonly List<InstanceStateChange> _changes;
    public Instance(InstanceConfig instanceConfig)
    {
        InstanceConfig = instanceConfig;
        _changes = new List<InstanceStateChange>();
    }
    public InstanceConfig InstanceConfig { get; set; }
    public InstanceState State { get; private set; }

    public IReadOnlyCollection<InstanceStateChange> StateChanges => _changes;

    public void ChangeInstanceState(InstanceState newState)
    {
        var change = new InstanceStateChange(DateTime.Now, State, newState);
        State = newState;
        _changes.Add(change);
    }
}