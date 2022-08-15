using FastEndpoints;
using FastEndpoints.Security;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.GET;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly DbAccess _db;
    public override void Configure()
    {
        Get("users/{id}");
    }

    public Endpoint(UsersDbContext? dbContext, DbAccess? dbAccess = null)
    {
        _db = dbAccess ?? new DbAccess(dbContext);
    }


    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var isUserAdmin = User.HasPermission("Users/read");
        var isUserOwnerOfAccount = User.Claims.Any(t=> t.Type is "Id" && int.Parse(t.Value) == req.Id);

        if (!isUserAdmin && !isUserOwnerOfAccount)
        {
            await SendForbiddenAsync(ct);
            return;
        }

        var user = await _db.GetUserById(req.Id, ct);

        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(new Response {User = MapUser(user)}, ct);
    }
    
    private static UserResponse MapUser(User user)
    {
        var roles = user.Roles.Select(MapRole);
        return new UserResponse(user.Id, user.Name, user.Email, user.Phone,roles);
    }

    private static RoleResponse MapRole(Role role) => new(role.Service.ToString(), role.CanWrite, role.CanRead);
}