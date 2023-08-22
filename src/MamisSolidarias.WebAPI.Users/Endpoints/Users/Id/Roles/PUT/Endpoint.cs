using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT;

internal class Endpoint : Endpoint<Request, Response>
{
    private readonly UsersDbContext _db;
    private readonly IRolesCache _rolesCache;

    public Endpoint(UsersDbContext dbContext, IRolesCache rolesCache)
    {
	    _rolesCache = rolesCache;
	    _db = dbContext;
    }
    
    public override void Configure()
    {
        Put("users/{id}/roles");
        Policies(Utils.Security.Policies.CanWrite);
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
	    try
	    {
		    var user = await _db.Users
		     .AsTracking()
		     .Include(t=>t.Roles)
		     .SingleAsync(t => t.Id == req.Id, ct);

		    var newRoles = req.Roles.Select(t => new Role
		    {
			    CanRead = t.CanRead,
			    CanWrite = t.CanWrite,
			    Service = Enum.Parse<Utils.Security.Services>(t.Service)
		    }).ToList();
		    
		    user.Roles = newRoles;
		    await _db.SaveChangesAsync(ct);
		    
		    await _rolesCache.SetPermissions(req.Id, GetUserPermissions(newRoles));
		    
		    await SendOkAsync(new Response
		    {
			    Roles = newRoles.Select(t => new RoleResponse(t.Service.ToString(), t.CanWrite, t.CanRead))
		    }, ct);
	    }
	    catch(InvalidOperationException)
	    {
		    await SendNotFoundAsync(ct);
	    }
    }
    
    private static IEnumerable<string> GetUserPermissions(IEnumerable<Role> roles)
    {
        var permissions = roles.ToList();
        if (permissions.Any(t=> t.Service == Utils.Security.Services.Campaigns) && permissions.All(t => t.Service != Utils.Security.Services.Donors))
            permissions.Add(new Role { CanRead = true, CanWrite = false, Service = Utils.Security.Services.Donors });

        if (permissions.Any(t => t.Service == Utils.Security.Services.Donations) && permissions.All(t => t.Service != Utils.Security.Services.Donors))
            permissions.Add(new Role { CanRead = true, CanWrite = false, Service = Utils.Security.Services.Donors });
        
        if (permissions.Any(t=> t.Service == Utils.Security.Services.Donors) && permissions.All(t => t.Service != Utils.Security.Services.Users))
            permissions.Add(new Role { CanRead = true, CanWrite = false, Service = Utils.Security.Services.Users });
        
	    return permissions
		    .SelectMany(t => new[]
		    {
			    (t.Service, Action: "read", CanDoAction: t.CanRead),
			    (t.Service, Action: "write", CanDoAction: t.CanWrite)
		    })
		    .Where(t => t.CanDoAction)
		    .Select(t => $"{t.Service}/{t.Action}");
    }
}