using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;

internal class DbAccess
{
    private readonly UsersDbContext? _dbContext;

    public DbAccess()
    {
    }

    public DbAccess(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual Task<int> GetTotalEntries(string? search, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        var query = _dbContext.Users.Select(t => t);

        if (!string.IsNullOrEmpty(search))
            query.Where(t => t.Email.Contains(search) || t.Name.Contains(search) || t.Phone.Contains(search));

        return query.CountAsync(ct);
    }

    public virtual async Task<IEnumerable<User>> GetPaginatedUsers(string? search, int page, int pageSize,
        CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        var query = _dbContext.Users
            .Include(t => t.Roles)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(t => t);

        if (!string.IsNullOrEmpty(search))
            query.Where(t => t.Email.Contains(search) || t.Name.Contains(search) || t.Phone.Contains(search));

        return await query.AsNoTracking().ToListAsync(ct);
    }
}