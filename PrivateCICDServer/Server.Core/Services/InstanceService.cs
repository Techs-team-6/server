using Domain.Entities.Instances;
using Domain.Services;
using Domain.Tools;

namespace Server.Core.Services;

public class InstanceService : IInstanceService
{
    private readonly ServerDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IDedicatedMachineService _machineService;

    public InstanceService(ServerDbContext context, IProjectService projectService,
        IDedicatedMachineService machineService)
    {
        _context = context;
        _projectService = projectService;
        _machineService = machineService;
    }

    public Instance CreateInstance(Guid projectId, InstanceConfig instanceConfig)
    {
        var project = _projectService.GetProject(projectId);
        project.Builds.GetById(instanceConfig.BuildId);
        _machineService.GetDedicatedMachine(instanceConfig.DedicatedMachineId);
        var instance = new Instance(instanceConfig);
    
        project.Instances.Add(instance);
        _context.Instances.Add(instance);
        _context.SaveChanges();
        return instance;
    }

    public void ChangeInstanceState(Guid instanceId, InstanceState newState)
    {
        var instance = _context.Instances.GetById(instanceId);
        instance.State = newState;
        instance.StateChanges.Add(new InstanceStateChange(newState));
        _context.Update(instance);
        _context.SaveChanges();
    }
    
    public InstanceConfig GetConfiguration(Guid instanceId)
    {
        return GetInstance(instanceId).InstanceConfig;
    }
    
    public void UpdateConfig(Guid instanceId, InstanceConfig newConfig)
    {
        var instance = GetInstance(instanceId);
        instance.InstanceConfig = newConfig;
        _context.SaveChanges();
    }
    
    public IReadOnlyCollection<InstanceState> ListAllStates(Guid instanceId)
    {
        return GetInstance(instanceId).StateChanges.Select(change => change.NewState).ToList();
    }
    
    public IReadOnlyCollection<InstanceState> ListLastStates(Guid instanceId, int numberOfStates)
    {
        var changes = ListAllStates(instanceId).ToList();
        return changes.GetRange(changes.Count - numberOfStates, numberOfStates);
    }

    private Instance GetInstance(Guid id)
    {
        return _context.Instances.GetById(id);
    }
}