using FastEndpoints;
using FastEndpoints.Security;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly ITextHasher _textHasher;
    private readonly IRolesCache _rolesCache;
    private readonly UsersDbContext _dbContext;

    public Endpoint(UsersDbContext dbContext, ITextHasher textHasher, IRolesCache rolesCache)
    {
        _textHasher = textHasher;
        _rolesCache = rolesCache;
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("users/auth");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
	    try
	    {
		    req.Email = req.Email.Trim().ToLowerInvariant();

		    var user = await _dbContext.Users
			    .Include(t => t.Roles)
			    .Where(t => t.IsActive == true)
			    .SingleAsync(t => t.Email == req.Email, ct);


		    var (_, hashedPassword) = _textHasher.Hash(req.Password, Convert.FromBase64String(user.Salt));
		    if (user.Password != hashedPassword)
		    {
			    await SendUnauthorizedAsync(ct);
			    return;
		    }

		    var jwtToken = JWTBearer.CreateToken(
			    "This token is only used to create the cookie later",
			    claims: new[] { ("Email", user.Email), ("Id", user.Id.ToString()), ("Name", user.Name) }
		    );

		    await _rolesCache.SetPermissions(user.Id, GetUserPermissions(user));

		    await SendOkAsync(new Response { Jwt = jwtToken }, ct);
	    }
	    catch (InvalidOperationException)
	    {
		    await SendUnauthorizedAsync(ct);
	    }
    }
    
    

    private static IEnumerable<string> GetUserPermissions(User user)
    {
        if (user.Roles.Any(t=> t.Service == Utils.Security.Services.Campaigns) && user.Roles.All(t => t.Service != Utils.Security.Services.Donors))
            user.Roles.Add(new Role { CanRead = true, CanWrite = false, Service = Utils.Security.Services.Donors });

        if (user.Roles.Any(t => t.Service == Utils.Security.Services.Donations) && user.Roles.All(t => t.Service != Utils.Security.Services.Donors))
            user.Roles.Add(new Role { CanRead = true, CanWrite = false, Service = Utils.Security.Services.Donors });
        
        if (user.Roles.Any(t=> t.Service == Utils.Security.Services.Donors) && user.Roles.All(t => t.Service != Utils.Security.Services.Users))
            user.Roles.Add(new Role { CanRead = true, CanWrite = false, Service = Utils.Security.Services.Users });
        
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