namespace Domain.Entities;

public class Build
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; }
    
    public string Name { get; set; }
    public Guid StorageId { get; set; }
}