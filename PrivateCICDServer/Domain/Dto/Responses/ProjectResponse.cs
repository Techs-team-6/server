using Domain.Entities;

namespace Domain.Dto.Responses;

public class ProjectResponse
{
    public int StatusCode { get; set; }
    public string? Description { get; set; }
    public Project? Project { get; set; }
}