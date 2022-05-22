using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Core;

public sealed class ServerDBContext : DbContext
{
    public ServerDBContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<Project> Projects { get; }
    public DbSet<Build> Builds { get; }
}