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
    public IReadOnlyCollection<Project> List()
    {
        return _service.GetProjects();
    }
    
    [HttpGet]
    public ActionResult Get(string name)
    {
        // TODO Show ServiceException messages as a popup for user
        try
        {
            var result = _service.GetProject(name);
            var response = Ok(result.Name);
            Console.WriteLine(response.StatusCode);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Project can't be returned");
        }
    }

    [HttpPost]
    public ActionResult Create(string name, string buildScript)
    {
        try
        {
            var result = _service.CreateProject(name, buildScript);
            var response = Ok(result.Name);
            Console.WriteLine(response.StatusCode);
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Project can't be created");
        }
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
    public void AddBuild(Guid projectId, string buildName, Guid storageId)
    {
        _service.AddBuild(projectId, buildName, storageId);
    }
}