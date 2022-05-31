using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Services.Abstraction;

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
    public string Generate(string description)
    {
        return _tokenService.Generate(description);
    }
    
    [HttpPost]
    public void Edit(Guid id, string description)
    {
        _tokenService.Edit(id, description);
    }

    [HttpPost]
    public void Refuse(Guid id)
    {
        _tokenService.Refuse(id);
    }
}