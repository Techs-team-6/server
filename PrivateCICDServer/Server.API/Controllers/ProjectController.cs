using Domain.Entities;
using Domain.Services;
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
    public Project Get(string name)
    {
        return _service.GetProject(name);
    }

    [HttpPost]
    public Project Create(string name, string buildScript)
    {
        return _service.CreateProject(name, buildScript);
    }

    [HttpPost]
    public void Edit(Guid id, string name, string repository, string buildScript)
    {
        _service.EditProject(id, name, repository, buildScript);
    }

    [HttpPost]
    public void Delete(Guid id)
    {
        _service.DeleteProject(id);
    }

    [HttpPost]
    public Build AddBuild(Guid projectId, string buildName, Guid storageId)
    {
        return _service.AddBuild(projectId, buildName, storageId);
    }
}