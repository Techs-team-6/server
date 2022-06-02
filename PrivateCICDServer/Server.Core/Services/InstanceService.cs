using Domain.Entities;
using Domain.Services;
using Domain.States;

namespace Server.Core.Services;

public class InstanceService : IInstanceService
{
    private readonly ServerDBContext _context;
    public InstanceService(ServerDBContext context)
    {
        _context = context;
    }
    public void ChangeInstanceState(Guid projectId, InstanceState newState)
    {
        _context.Projects.FirstOrDefault(p => p.Id == projectId)?.Instance.ChangeInstanceState(newState);
        _context.SaveChanges();
    }
    public InstanceConfig GetConfiguration(Guid projectId)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                      throw new ArgumentOutOfRangeException(nameof(projectId));
        return project.Instance.InstanceConfig;
    }
    public void UpdateConfig(Guid projectId, InstanceConfig newConfig)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                      throw new ArgumentOutOfRangeException(nameof(projectId));
        project.Instance.InstanceConfig = newConfig;
        _context.SaveChanges();
    }
    public IReadOnlyCollection<InstanceState> ListAllStates(Guid projectId)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                      throw new ArgumentOutOfRangeException(nameof(projectId));
        return project.Instance.StateChanges.Select(change => change.CurrentState).ToList();
    }
    public IReadOnlyCollection<InstanceState> ListLastStates(Guid projectId, int numberOfStates)
    {
        var project =  _context.Projects.FirstOrDefault(p => p.Id == projectId) ??
                                       throw new ArgumentOutOfRangeException(nameof(projectId));
        var instances = project.Instance.StateChanges.Select(change => change.CurrentState).ToList();
        return instances.GetRange(instances.Count - numberOfStates, numberOfStates);
    }
}