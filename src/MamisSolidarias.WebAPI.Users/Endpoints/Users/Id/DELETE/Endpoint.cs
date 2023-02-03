using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Utils.Security;
using MamisSolidarias.WebAPI.Users.Services;
using Microsoft.EntityFrameworkCore;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;

internal class Endpoint : Endpoint<Request>
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
		Delete("users/{id}");
		Policies(Utils.Security.Policies.CanWrite);
	}

	public override async Task HandleAsync(Request request, CancellationToken ct)
	{
		if (request.Id == User.GetUserId())
		{
			AddError("Un usuario no puede eliminarse a si mismo!");
			await SendErrorsAsync(cancellation: ct);
			return;
		}

		var usersAffected = await _db.Users
			.Where(t => t.Id == request.Id)
			.ExecuteUpdateAsync(
				t => t.SetProperty(r => r.IsActive, false),
				ct
			);

		if (usersAffected is 0)
		{
			await SendNotFoundAsync(ct);
			return;
		}
		
		await _rolesCache.ClearPermissions(request.Id);
		await SendOkAsync(ct);
	}
}