using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Entities.Instances;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProjectServiceApi;
using Server.Core.Services;

namespace Server.Core.Test;

public class InstanceServiceTest
{
    private IInstanceService _service;
    private const int Count = 10;
    private List<string> _names;
    private List<string> _scripts;
    private Project _testProject;
    private Build _testBuild;
    private DedicatedMachine _testMachine;

    [SetUp]
    public void SetUp()
    {
        Environment.SetEnvironmentVariable("WITHOUT_PS", "true");
        var options = new DbContextOptionsBuilder<ServerDbContext>()
            .UseInMemoryDatabase("Test").Options;
        var context = new ServerDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        _names = Enumerable.Range(0, Count)
            .Select(i => $"name{i}")
            .ToList();
        _scripts = Enumerable.Range(0, Count)
            .Select(i => $"script{i}")
            .ToList();

        var nameValidator = new NameValidatorService();
        var projectService =
            new ProjectService(context, new ProjectServiceApiClient("test", new HttpClient()), nameValidator);
        var tokenService = new TokenService(context);
        var dmService = new DedicatedMachineService(context, tokenService);
        _service = new InstanceService(context, projectService, dmService, nameValidator, null);
        _testProject = projectService.CreateProject(_names[0], _scripts[0]);
        _testBuild = projectService.AddBuild(_testProject.Id, _names[0], Guid.NewGuid());
        _testMachine =
            dmService.RegisterMachine(new RegisterDto(tokenService.Generate("description"), "label", "description"));
    }

    [Test]
    public void RegisterInstanceTest()
    {
        _service.CreateInstance(_testProject.Id, "Name",
            new InstanceConfig(_testBuild.Id, _testMachine.Id, "start"));

        Assert.That(_service.ListAllStates(_testProject.Instances.First().Id).Count, Is.EqualTo(1));
    }

    [Test]
    public void ChangeInstanceStateTest()
    {
        var instance = _service.CreateInstance(_testProject.Id, "Name",
            new InstanceConfig(_testBuild.Id, _testMachine.Id, "start"));

        Assert.That(_service.ListAllStates(instance.Id).Last(), Is.EqualTo(InstanceState.NotPublished));
        _service.ChangeInstanceState(instance.Id, InstanceState.Running);
        Assert.That(_service.ListAllStates(instance.Id).Last(), Is.EqualTo(InstanceState.Running));
    }
}