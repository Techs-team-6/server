using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Core;

// ReSharper disable once InconsistentNaming
public sealed class ServerDBContext : DbContext
{
    public ServerDBContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;
    public DbSet<DedicatedMachine> DedicatedServers { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>()
            .OwnsMany(t => t.Builds)
            .WithOwner(t => t.Project)
            .HasForeignKey(t => t.ProjectId);

        builder.Entity<Token>();
        builder.Entity<DedicatedMachine>();
    }
}