using System.Runtime.Serialization;
using Domain.Entities;
using Domain.Services;
using Server.Core.Tools;

namespace Server.Core.Services;

public class ProjectService : IProjectService
{
    private readonly ServerDBContext _context;
    private readonly IBuildingService _buildingService;

    public ProjectService(ServerDBContext context, IBuildingService buildingService)
    {
        _context = context;
        _buildingService = buildingService;
    }

    public Project CreateProject(string name, string buildScript)
    {
        if (_context.Projects.Any(p => p.Name.Equals(name)))
            throw new SerializationException("There is another project with such name");
        
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            BuildScript = buildScript,
        };
        project.Repository = _buildingService.CreateProject(project.Id, name);
        
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }

    public IReadOnlyList<Project> GetProjects()
    {
        return _context.Projects.ToList();
    }

    public Project GetProject(Guid id)
    {
        return _context.Projects.FirstOrDefault(p => p.Id.Equals(id))
               ?? throw new ServiceException($"Project with id '{id}' does not exist.");
    }

    public Project GetProject(string name)
    {
        return _context.Projects.FirstOrDefault(p => p.Name.Equals(name))
               ?? throw new ServiceException($"Project with name '{name}' does not exist.");
    }

    public void EditProject(Guid id, string name, string repository, string buildScript)
    {
        var project = GetProject(id);
        project.Name = name;
        project.Repository = repository;
        project.BuildScript = buildScript;
        _context.SaveChanges();
    }

    public void DeleteProject(Guid id)
    {
        var projectToDelete = _context.Projects.FirstOrDefault(p => p.Id.Equals(id))
                              ?? throw new Exception($"Project with id '{id}' does not exist.");
        _context.Projects.Remove(projectToDelete);
        _context.SaveChanges();
    }

    public IReadOnlyList<Project> FindProjects(string substring)
    {
        return _context.Projects.Where(p => p.Name.Contains(substring)).ToList();
    }

    public void AddBuild(Guid projectId, string buildName, Guid storageId)
    {
        var project = GetProject(projectId);

        var build = new Build
        {
            Name = buildName,
            Project = project,
            ProjectId = project.Id,
            StorageId = storageId,
        };
        
        project.Builds.Add(build);
        _context.SaveChanges();
    }
}