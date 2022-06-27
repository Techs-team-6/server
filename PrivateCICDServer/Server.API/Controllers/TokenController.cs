using Domain.Entities;
using Domain.Services;
using Domain.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public List<Token> List()
    {
        return _tokenService.List();
    }

    [HttpPost]
    public ActionResult<string> Generate(string description)
    {
        return _tokenService.Generate(description);
    }
    
    [HttpPost]
    public void Edit(Guid id, string description)
    {
        _tokenService.Edit(id, description);
    }

    [HttpPost]
    public ActionResult Refuse(Guid id)
    {
        _tokenService.Refuse(id);
        return Ok();
    }
}