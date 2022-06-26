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
    public ActionResult RegisterInstance(Guid projectId, InstanceConfig config)
    {
        _service.CreateInstance(projectId, config);
        return Ok();
    }

    [HttpPost]
    public ActionResult ChangeInstanceState(Guid instanceId, InstanceState newState)
    {
        _service.ChangeInstanceState(instanceId, newState);
        return Ok();
    }
    
    [HttpPost]
    public ActionResult UpdateConfiguration(Guid instanceId, InstanceConfig instanceConfig)
    {
        _service.UpdateConfig(instanceId, instanceConfig);
        return Ok();
    }
    
    [HttpGet]
    public ActionResult<InstanceConfig> GetConfiguration(Guid instanceId)
    {
        return _service.GetConfiguration(instanceId);
    }
    
    [HttpGet]
    public ActionResult<IReadOnlyCollection<InstanceState>> ListAllStates(Guid instanceId)
    {
        return _service.ListAllStates(instanceId).ToList();
    }
    
    [HttpGet]
    public ActionResult<IReadOnlyCollection<InstanceState>> ListLastStates(Guid instanceId, int count)
    {
        return _service.ListLastStates(instanceId, count).ToList();
    }
}