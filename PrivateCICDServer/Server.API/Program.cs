using DMConnect.Server;
using Domain.Services;
using Server.Core;

namespace Server.API;

public class Program
{
    public static void Main(string[] args)
    {
        Environment.SetEnvironmentVariable("WITHOUT_PS", "true");
        var host = CreateHostBuilder(args).Build();

        CreateDbIfNotExists(host);
        
        // When we launch API, we do not use arguments, hub works
        // During the NSwag code gen because of DedicatedMachineHub build got stacked
        // #TechnicalDebt
        if (args.Length == 0)
            StartDmHub(host);

        host.Run();
    }
    
    private static void StartDmHub(IHost host)
    {
        var scope = host.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IDedicatedMachineService>();
        var hub = new DedicatedMachineHub(service, 50050);
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