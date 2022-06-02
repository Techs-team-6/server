using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class DmController : ControllerBase
{
    private readonly IDedicatedMachineService _service;

    public DmController(IDedicatedMachineService service)
    {
        _service = service;
    }

    [HttpGet]
    public IReadOnlyCollection<DedicatedMachine> List()
    {
        return _service.List();
    }
}