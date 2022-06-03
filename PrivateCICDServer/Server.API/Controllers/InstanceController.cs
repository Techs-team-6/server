using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using InstanceConfig = Domain.Entities.InstanceConfig;
using InstanceState = Domain.States.InstanceState;

namespace Server.API.Controllers;


[ApiController]
[Route("[controller]/[action]")]
public class InstanceController
{
    private readonly IInstanceService _service;
    public InstanceController(IInstanceService service)
    {
        _service = service;
    }

    [HttpPost]
    public void RegisterInstance(Guid projectId, InstanceState initialState, string startString, Guid buildId, Guid machineId)
    {
        _service.RegisterInstance(projectId, initialState, startString, buildId, machineId);
    }
    
    [HttpPost]
    public void ChangeInstanceState(Guid projectId, Guid instanceId, InstanceState newState)
    {
        _service.ChangeInstanceState(projectId, instanceId, newState);
    }

    [HttpPost]
    public void UpdateConfiguration(Guid projectId, Guid instanceId, InstanceConfig instanceConfig)
    {
        _service.UpdateConfig(projectId, instanceId, instanceConfig);
    }

    [HttpGet]
    public InstanceConfig GetConfiguration(Guid projectId, Guid instanceId)
    {
        return _service.GetConfiguration(projectId, instanceId);
    }

    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListAllStates(Guid projectId, Guid instanceId)
    {
        return _service.ListAllStates(projectId, instanceId);
    }
    
    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListLastStates(Guid projectId, Guid instanceId, int count)
    {
        return _service.ListLastStates(projectId, instanceId, count);
    }
}