using System.Runtime.Serialization;
using Domain.Entities;
using Domain.Services;
using Domain.Tools;
using Microsoft.AspNetCore.Mvc;

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
    public IEnumerable<Project> List()
    {
        return _service.GetProjects();
    }

    [HttpGet]
    public ActionResult<Project> Get(string name)
    {
        var project = _service.GetProject(name);
        return project;
    }

    [HttpPost]
    public ActionResult<Project> Create(string name, string buildScript)
    {
        return _service.CreateProject(name, buildScript);
    }
    
    [HttpPost]
    public ActionResult Edit(Guid id, string name, string repository, string buildScript)
    {
        _service.EditProject(id, name, repository, buildScript);
        return Ok();
    }

    [HttpPost]
    public ActionResult Delete(Guid id)
    {
        _service.DeleteProject(id);
        return Ok();
    }

    [HttpPost]
    public ActionResult<Build> AddBuild(Guid projectId, string buildName, Guid storageId)
    {
        return _service.AddBuild(projectId, buildName, storageId);
    }
}