using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;

internal class DbAccess
{
    private readonly UsersDbContext? _dbContext;

    public DbAccess(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DbAccess() { }

    public virtual Task<User?> GetUserById(int id, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.Users.FirstOrDefaultAsync(t => t.IsActive == true && t.Id == id,token);
    }

    public virtual async Task SoftDeleteUser(User user, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        user.IsActive = false;
        await _dbContext.SaveChangesAsync(token);
    }
}