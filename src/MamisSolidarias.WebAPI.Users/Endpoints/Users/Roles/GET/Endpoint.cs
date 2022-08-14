using FastEndpoints;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;

internal class Endpoint : EndpointWithoutRequest<Response>
{
    public override void Configure()
    {
        Get("users/roles");
        Policies(Services.Policies.Names.CanRead.ToString());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var roles = Enum.GetValues<Infrastructure.Users.Models.Services>()
            .Select(t => new RoleResponse(t.ToString(), true, true));
        
        await SendOkAsync(new Response { Roles = roles }, ct);
    }
}