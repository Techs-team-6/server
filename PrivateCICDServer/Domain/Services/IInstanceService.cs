using Domain.Entities.Instances;

namespace Domain.Services;

public interface IInstanceService
{
    Instance CreateInstance(Guid projectId, InstanceConfig instanceConfig);
    void ChangeInstanceState(Guid instanceId, InstanceState newState);
    InstanceConfig GetConfiguration(Guid instanceId);
    void UpdateConfig(Guid instanceId, InstanceConfig newConfig);
    IReadOnlyCollection<InstanceState> ListAllStates(Guid instanceId);
    IReadOnlyCollection<InstanceState> ListLastStates(Guid instanceId, int numberOfStates);
}