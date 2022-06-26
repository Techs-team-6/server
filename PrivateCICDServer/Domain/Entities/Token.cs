using Domain.Tools;

namespace Domain.Entities;

public class Token : IHasId
{
    public Token(string tokenStr, string description)
        : this(Guid.NewGuid(), tokenStr, description, DateTime.Now)
    {
    }

    public Token(Guid id, string tokenStr, string description, DateTime creationTime)
    {
        Id = id;
        TokenStr = tokenStr;
        Description = description;
        CreationTime = creationTime;
    }

    public Guid Id { get; set; }
    public string TokenStr { get; set; }
    public string Description { get; set; }
    public DateTime CreationTime { get; set; }
}