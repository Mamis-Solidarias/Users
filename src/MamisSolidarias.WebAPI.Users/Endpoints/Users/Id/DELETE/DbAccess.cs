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

    public Task<User?> GetUserById(int id, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.Users.FirstOrDefaultAsync(t => t.IsActive && t.Id == id,token);
    }

    public async Task SoftDeleteUser(User user, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(token);
    }
}