using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Services.Interfaces;

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

    [HttpGet(Name = "TokenList")]
    public List<Token> List()
    {
        return _tokenService.List();
    }

    [HttpPost(Name = "TokenGenerate")]
    public string Generate(string description)
    {
        return _tokenService.Generate(description);
    }

    [HttpPost(Name = "TokenRefuse")]
    public void Refuse(Guid id)
    {
        _tokenService.Refuse(id);
    }
}