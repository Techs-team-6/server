using Domain.Entities;

namespace Server.Core.Services.Abstraction;

public interface IProjectService
{
    public Project CreateProject(string name, string buildScript);

    public IReadOnlyList<Project> GetProjects();

    public Project GetProject(Guid id);
    
    public Project GetProject(string name);

    public void EditProject(Guid id, string name, string repository, string buildScript);

    public void DeleteProject(Guid id);

    public IReadOnlyList<Project> FindProjects(string substring);
}