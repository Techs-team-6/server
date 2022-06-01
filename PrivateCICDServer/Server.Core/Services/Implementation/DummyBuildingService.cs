using Server.Core.Services.Abstraction;

namespace Server.Core.Services.Implementation;

public class DummyBuildingService : IBuildingService
{
    public string CreateProject(Guid projectId, string projectName)
    {
        return projectName + ".git";
    }
}