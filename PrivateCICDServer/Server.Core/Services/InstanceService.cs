using Domain.Entities;
using Domain.Services;
using Domain.States;

namespace Server.Core.Services;

public class InstanceService : IInstanceService
{
    private readonly ServerDbContext _context;
    public InstanceService(ServerDbContext context)
    {
        _context = context;
    }
    public Instance RegisterInstance(Guid projectId, InstanceConfig instanceConfig, InstanceState initialState)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                      throw new ArgumentOutOfRangeException(nameof(projectId));
        var instance = new Instance()
        {
            Id = Guid.NewGuid(),
            InstanceConfig = instanceConfig,
            StateChanges = {}
        };

        instance.ChangeInstanceState(initialState);

        project.Instances.Add(instance);
        _context.SaveChanges();
        return instance;
    }
    public void ChangeInstanceState(Guid projectId, Guid instanceId, InstanceState newState)
    {
        _context.Projects.FirstOrDefault(p => p.Id == projectId)?
            .Instances.FirstOrDefault(i => i.Id == instanceId)?
            .ChangeInstanceState(newState);
        _context.SaveChanges();
    }
    public InstanceConfig GetConfiguration(Guid projectId, Guid instanceId)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                      throw new ArgumentOutOfRangeException(nameof(projectId));
        var instance = project.Instances.FirstOrDefault(i => i.Id == instanceId) ?? 
                       throw new ArgumentOutOfRangeException(nameof(instanceId));
        return instance.InstanceConfig;
    }
    public void UpdateConfig(Guid projectId, Guid instanceId, InstanceConfig newConfig)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                      throw new ArgumentOutOfRangeException(nameof(projectId));
        var instance = project.Instances.FirstOrDefault(i => i.Id == instanceId) ?? 
                       throw new ArgumentOutOfRangeException(nameof(instanceId));
       instance.InstanceConfig = newConfig;
        _context.SaveChanges();
    }
    public IReadOnlyCollection<InstanceState> ListAllStates(Guid projectId, Guid instanceId)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                      throw new ArgumentOutOfRangeException(nameof(projectId));
        var instance = project.Instances.FirstOrDefault(i => i.Id == instanceId) ?? 
                       throw new ArgumentOutOfRangeException(nameof(instanceId));
        return instance.StateChanges.Select(change => change.CurrentState).ToList();
    }
    public IReadOnlyCollection<InstanceState> ListLastStates(Guid projectId, Guid instanceId, int numberOfStates)
    {
        var project =  _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                                       throw new ArgumentOutOfRangeException(nameof(projectId));
        var instance = project.Instances.FirstOrDefault(i => i.Id == instanceId) ?? 
                       throw new ArgumentOutOfRangeException(nameof(instanceId));
        var instances = instance.StateChanges.Select(change => change.CurrentState).ToList();
        return instances.GetRange(instances.Count - numberOfStates, numberOfStates);
    }
}