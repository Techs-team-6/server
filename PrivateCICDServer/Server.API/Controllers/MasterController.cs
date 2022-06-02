using Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class MasterController : ControllerBase
{
    private readonly IMasterService _service;

    public MasterController(IMasterService service)
    {
        _service = service;
    }

    [HttpGet]
    public void Reset()
    {
        _service.Reset();
    }
}