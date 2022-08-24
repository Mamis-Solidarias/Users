using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Utils.Security;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;

internal class Endpoint : Endpoint<Request>
{
    private readonly DbAccess _db;

    public Endpoint(UsersDbContext dbContext, DbAccess? db = null)
    {
        _db = db ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Delete("users/{id}");
        Policies(Utils.Security.Services.Users.WritePermission());
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
        if (request.Id == User.GetUserId()) 
        {
            AddError("Un usuario no puede eliminarse a si mismo!");
            await SendErrorsAsync(cancellation: ct);
            return;
        }
        
        var user = await _db.GetUserById(request.Id, ct);
        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await _db.SoftDeleteUser(user,ct);
        await SendOkAsync(ct);
    }
}