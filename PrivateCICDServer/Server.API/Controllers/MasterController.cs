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
    
    [HttpGet]
    public string TestStringGet()
    {
        return "Get";
    }
    
    [HttpGet]
    public JsonResult TestStringUrl()
    {
        return new JsonResult("Getasdas://asdasdadadasd.git");
    }
    
    [HttpPost]
    public string TestStringPost()
    {
        return "Post";
    }
}