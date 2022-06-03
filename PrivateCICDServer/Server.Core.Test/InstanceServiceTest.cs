using System.Runtime.Serialization;
using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Domain.States;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProjectServiceApiClient;
using Server.Core.Services;

namespace Server.Core.Test;

public class InsatnceServiceTest
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
        _service = new InstanceService(context);
        _names = Enumerable.Range(0, Count)
            .Select(i => $"name{i}")
            .ToList();
        _scripts = Enumerable.Range(0, Count)
            .Select(i => $"script{i}")
            .ToList();
        
        var projectService =  new ProjectService(context, new ProjectServiceClient("test", new HttpClient()));
        var tokenSevice = new TokenService(context);
        var dmService = new DedicatedMachineService(context, tokenSevice);
        _testProject = projectService.CreateProject(_names[0], _scripts[0]);
        _testBuild = projectService.AddBuild(_testProject.Id, _names[0], Guid.NewGuid());
        _testMachine =
            dmService.RegisterMachine(new RegisterDto(tokenSevice.Generate("description"), "label", "description"));
    }

    [Test]
    public void RegisterInstanceTest()
    {
        _service.RegisterInstance(_testProject.Id, InstanceState.NotPublished, "start",
            _testBuild.Id, _testMachine.Id);
        
        Assert.That(_service.ListAllStates(_testProject.Id, 
            _testProject.Instances.FirstOrDefault().Id).Count, Is.EqualTo(1));
    }
    
    [Test]
    public void ChangeInstanceStateTest()
    {
        var instance = _service.RegisterInstance(_testProject.Id,
            InstanceState.NotPublished,
            "start",
            _testBuild.Id, _testMachine.Id);
        
        Assert.That(_service.ListAllStates(_testProject.Id, instance.Id).Last(), Is.EqualTo(InstanceState.NotPublished));
        _service.ChangeInstanceState(_testProject.Id, instance.Id, InstanceState.Running);
        Assert.That(_service.ListAllStates(_testProject.Id, instance.Id).Last(), Is.EqualTo(InstanceState.Running));
    }
}