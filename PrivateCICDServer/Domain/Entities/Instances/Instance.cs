using Domain.Tools;

namespace Domain.Entities.Instances;

public class Instance : IHasId
{
    public Instance(string name, InstanceConfig instanceConfig)
        : this(Guid.NewGuid(), name, instanceConfig, InstanceState.NotPublished)
    {
    }

    public Instance(Guid id, string name, InstanceConfig instanceConfig, InstanceState state)
        : this(id, name, instanceConfig, state, new List<InstanceStateChange> { new(state) })
    {
    }


    public Instance(Guid id, string name, InstanceConfig instanceConfig, InstanceState state,
        List<InstanceStateChange> stateChanges)
    {
        Id = id;
        Name = name;
        InstanceConfig = instanceConfig;
        State = state;
        StateChanges = stateChanges;
    }

    // EF constructor
    private Instance(Guid id, string name, InstanceState state)
    {
        Id = id;
        Name = name;
        State = state;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public InstanceConfig InstanceConfig { get; set; }
    public InstanceState State { get; set; }
    public List<InstanceStateChange> StateChanges { get; set; }
}