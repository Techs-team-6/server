using Domain.Tools;

namespace Domain.Entities;

public class Build : IHasId
{
    public Build(string name, Guid storageId)
        : this(Guid.NewGuid(), name, storageId)
    {
    }

    public Build(Guid id, string name, Guid storageId)
    {
        Id = id;
        Name = name;
        StorageId = storageId;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid StorageId { get; set; }
}