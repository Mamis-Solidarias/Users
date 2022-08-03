using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

internal class DbService
{
    private readonly UsersDbContext? _dbContext;

    public DbService() { }
    public DbService(UsersDbContext dbContext)
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