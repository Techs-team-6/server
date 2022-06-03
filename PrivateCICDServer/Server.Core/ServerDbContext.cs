﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Core;

public class ServerDbContext : DbContext
{
    public ServerDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    private DbSet<Build> Builds { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;
    public DbSet<DedicatedMachine> DedicatedMachines { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Project>()
            .HasMany(p => p.Builds);

        builder.Entity<Project>()
            .OwnsMany(t => t.Instances)
            .OwnsOne(t => t.InstanceConfig)
            .HasOne(t => t.DedicatedMachine);

        builder.Entity<Project>()
            .OwnsMany(t => t.Instances)
            .OwnsOne(t => t.InstanceConfig)
            .HasOne(t => t.Build);

        builder.Entity<Project>()
            .OwnsMany(t => t.Instances)
            .OwnsMany(t => t.StateChanges);

        builder.Entity<Token>();
        builder.Entity<DedicatedMachine>();
    }
}