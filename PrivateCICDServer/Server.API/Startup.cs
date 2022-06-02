using DMConnect.Server;
using Domain.Services;
using Domain.States;
using Microsoft.EntityFrameworkCore;
using ProjectServiceApiClient;
using Server.Core;
using Server.Core.Services;

namespace Server.API;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ServerDBContext>(
            options => options.UseSqlite(Configuration.GetConnectionString("Sqlite")!));

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IBuildingService, DummyBuildingService>();
        services.AddScoped<IMasterService, MasterService>();
        services.AddScoped(serviceProvider => new ProjectServiceClient(
            serviceProvider.GetService<IConfiguration>()!["ProjectBuildingServiceUrl"]!, new HttpClient()));

        services.AddScoped<IDedicatedMachineService, DedicatedMachineService>();
        services.AddScoped<IInstanceService, InstanceService>();
        // services.AddScoped(serviceProvider => new DedicatedMachineHub(
        //     serviceProvider.GetRequiredService<IDedicatedMachineService>(), 50050));

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

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        // app.UseOpenApi();
        // app.UseSwaggerUi3();
    }
}