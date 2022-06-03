using Domain.Dto.Responses;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public ProjectResponse Get(string name)
    {
        // TODO Show ServiceException messages as a popup for user
        try
        {
            var project = _service.GetProject(name);
            var response = new ProjectResponse
            {
                StatusCode = 200,   // todo get rid of magic values
                Description = "OK",
                Project = project,
            };

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = new ProjectResponse
            {
                StatusCode = 228,   // todo get rid of magic values
                Description = "Error",
                Project = null,
            };

            return response;
        }
    }

    [HttpPost]
    public ProjectResponse Create(string name, string buildScript)
    {
        try
        {
            var project = _service.CreateProject(name, buildScript);
            
            var response = new ProjectResponse
            {
                StatusCode = 200,   // todo get rid of magic values
                Description = "OK",
                Project = project,
            };

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = new ProjectResponse
            {
                StatusCode = 228,   // todo get rid of magic values
                Description = "Error",
                Project = null,
            };

            return response;
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