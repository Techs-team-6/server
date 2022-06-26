using Domain.Entities.Instances;
using Domain.Tools;

namespace Domain.Entities;

public class Project : IHasId
{
    public Project(string name, string repository, string buildScript) :
        this(Guid.NewGuid(), name, repository, buildScript, new List<Build>(), new List<Instance>())
    {
    }

    public Project(Guid id, string name, string repository, string buildScript, List<Build> builds,
        List<Instance> instances)
    {
        Id = id;
        Name = name;
        Repository = repository;
        BuildScript = buildScript;
        Builds = builds;
        Instances = instances;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Repository { get; set; }
    public string BuildScript { get; set; }
    public List<Build> Builds { get; set; }
    public List<Instance> Instances { get; set; }
}