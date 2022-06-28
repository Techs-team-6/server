using Domain.Entities.Instances;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class InstanceController : ControllerBase
{
    private readonly IInstanceService _service;

    public InstanceController(IInstanceService service)
    {
        _service = service;
    }

    [HttpPost]
    public Instance CreateInstance(Guid projectId, string name, InstanceConfig config)
    {
        return _service.CreateInstance(projectId, name, config);
    }

    [HttpGet]
    public void DeleteInstance(Guid instanceId)
    {
        _service.DeleteInstance(instanceId);
    }

    [HttpPost]
    public void ChangeInstanceState(Guid instanceId, InstanceState newState)
    {
        _service.ChangeInstanceState(instanceId, newState);
    }

    [HttpPost]
    public void UpdateConfiguration(Guid instanceId, InstanceConfig instanceConfig)
    {
        _service.UpdateConfig(instanceId, instanceConfig);
    }

    [HttpGet]
    public InstanceConfig GetConfiguration(Guid instanceId)
    {
        return _service.GetConfiguration(instanceId);
    }

    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListAllStates(Guid instanceId)
    {
        return _service.ListAllStates(instanceId).ToList();
    }

    [HttpGet]
    public void StartInstance(Guid instanceId)
    {
        _service.StartInstance(instanceId);
    }

    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListLastStates(Guid instanceId, int count)
    {
        return _service.ListLastStates(instanceId, count).ToList();
    }
}