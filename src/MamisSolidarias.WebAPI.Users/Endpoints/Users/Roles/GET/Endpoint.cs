using FastEndpoints;
namespace MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;

internal class Endpoint : EndpointWithoutRequest<Response>
{
    public override void Configure()
    {
        Get("users/roles");
        Policies( Utils.Security.Policies.CanRead);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var roles = Enum.GetValues<Utils.Security.Services>()
            .Select(t => new RoleResponse(t.ToString(), true, true));
        
        await SendOkAsync(new Response { Roles = roles }, ct);
    }
}