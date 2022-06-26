using Domain.Entities;
using Domain.Entities.Instances;
using Microsoft.EntityFrameworkCore;

namespace Server.Core.Test;

public class DbJokes
{
    public static void Test()
    {
        using (var db = TestDb())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Projects.Add(new Project("name", "Repository name", "build"));
            db.SaveChanges();
        }

        using (var db = TestDb())
        {
            var project = db.Projects.First();
            project.Builds.Add(new Build("BuildNameIsCool", Guid.NewGuid()));

            db.Update(project);
            db.SaveChanges();
        }

        using (var db = TestDb())
        {
            var tokenId = Guid.NewGuid();
            db.Tokens.Add(new Token(tokenId, "asd", "description", DateTime.Now));

            db.DedicatedMachines.Add(new DedicatedMachine(tokenId, "label", "description"));

            db.SaveChanges();
        }

        using (var db = TestDb())
        {
            var project = db.Projects.Include(p => p.Builds).First();
            var build = project.Builds.First();
            var dedicatedMachine = db.DedicatedMachines.First();
            project.Instances.Add(new Instance(new InstanceConfig(build.Id, dedicatedMachine.Id, "echo 123")));

            db.Update(project);
            db.SaveChanges();
        }
    }

    private static ServerDbContext TestDb()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ServerDbContext>();
        optionsBuilder.UseSqlite(@"Filename=../../../Test.db");
        return new ServerDbContext(optionsBuilder.Options);
    }
}