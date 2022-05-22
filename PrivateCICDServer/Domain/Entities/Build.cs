namespace Domain.Entities;

public class Build
{
    public Guid Id { get; }
    public Project Project { get; }
    public string Name { get; }
    public string BuildUrl { get; }

    public Build(Guid id, Project project, string name, string buildUrl)
    {
        Id = id;
        Project = project;
        Name = name;
        BuildUrl = buildUrl;
    }
}