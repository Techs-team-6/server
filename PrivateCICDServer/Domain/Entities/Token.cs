namespace Domain.Entities;

public class Token
{
    public Guid Id { get; set; }
    public string TokenStr { get; set; }
    public string Description { get; set; }
    public DateTime CreationTime { get; set; }
}