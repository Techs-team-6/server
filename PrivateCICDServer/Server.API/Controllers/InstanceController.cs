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
        try
        {
            _service.CreateInstance(projectId, config);
            return Ok();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
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
        try
        {
            _service.UpdateConfig(instanceId, instanceConfig);
            return Ok();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
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
        try
        {
            return _service.ListAllStates(instanceId).ToList();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    public ActionResult<IReadOnlyCollection<InstanceState>> ListLastStates(Guid instanceId, int count)
    {
        try
        {
            return _service.ListLastStates(instanceId, count).ToList();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }
}