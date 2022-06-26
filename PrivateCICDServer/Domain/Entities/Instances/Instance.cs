using Domain.Tools;

namespace Domain.Entities.Instances;

public class Instance : IHasId
{
    public Instance(InstanceConfig instanceConfig)
        : this(Guid.NewGuid(), instanceConfig, InstanceState.NotPublished)
    {
    }
    
    public Instance(Guid id, InstanceConfig instanceConfig, InstanceState state)
        : this(id, instanceConfig, state, new List<InstanceStateChange> {new(state)})
    {
    }


    public Instance(Guid id, InstanceConfig instanceConfig, InstanceState state, List<InstanceStateChange> stateChanges)
    {
        Id = id;
        InstanceConfig = instanceConfig;
        State = state;
        StateChanges = stateChanges;
    }

    // EF constructor
    private Instance(Guid id, InstanceState state)
    {
        Id = id;
        State = state;
    }

    public Guid Id { get; set; }
    public InstanceConfig InstanceConfig { get; set; }
    public InstanceState State { get; set; }
    public List<InstanceStateChange> StateChanges { get; set; }
}