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
        try
        {
            return _tokenService.Generate(description);
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public void Edit(Guid id, string description)
    {
        try
        {
            _tokenService.Edit(id, description);
        }
        catch (ServiceException e)
        {
            NotFound(e.Message);
        }
    }

    [HttpPost]
    public ActionResult Refuse(Guid id)
    {
        try
        {
            _tokenService.Refuse(id);
            return Ok();
        }
        catch (ServiceException e)
        {
            return BadRequest(e.Message);
        }
    }
}