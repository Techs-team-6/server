using System.Runtime.Serialization;
using Domain.Dto.Responses;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Core.Tools;

namespace Server.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public IReadOnlyCollection<Project> List()
    {
        return _service.GetProjects();
    }

    [HttpGet]
    public ActionResult<Project> Get(string name)
    {
        try
        {
            var project = _service.GetProject(name);
            return project;
        }
        catch (ServiceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public ActionResult Create(string name, string buildScript)
    {
        try
        {
            _service.CreateProject(name, buildScript);

            return Ok();
        }
        catch (SerializationException e)
        {
            return BadRequest(e.Message);
        }
        catch (ServiceException e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    [HttpPost]
    public ActionResult Edit(Guid id, string name, string repository, string buildScript)
    {
        try
        {
            _service.EditProject(id, name, repository, buildScript);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public ActionResult Delete(Guid id)
    {
        try
        {
            _service.DeleteProject(id);
            return Ok();
        }
        catch (ServiceException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public ActionResult<Build> AddBuild(Guid projectId, string buildName, Guid storageId)
    {
        try
        {
            return _service.AddBuild(projectId, buildName, storageId);
        }
        catch (ServiceException e)
        {
            return NotFound(e.Message);
        }
    }
}