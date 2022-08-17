using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT;

internal class DbAccess
{
    private readonly UsersDbContext? _dbContext;

    public DbAccess(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public DbAccess() { }


    public virtual Task<User?> GetUserById(int id, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.Users.FirstOrDefaultAsync(t => t.IsActive == true && t.Id == id, ct);
    }

    public virtual Task SaveChanges(CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.SaveChangesAsync(ct);
    }
}