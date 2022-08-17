using FastEndpoints;
using FastEndpoints.Security;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly DbAccess _db;

    public Endpoint(UsersDbContext dbContext, DbAccess? db = null)
    {
        _db = db ?? new DbAccess(dbContext);
    }


    public override void Configure()
    {
        Put("users/{id}/roles");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        if (!User.HasPermissionOrIsAccountOwner("Users/write", req.Id))
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

        user.Roles = req.Roles.Select(t => new Role
        {
            CanRead = t.CanRead,
            CanWrite = t.CanWrite,
            Service = Enum.Parse<Infrastructure.Users.Models.Services>(t.Service)
        }).ToList();

        await _db.SaveChanges(ct);
        
        await SendOkAsync(new Response
        {
            Roles = user.Roles.Select(t=> new RoleResponse(t.Service.ToString(),t.CanWrite,t.CanRead))
        }, ct);

    }
}