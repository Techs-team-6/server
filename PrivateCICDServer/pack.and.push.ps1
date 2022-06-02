param(
    $version
)

function PackAndPush ($ProjectName)
{
    dotnet pack --configuration Release $ProjectName /p:Version=$version
    dotnet nuget push "$ProjectName\bin\Release\$ProjectName.$version.nupkg" --source "github"
}

PackAndPush "Domain"
PackAndPush "DMConnect.Client"
PackAndPush "DMConnect.Share"