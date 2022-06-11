using Server.Core;

namespace Server.API;

public class Program
{
    public static void Main(string[] args)
    {
        Environment.SetEnvironmentVariable("WITHOUT_PS", "true");
        var host = CreateHostBuilder(args).Build();

        CreateDbIfNotExists(host);

        host.Run();
    }

    private static void CreateDbIfNotExists(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ServerDbContext>();
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}