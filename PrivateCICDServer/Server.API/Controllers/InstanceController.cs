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
    public void ChangeInstanceState(Guid projectId, InstanceState newState)
    {
        _service.ChangeInstanceState(projectId, newState);
    }

    [HttpPost]
    public void UpdateConfiguration(Guid projectId, InstanceConfig instanceConfig)
    {
        _service.UpdateConfig(projectId, instanceConfig);
    }

    [HttpGet]
    public InstanceConfig GetConfiguration(Guid projectId)
    {
        return _service.GetConfiguration(projectId);
    }

    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListAllStates(Guid projectId)
    {
        return _service.ListAllStates(projectId);
    }
    
    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListLastStates(Guid projectId, int count)
    {
        return _service.ListLastStates(projectId, count);
    }
}