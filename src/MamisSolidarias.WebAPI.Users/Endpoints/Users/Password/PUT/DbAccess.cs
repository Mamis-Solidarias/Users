using System.Diagnostics;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Password.PUT;

internal class DbAccess
{
    private readonly UsersDbContext? _dbContext;
    public DbAccess(){}
    public DbAccess(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual Task<User?> FindUserById(int id,CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        return _dbContext.Users.FirstOrDefaultAsync(t => t.Id == id,ct);
    }

    public virtual async Task UpdatePassword(User user, string newPassword, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(_dbContext);
        user.Password = newPassword;
        await _dbContext.SaveChangesAsync(ct);
    }
}