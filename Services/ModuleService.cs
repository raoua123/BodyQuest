
using BodyQuest.Data;
using BodyQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace BodyQuest.Services;

public class ModuleService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public ModuleService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<List<BodyModule>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Modules
            .AsNoTracking()
            .Include(m => m.Lessons.OrderBy(l => l.SortOrder))
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    public async Task<BodyModule?> GetBySlugAsync(string slug)
    {
        await using var db = await _factory.CreateDbContextAsync();

        return await db.Modules
            .AsNoTracking()
            .Include(m => m.Lessons.OrderBy(l => l.SortOrder))
            .Include(m => m.Organs)
            .FirstOrDefaultAsync(m => m.Slug == slug);
    }

}
