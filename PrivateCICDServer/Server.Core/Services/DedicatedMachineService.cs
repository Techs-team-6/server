using Domain.Dto.DedicatedMachineDto;
using Domain.Entities;
using Domain.Services;
using Domain.Tools;

namespace Server.Core.Services;

public class DedicatedMachineService : IDedicatedMachineService
{
    private readonly ServerDbContext _context;
    private readonly ITokenService _tokenService;

    public DedicatedMachineService(ServerDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public DedicatedMachine RegisterMachine(RegisterDto dto)
    {
        var token = _tokenService.FindByTokenString(dto.TokenString) ??
                    throw new InvalidTokenException("Invalid token");
        var dedicatedServer = new DedicatedMachine
        {
            TokenId = token.Id,
            Label = dto.Label,
            Description = dto.Description,
            State = DedicatedMachineState.Offline,
        };

        _context.DedicatedMachines.Add(dedicatedServer);
        _context.SaveChanges();
        return dedicatedServer;
    }

    public void AuthMachine(AuthDto dto)
    {
        var machine = _context.DedicatedMachines.FirstOrDefault(m => m.Id == dto.Id) ??
                      throw new AuthException("There is no machine with such id");
        var token = _tokenService.FindByTokenString(dto.TokenString) ??
                    throw new AuthException(null, new InvalidTokenException("Wrong token"));
        if (token.Id != machine.TokenId)
            throw new AuthException("Wrong credentials");
    }

    public void SetState(SetStateDto dto)
    {
        var server = _context.DedicatedMachines.FirstOrDefault(s => s.Id == dto.ServerId)
                     ?? throw new ServiceException($"There is no dedicated server with such id '{dto.ServerId}'");
        server.State = dto.State;
        _context.SaveChanges();
    }

    public List<DedicatedMachine> List()
    {
        return _context.DedicatedMachines.ToList();
    }
}