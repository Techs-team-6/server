using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Services.Abstraction;

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
    public Project Get(string name)
    {
        // TODO Show ServiceException messages as a popup for user
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
}