using Domain.Entities;
using Server.Core.Services.Abstraction;

namespace Server.Core.Services.Implementation;

public class ProjectService : IProjectService
{
    private ServerDBContext _context;

    public void RegisterProject(Project project)
    {
        if (_context.Projects.Any(p => p.Id.Equals(project.Id)))
            throw new Exception($"Project with id '{project.Id}' already exists.");
        
        _context.Projects.Add(
            new Project 
            {
                Id = project.Id,
                Name = project.Name,
                Repository = project.Repository,
                BuildScript = project.BuildScript,
                StartScript = project.StartScript,
                Builds = project.Builds.ToList(),
                
            });
        
        _context.SaveChanges();
    }

    public IReadOnlyList<Project> GetProjects()
    {
        // todo build list deep copy?
        return _context.Projects.ToList();
    }

    public Project GetProject(Guid id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id.Equals(id))
                      ?? throw new Exception($"Project with id '{id}' does not exist.");

        // todo create a new object?
        return project;
    }

    public void UpdateProject(Project project)
    {
        var projectToUpdate = _context.Projects.FirstOrDefault(p => p.Id.Equals(project.Id))
            ?? throw new Exception($"Project with id '{project.Id}' does not exist.");
        
        projectToUpdate.Name = project.Name;
        projectToUpdate.Repository = project.Repository;
        projectToUpdate.BuildScript = project.BuildScript;
        projectToUpdate.StartScript = project.StartScript;
        projectToUpdate.Builds = project.Builds.ToList();

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
}