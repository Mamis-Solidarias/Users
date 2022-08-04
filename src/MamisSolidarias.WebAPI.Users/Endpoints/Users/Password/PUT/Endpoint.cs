using FastEndpoints;
using FastEndpoints.Security;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.WebAPI.Users.Services;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Password.PUT;

// ReSharper disable once ClassNeverInstantiated.Global
internal class Endpoint : Endpoint<Request>
{
    private readonly ITextHasher _textHasher;
    private readonly DbAccess _db;

    public Endpoint(ITextHasher textHasher,UsersDbContext dbContext, DbAccess? dbAccess = null)
    {
        _textHasher = textHasher;
        _db = dbAccess ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Put("users/{id}/password");
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var isUserAdmin = User.HasPermission("Users/write");
        var isUserOwnerOfAccount = User.Claims.Any(t=> t.Type is "Id" && int.Parse(t.Value) == req.Id);

        if (!isUserAdmin && !isUserOwnerOfAccount)
        {
            await SendForbiddenAsync(ct);
            return;
        }

        var user = await _db.FindUserById(req.Id,ct);

        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var (_, oldPasswordHash) = _textHasher.Hash(req.OldPassword, Convert.FromBase64String(user.Salt));

        if (user.Password != oldPasswordHash)
        {
            AddError("La contraseña antigua no es correcta");
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var (_, newPasswordHash) = _textHasher.Hash(req.NewPassword, Convert.FromBase64String(user.Salt));

        await _db.UpdatePassword(user, newPasswordHash, ct);
        
        await SendOkAsync(ct);
    }
}