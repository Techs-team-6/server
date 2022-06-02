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
    public DbSet<DedicatedMachine> DedicatedMachines { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>()
            .HasMany(t => t.Instances);
        builder.Entity<Instance>()
            .OwnsMany(t => t.StateChanges);
        builder.Entity<Project>()
            .HasMany(t => t.Builds);

        builder.Entity<Token>();
        builder.Entity<DedicatedMachine>();
    }
}