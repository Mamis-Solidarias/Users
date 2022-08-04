using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

internal class DbAccess
{
    private readonly UsersDbContext? _dbContext;

    public DbAccess() { }
    public DbAccess(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public virtual async Task AddUser(User user, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        await _dbContext.Users.AddAsync(user, ct);
        await _dbContext.SaveChangesAsync(ct);
    }
}