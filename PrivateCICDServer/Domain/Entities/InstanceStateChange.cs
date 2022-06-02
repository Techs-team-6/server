using Domain.States;

namespace Domain.Entities;

public class InstanceStateChange
{
    public Guid Id { get; set; }
    public DateTime ChangeDate { get; set; }
    public InstanceState PreviousState { get; set; }
    public InstanceState CurrentState { get; set; }
}