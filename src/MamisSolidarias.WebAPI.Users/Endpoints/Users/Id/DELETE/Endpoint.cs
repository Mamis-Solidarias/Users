using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;

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
        Policies(Services.Policies.Names.CanWrite.ToString());
    }

    public override async Task HandleAsync(Request request, CancellationToken ct)
    {
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