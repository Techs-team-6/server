namespace Domain.Entities.Instances;

public class InstanceStateChange
{
    public InstanceStateChange(InstanceState newState)
        : this(Guid.NewGuid(), DateTime.Now, newState)
    {
    }

    public InstanceStateChange(Guid id, DateTime changeDate, InstanceState newState)
    {
        Id = id;
        ChangeDate = changeDate;
        NewState = newState;
    }

    public Guid Id { get; set; }
    public DateTime ChangeDate { get; set; }
    public InstanceState NewState { get; set; }
}