using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.Utils.Security;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.GET;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly DbAccess _db;

    public Endpoint(UsersDbContext dbContext, DbAccess? db = null)
    {
        _db = db ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Get("users/{id}/roles");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (!User.HasPermissionOrIsAccountOwner(Utils.Security.Services.Users.ReadPermission(), req.Id))
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

        await SendOkAsync(new Response {Roles = MapRoles(user.Roles)}, ct);
    }

    private static IEnumerable<RoleResponse> MapRoles(IEnumerable<Role> roles)
        => roles.Select(t => new RoleResponse(t.Service.ToString(), t.CanWrite, t.CanRead));
}