using Domain.Entities;
using Domain.Entities.Instances;
using Domain.Services;
using Domain.Tools;
using Microsoft.EntityFrameworkCore;
using ProjectServiceApiClient;
using ProjectServiceApiClient.Models;

namespace Server.Core.Services;

public class ProjectService : IProjectService
{
    private readonly ServerDbContext _context;
    private readonly ProjectServiceClient _projectServiceClient;
    private readonly INameValidatorService _nameValidatorService = new NameValidatorService();

    public ProjectService(ServerDbContext context, ProjectServiceClient projectServiceClient)
    {
        _context = context;
        _projectServiceClient = projectServiceClient;
    }

    public Project CreateProject(string name, string buildScript)
    {
        if (_context.Projects.Any(p => p.Name.Equals(name)))
            throw new ServiceException($"There is another project with name: '{name}'");

        if (!_nameValidatorService.IsValidProjectName(name))
            throw new ServiceException($"Name '{name}' does not fit the pattern");
        
        var id = Guid.NewGuid();
        string repository;
        if (Environment.GetEnvironmentVariable("WITHOUT_PS") is null)
        {
            repository = _projectServiceClient.CreateAsync(new ProjectCreateDto(id, name, false)).Result
                .ToString();
        }
        else
        {
            repository = name + ".git";
        }
        var project = new Project(id, name, repository, buildScript, new List<Build>(), new List<Instance>());

        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }

    public IEnumerable<Project> GetProjects()
    {
        return _context.Projects
            .Include(p => p.Builds)
            .Include(p => p.Instances);
    }

    public Project GetProject(Guid id)
    {
        return GetProjects().GetById(id);
    }

    public Project GetProject(string name)
    {
        return GetProjects().FirstOrDefault(p => p.Name.Equals(name))
               ?? throw new ServiceException($"Project with name '{name}' does not exist.");
    }

    public void EditProject(Guid id, string name, string repository, string buildScript)
    {
        if (!_nameValidatorService.IsValidProjectName(name))
            throw new ServiceException($"Name '{name}' does not fit the pattern");

        var project = GetProject(id);
        project.Name = name;
        project.Repository = repository;
        project.BuildScript = buildScript;
        _context.Update(project);
        _context.SaveChanges();
    }

    public void DeleteProject(Guid id)
    {
        var projectToDelete = GetProject(id);
        _context.Projects.Remove(projectToDelete);
        _context.SaveChanges();
    }

    public IReadOnlyList<Project> FindProjects(string substring)
    {
        return GetProjects().Where(p => p.Name.Contains(substring)).ToList();
    }

    public Build AddBuild(Guid projectId, string buildName, Guid storageId)
    {
        var project = GetProject(projectId);
        var build = new Build(buildName, storageId);

        project.Builds.Add(build);
        _context.Builds.Add(build);
        _context.SaveChanges();
        return build;
    }
}