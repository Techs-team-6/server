using Domain.Services;

namespace Server.Core.Services;

public class DummyBuildingService : IBuildingService
{
    public string CreateProject(Guid projectId, string projectName)
    {
        return projectName + ".git";
    }
}