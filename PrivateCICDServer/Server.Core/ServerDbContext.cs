using Domain.Entities;
using Domain.Entities.Instances;
using Microsoft.EntityFrameworkCore;

namespace Server.Core;

public sealed class ServerDbContext : DbContext
{
    public ServerDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
        SaveChanges();
    }

    // ReSharper disable once UnusedMember.Local
    public DbSet<Build> Builds { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Instance> Instances { get; set; } = null!;
    public DbSet<Token> Tokens { get; set; } = null!;
    public DbSet<DedicatedMachine> DedicatedMachines { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Project>()
            .HasMany(project => project.Builds)
            .WithOne();

        builder.Entity<Project>()
            .HasMany(project => project.Instances)
            .WithOne();

        builder.Entity<Instance>()
            .OwnsMany(instance => instance.StateChanges);

        var instanceConfigEntity =
            builder.Entity<Instance>()
                .OwnsOne(instance => instance.InstanceConfig);

        instanceConfigEntity
            .HasOne<DedicatedMachine>()
            .WithMany()
            .HasForeignKey(config => config.DedicatedMachineId);

        instanceConfigEntity
            .HasOne<Build>()
            .WithMany()
            .HasForeignKey(config => config.BuildId);

        builder.Entity<DedicatedMachine>()
            .HasOne<Token>()
            .WithMany()
            .HasForeignKey(machine => machine.TokenId);
    }
}