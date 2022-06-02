using Domain.Entities;
using Domain.States;

namespace Domain.Services;
public interface IInstanceService
{
    void ChangeInstanceState(Guid projectId, InstanceState newState);
    InstanceConfig GetConfiguration(Guid projectId);
    void UpdateConfig(Guid projectId, InstanceConfig newConfig);
    IReadOnlyCollection<InstanceState> ListAllStates(Guid projectId);
    IReadOnlyCollection<InstanceState> ListLastStates(Guid projectId, int numberOfStates);
}