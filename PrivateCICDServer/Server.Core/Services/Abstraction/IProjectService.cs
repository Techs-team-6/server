using Domain.Entities;

namespace Server.Core.Services.Abstraction;

public interface IProjectService
{
    public void RegisterProject(Project project);

    public IReadOnlyList<Project> GetProjects();

    public Project GetProject(Guid id);

    public void UpdateProject(Project project);

    public void DeleteProject(Guid id);

    public IReadOnlyList<Project> FindProjects(string substring);
}