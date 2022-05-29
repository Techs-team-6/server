using System;

namespace Domain.Entities;

public class Instance
{
    public Instance(Guid id, Guid tokenId, string description)
    {
        Id = id;
        TokenId = tokenId;
        Description = description;
    }
    
    public Guid TokenId { get; private init; }
    public Guid Id { get; private init; }
    public string Description { get; set; }
}