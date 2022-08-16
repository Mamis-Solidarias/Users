using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.WebAPI.Users.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH;

internal class Endpoint : Endpoint<Request,Response>
{
    private readonly DbAccess _db;

    public Endpoint(UsersDbContext dbContext, DbAccess? db = null)
    {
        _db = db ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Patch("users/{id}");
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

        if (req.Email is not null)
            user.Email = req.Email;
        if (req.Name is not null)
            user.Name = req.Name;
        if (req.Phone is not null)
            user.Phone = req.Phone;
        
        try
        {
            await _db.UpdateUser(user, ct);
        }
        catch (DbUpdateException)
        {
            AddError("Sucedio un error al crear al usuario");
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await SendOkAsync(new Response
        {
            Email = user.Email,
            Name = user.Name,
            Phone = user.Phone
        }, ct);
    }
}