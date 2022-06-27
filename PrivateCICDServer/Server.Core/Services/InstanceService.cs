using DMConnect.Server;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities.Instances;
using Domain.Services;
using Domain.Tools;
using Microsoft.Extensions.Configuration;

namespace Server.Core.Services;

public class InstanceService : IInstanceService
{
    private readonly ServerDbContext _context;
    private readonly IProjectService _projectService;
    private readonly IDedicatedMachineService _machineService;
    private readonly INameValidatorService _nameValidatorService;
    private readonly IConfiguration _configuration;

    public InstanceService(ServerDbContext context, IProjectService projectService,
        IDedicatedMachineService machineService, INameValidatorService nameValidatorService,
        IConfiguration configuration)
    {
        _context = context;
        _projectService = projectService;
        _machineService = machineService;
        _nameValidatorService = nameValidatorService;
        _configuration = configuration;
    }

    public Instance CreateInstance(Guid projectId, string name, InstanceConfig instanceConfig)
    {
        if (!_nameValidatorService.IsValidInstanceName(name))
            throw new ServiceException($"Invalid instance name '{name}'");
        var project = _projectService.GetProject(projectId);
        var build = project.Builds.GetById(instanceConfig.BuildId);
        _machineService.GetDedicatedMachine(instanceConfig.DedicatedMachineId);
        var instance = new Instance(name, instanceConfig);

        project.Instances.Add(instance);
        _context.Instances.Add(instance);
        _context.SaveChanges();

        return instance;
    }

    public void DeleteInstance(Guid id)
    {
        var instance = GetInstance(id);
        _context.Remove(instance);
        _context.SaveChanges();
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

    public void StartInstance(Guid id)
    {
        var instance = GetInstance(id);
        var build = _projectService.GetProjects()
            .SelectMany(project => project.Builds)
            .GetById(instance.InstanceConfig.BuildId);

        var hub = DedicatedMachineHub.Instance;
        var machineAgent = hub.GetMachineAgent(instance.InstanceConfig.DedicatedMachineId);
        machineAgent.StartInstance(new StartInstanceDto(
            instance.Id,
            $"{_configuration["ProjectBuildingServiceUrl"]}/api/v1/DownloadZipFile/{build.StorageId}",
            instance.InstanceConfig.StartString
        ));
    }

    private Instance GetInstance(Guid id)
    {
        return _context.Instances.GetById(id);
    }
}