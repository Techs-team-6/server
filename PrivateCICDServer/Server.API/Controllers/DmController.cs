﻿using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Tools;

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
    
    [HttpPost]
    public ActionResult<DedicatedMachine> RegisterMachine(string token, string label, string description)
    {
        try
        {
            var dto = new RegisterDto(token, label, description);
            return _service.RegisterMachine(dto);
        }
        catch (ServiceException e)
        {
            return BadRequest(e.Message);
        }
    }
}