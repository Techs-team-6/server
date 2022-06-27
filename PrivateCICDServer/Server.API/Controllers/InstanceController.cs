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
    public ActionResult CreateInstance(Guid projectId, string name, InstanceConfig config)
    {
        try
        {
            _service.CreateInstance(projectId, name,  config);
            return Ok();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public void DeleteInstance(Guid instanceId)
    {
        _service.DeleteInstance(instanceId);
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
        try
        {
            return _service.GetConfiguration(instanceId);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<InstanceState>> ListAllStates(Guid instanceId)
    {
        return _service.ListAllStates(instanceId).ToList();
    }

    [HttpGet]
    public void StartInstance(Guid instanceId)
    {
        _service.StartInstance(instanceId);
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<InstanceState>> ListLastStates(Guid instanceId, int count)
    {
        return _service.ListLastStates(instanceId, count).ToList();
    }
}