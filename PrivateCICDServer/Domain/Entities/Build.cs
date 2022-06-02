using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Build
{
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid ProjectId { get; set; }
    [JsonIgnore]
    public Project Project { get; set; }
    public string Name { get; set; }
    public Guid StorageId { get; set; }
}