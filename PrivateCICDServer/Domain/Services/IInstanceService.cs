using Domain.Entities;
using Domain.States;

namespace Domain.Services;
public interface IInstanceService
{
    Instance RegisterInstance(Guid projectId, InstanceConfig instanceConfig, InstanceState initialState);
    void ChangeInstanceState(Guid projectId, Guid instanceId, InstanceState newState);
    InstanceConfig GetConfiguration(Guid projectId, Guid instanceId);
    void UpdateConfig(Guid projectId, Guid instanceId, InstanceConfig newConfig);
    IReadOnlyCollection<InstanceState> ListAllStates(Guid projectId, Guid instanceId);
    IReadOnlyCollection<InstanceState> ListLastStates(Guid projectId, Guid instanceId, int numberOfStates);
}