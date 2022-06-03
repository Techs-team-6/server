namespace Domain.Entities;

public class Build
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid StorageId { get; set; }
}