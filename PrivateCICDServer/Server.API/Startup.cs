using Domain.Services;
using ProjectServiceApiClient;
using Server.API.Services;
using Server.Core;
using Server.Core.Services;

namespace Server.API;

public class Startup
{
    public static void Main(string[] args)
    {
        Environment.SetEnvironmentVariable("WITHOUT_PS", "true");

        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .Build()
            .Run();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Settings>();

        services.AddDbContext<ServerDbContext>(
            (provider, options) =>
            {
                var settings = provider.GetRequiredService<Settings>();
                settings.UseDbAction(options);
            });

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IBuildingService, DummyBuildingService>();
        services.AddScoped<IMasterService, MasterService>();
        services.AddScoped(provider =>
        {
            var settings = provider.GetRequiredService<Settings>();
            return new ProjectServiceClient(settings.ProjectBuildingServiceUrl, new HttpClient());
        });

        services.AddScoped<IDedicatedMachineService, DedicatedMachineService>();
        services.AddScoped<IInstanceService, InstanceService>();

        services.AddHostedService<HostedHubService>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}