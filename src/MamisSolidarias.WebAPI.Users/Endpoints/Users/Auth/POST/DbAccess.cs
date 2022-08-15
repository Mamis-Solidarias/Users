using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;

internal class DbAccess
{
    private readonly UsersDbContext? _dbContext;

    public DbAccess()
    {
    }

    public DbAccess(UsersDbContext? dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual async Task<User?> FindUserByEmail(string email, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return await _dbContext.Users
            .Include(t => t.Roles)
            .FirstOrDefaultAsync(t => t.Email == email, ct);
    }
}