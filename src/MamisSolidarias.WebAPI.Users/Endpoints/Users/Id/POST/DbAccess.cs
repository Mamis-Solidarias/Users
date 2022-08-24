using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.POST;

internal class DbAccess
{
    private readonly UsersDbContext? _dbContext;
    public DbAccess() { }

    public DbAccess(UsersDbContext? dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual Task<User?> GetUserById(int id, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.Users.FirstOrDefaultAsync(t => t.Id == id,token);
    }

    public virtual Task SaveChanges(CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.SaveChangesAsync(token);
    }
}