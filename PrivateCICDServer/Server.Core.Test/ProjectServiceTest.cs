using Domain.Entities;
using Domain.Services;
using Domain.Tools;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProjectServiceApi;
using Server.Core.Services;

namespace Server.Core.Test;

public class ProjectServiceTest
{
    private IProjectService _service;
    private const int Count = 10;
    private List<string> _names;
    private List<string> _scripts;

    [SetUp]
    public void SetUp()
    {
        Environment.SetEnvironmentVariable("WITHOUT_PS", "true");
        var options = new DbContextOptionsBuilder<ServerDbContext>()
            .UseInMemoryDatabase("Test").Options;
        var context = new ServerDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        var nameValidator = new NameValidatorService();
        _service = new ProjectService(context, new ProjectServiceApiClient("test", new HttpClient()), nameValidator);
        _names = Enumerable.Range(0, Count)
            .Select(i => $"name{i}")
            .ToList();
        _scripts = Enumerable.Range(0, Count)
            .Select(i => $"script{i}")
            .ToList();
    }

    [Test]
    public void CreateProjectTest()
    {
        Project created = null;
        Assert.DoesNotThrow(() => { created = _service.CreateProject(_names[0], _scripts[0]); });

        Assert.That(_service.GetProject(_names[0]), Is.EqualTo(created));

        Assert.Throws<ServiceException>(() => { _service.CreateProject(_names[0], _scripts[0]); });
    }

    [Test]
    public void DeleteGetProjectTest()
    {
        for (var i = 0; i < Count; i++)
        {
            _service.CreateProject(_names[i], _scripts[i]);
        }

        Assert.That(_service.GetProjects().Count, Is.EqualTo(Count));

        for (var i = 0; i < Count; i++)
        {
            _service.DeleteProject(_service.GetProject(_names[i]).Id);
        }

        Assert.That(_service.GetProjects().Count, Is.EqualTo(0));
    }

    [Test]
    public void AddGetBuildsTest()
    {
        var project = _service.CreateProject(_names[0], _scripts[0]);
        var name1 = "name1";
        var name2 = "name2";
        var storage1 = Guid.NewGuid();
        var storage2 = Guid.NewGuid();

        _service.AddBuild(project.Id, name1, storage1);
        Assert.That(_service.GetProject(_names[0]).Builds, Has.Count.EqualTo(1));
        _service.AddBuild(project.Id, name2, storage2);
        Assert.That(_service.GetProject(_names[0]).Builds, Has.Count.EqualTo(2));
    }
}