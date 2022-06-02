using Domain.Services;

namespace Server.Core.Services;

public class MasterService : IMasterService
{
    private readonly ServerDBContext _context;

    public MasterService(ServerDBContext context)
    {
        _context = context;
    }

    public void Reset()
    {
        _context.Database.EnsureCreated();
        _context.Database.EnsureDeleted();
        _context.SaveChanges();
    }
}