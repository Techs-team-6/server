using Domain.States;

namespace Domain.Entities;

public class InstanceStateChange
{
    public InstanceStateChange()
    {
        
    }
    public InstanceStateChange(DateTime changeDate, InstanceState previousState, InstanceState currentState)
    {
        ChangeDate = changeDate;
        PreviousState = previousState;
        CurrentState = currentState;
    }
    public Guid Id { get; set; }
    public DateTime ChangeDate { get; set; }
    public InstanceState PreviousState { get; set; }
    public InstanceState CurrentState { get; set; }
}