namespace Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Repository { get; set; }

    public string BuildScript { get; set; }

    public List<Build> Builds { get; set; } = new();
    
    public Instance Instance { get; private init; }
}