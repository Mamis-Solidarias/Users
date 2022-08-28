using FastEndpoints;
using FastEndpoints.Security;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Services;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly DbAccess _dbAccess;
    private readonly ITextHasher _textHasher;
    private readonly IConfiguration _configuration;

    public Endpoint(UsersDbContext dbContext, ITextHasher textHasher, IConfiguration configuration,
        DbAccess? dbAccess = null)
    {
        _textHasher = textHasher;
        _configuration = configuration;
        _dbAccess = dbAccess ?? new DbAccess(dbContext);
    }

    public override void Configure()
    {
        Post("users/auth");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        req.Email = req.Email.Trim().ToLowerInvariant();
        var user = await _dbAccess.FindUserByEmail(req.Email, ct);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var (_, hashedPassword) = _textHasher.Hash(req.Password, Convert.FromBase64String(user.Salt));
        if (user.Password != hashedPassword)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        ArgumentNullException.ThrowIfNull(_configuration);

        var jwtToken = JWTBearer.CreateToken(
            _configuration["JWT:Key"],
            DateTime.UtcNow.AddDays(int.Parse(_configuration["JWT:ExpiresIn"])),
            claims: new[] {("Email", user.Email), ("Id", user.Id.ToString()), ("Name", user.Name)},
            permissions: GetUserPermissions(user)
        );

        await SendOkAsync(new Response {Jwt = jwtToken}, ct);
    }

    private static IEnumerable<string> GetUserPermissions(User user)
    {
        return user.Roles
            .SelectMany(t => new[]
            {
                (t.Service, Action: "read", CanDoAction: t.CanRead),
                (t.Service, Action: "write", CanDoAction: t.CanWrite)
            })
            .Where(t => t.CanDoAction)
            .Select(t => $"{t.Service}/{t.Action}")
            .ToArray();
    }
}