﻿using System.Runtime.Serialization;
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
    public ActionResult<Project> Create(string name, string buildScript)
    {
        try
        {
            return _service.CreateProject(name, buildScript);
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