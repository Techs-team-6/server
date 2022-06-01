using Domain.Entities;
using Domain.Services;
using Server.Core.Tools;

namespace Server.Core.Services;

public class DedicatedMachineService : IDedicatedMachineService
{
    private readonly ServerDBContext _context;
    private readonly ITokenService _tokenService;

    public DedicatedMachineService(ServerDBContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public DedicatedMachine RegisterMachine(string tokenStr, string label, string description)
    {
        var token = _tokenService.FindByTokenString(tokenStr) ?? throw new ServiceException("Invalid token");
        var dedicatedServer = new DedicatedMachine()
        {
            TokenId = token.Id,
            Label = label,
            Description = description,
            State = DedicatedMachine.DedicatedMachineState.Offline,
        };

        _context.DedicatedServers.Add(dedicatedServer);
        _context.SaveChanges();
        return dedicatedServer;
    }

    public bool AuthMachine(Guid id, string tokenStr)
    {
        var machine = _context.DedicatedServers.FirstOrDefault(m => m.Id == id) ??
                      throw new ServiceException("There is no machine with such id");
        if (!_tokenService.Check(tokenStr))
            throw new ServiceException("Wrong token");
        return true;
    }

    public void SetState(Guid serverId, DedicatedMachine.DedicatedMachineState state)
    {
        var server = _context.DedicatedServers.FirstOrDefault(s => s.Id == serverId)
                     ?? throw new ServiceException($"There is no dedicated server with such id '{serverId}'");
        server.State = state;
        _context.SaveChanges();
    }
}