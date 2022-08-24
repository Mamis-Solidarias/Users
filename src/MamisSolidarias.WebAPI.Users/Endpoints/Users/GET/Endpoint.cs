using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.Utils.Security;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly DbAccess _db;

    public Endpoint(UsersDbContext dbContext, DbAccess? db = null)
    {
        _db = db ?? new DbAccess(dbContext);
    }


    public override void Configure()
    {
        Get("users");
        Policies(Utils.Security.Services.Users.ReadPermission());
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        var users = await _db.GetPaginatedUsers(request.Search, request.Page, request.PageSize, ct);
        var totalEntries = await _db.GetTotalEntries(request.Search, ct);

        await SendOkAsync(new Response
        {
            Entries = users.Select(MapUser),
            Page = request.Page,
            TotalPages = totalEntries / request.PageSize + (totalEntries % request.PageSize is 0 ? 0 : 1)
        }, ct);
    }

    private static UserResponse MapUser(User user)
    {
        var roles = user.Roles.Select(MapRole);
        return new UserResponse(user.Id, user.Name, user.Email, user.Phone,user.IsActive ?? true, roles);
    }

    private static RoleResponse MapRole(Role role)
    {
        return new(role.Service.ToString(), role.CanWrite, role.CanRead);
    }
}