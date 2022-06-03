using Domain.Services;

namespace Server.Core.Services;

public class MasterService : IMasterService
{
    private readonly ServerDbContext _context;

    public MasterService(ServerDbContext context)
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