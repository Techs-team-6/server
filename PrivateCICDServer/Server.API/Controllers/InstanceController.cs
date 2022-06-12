using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using InstanceConfig = Domain.Entities.InstanceConfig;
using InstanceState = Domain.States.InstanceState;

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
    public ActionResult RegisterInstance(Guid projectId, InstanceState initialState, string startString, Guid buildId, Guid machineId)
    {
        try
        {
            _service.RegisterInstance(projectId, initialState, startString, buildId, machineId);
            return Ok();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public ActionResult ChangeInstanceState(Guid projectId, Guid instanceId, InstanceState newState)
    {
        _service.ChangeInstanceState(projectId, instanceId, newState);
        return Ok();
    }

    [HttpPost]
    public ActionResult UpdateConfiguration(Guid projectId, Guid instanceId, InstanceConfig instanceConfig)
    {
        try
        {
            _service.UpdateConfig(projectId, instanceId, instanceConfig);
            return Ok();
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public ActionResult<InstanceConfig> GetConfiguration(Guid projectId, Guid instanceId)
    {
        try
        {
            return _service.GetConfiguration(projectId, instanceId);
        }
        catch (ArgumentOutOfRangeException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListAllStates(Guid projectId, Guid instanceId)
    {
        // todo rewrite to ActionResult
        return _service.ListAllStates(projectId, instanceId);
    }
    
    [HttpGet]
    public IReadOnlyCollection<InstanceState> ListLastStates(Guid projectId, Guid instanceId, int count)
    {
        // todo rewrite to ActionResult
        return _service.ListLastStates(projectId, instanceId, count);
    }
}