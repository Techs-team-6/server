using Microsoft.EntityFrameworkCore;
using Server.Core;
using Server.Core.Services.Implementations;
using Server.Core.Services.Interfaces;

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