using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> UpdateRoles(Request request, CancellationToken token = default)
    {
        return CreateRequest<Response>(HttpMethod.Put, "users", $"{request.Id}", "roles")
            .WithContent(new
            {
                request.Roles
            })
            .ExecuteAsync(token);
    }
}