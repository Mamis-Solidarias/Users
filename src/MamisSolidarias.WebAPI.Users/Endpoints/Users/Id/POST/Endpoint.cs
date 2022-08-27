using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Utils.Security;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.POST;

internal class Endpoint : Endpoint<Request>
{
    private readonly DbAccess _db;

    public Endpoint(UsersDbContext? dbContext,DbAccess? db = null)
    {
        _db = db ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Post("users/{id}");
        Policies(Utils.Security.Policies.CanWrite);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await _db.GetUserById(req.Id, ct);

        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (user.IsActive ?? true)
        {
            AddError("El usuario ya esta activo");
            await SendErrorsAsync(cancellation:ct);
            return;
        }

        user.IsActive = true;
        await _db.SaveChanges(ct);
        await SendOkAsync(ct);
    }
}