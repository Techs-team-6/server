namespace Domain.Entities;

public class Project
{
    public Guid Id { get; }
    public string Name { get; }
    public string Repository { get; }

    public string BuildScript { get; }
    public string StartScript { get; }

    public IReadOnlyList<Build> Builds { get; }

    public Project(Guid id, string name, string repository, string buildScript, string startScript,
        IReadOnlyList<Build> builds)
    {
        Id = id;
        Name = name;
        Repository = repository;
        BuildScript = buildScript;
        StartScript = startScript;
        Builds = builds;
    }
}