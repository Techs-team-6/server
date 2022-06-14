using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Server.API.Services;

public class Settings
{
    public string ProjectBuildingServiceUrl { get; }
    public Action<DbContextOptionsBuilder> UseDbAction { get; }
    public int DmConnectHubPort { get; }

    public Settings(IConfiguration configuration)
    {
        ProjectBuildingServiceUrl = configuration.GetValue<string>("ProjectBuildingServiceUrl");
        UseDbAction = ParseDatabaseConfig(configuration.GetSection("Database"));
        DmConnectHubPort = configuration.GetValue<int>("DMConnectHubPort");
    }

    private static Action<DbContextOptionsBuilder> ParseDatabaseConfig(IConfiguration databaseConfig)
    {
        var configurationName = databaseConfig.GetValue<string>("Configuration");
        var configuration = databaseConfig.GetSection(configurationName);

        var databaseProvider = configuration.GetValue<string>("Provider");
        var connectionString = ParseConnectionString(configuration);

        return databaseProvider switch
        {
            "MySQL" => options => options.UseMySQL(connectionString),
            "Sqlite" => options => options.UseSqlite(connectionString),
            _ => throw new Exception($"Unknown database provider '{databaseProvider}'")
        };
    }

    private static string ParseConnectionString(IConfiguration configuration)
    {
        var connectionStringTemplate = configuration.GetValue<string>("ConnectionString");
        return Regex.Replace(connectionStringTemplate, @"\$\([a-zA-Z_0-9]+\)", match =>
        {
            var name = match.Value.Substring(2, match.Length - 3);
            return configuration.GetValue<string>(name);
        });
    }
}