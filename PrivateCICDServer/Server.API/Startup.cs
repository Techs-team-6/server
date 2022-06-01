﻿using Domain.Services;
using Microsoft.EntityFrameworkCore;
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