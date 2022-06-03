using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Server.Core.Test;

public class DbJokes
{
    public static void Test()
    {
        using (var db = new TestDb())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Projects.Add(new Project
            {
                Name = "Some name",
                Repository = "Repository name",
                BuildScript = "",
            });
            db.SaveChanges();
        }

        using (var db = new TestDb())
        {
            var project = db.Projects.First();
            project.Builds.Add(new Build
            {
                Name = "BuildNameIsCool",
                StorageId = Guid.NewGuid()
            });

            db.Update(project);
            db.SaveChanges();
        }

        using (var db = new TestDb())
        {
            var tokenId = Guid.NewGuid();
            db.Tokens.Add(new Token
            {
                Id = tokenId,
                Description = "descr token",
                CreationTime = DateTime.Now,
                TokenStr = "asd"
            });

            db.DedicatedMachines.Add(new DedicatedMachine
            {
                Label = "label",
                Description = "description",
                TokenId = tokenId,
            });

            db.SaveChanges();
        }

        using (var db = new TestDb())
        {
            var project = db.Projects.Include(p => p.Builds).First();
            var build = project.Builds.First();
            var dedicatedMachine = db.DedicatedMachines.First();
            project.Instances.Add(new Instance
                {
                    InstanceConfig = new InstanceConfig
                    {
                        Build = build,
                        DedicatedMachine = dedicatedMachine,
                        StartString = "echo 123",
                    }
                }
            );

            db.Update(project);
            db.SaveChanges();
        }
    }

    private class TestDb : ServerDbContext
    {
        public TestDb() : base(new DbContextOptionsBuilder<ServerDbContext>().UseSqlite(@"Filename=../../../Test.db")
            .Options)
        {
        }
    }
}