using HotChocolate.AspNetCore.Authorization;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;

namespace MamisSolidarias.WebAPI.Users.Queries;

public class UsersQuery
{

    [Authorize(Policy = "CanRead")]
    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<User> GetUser(UsersDbContext dbContext, int? id)
        => dbContext.Users.Where(t => t.Id == id);
}