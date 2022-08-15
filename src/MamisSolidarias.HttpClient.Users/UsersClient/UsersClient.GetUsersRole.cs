using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.GET;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> GetUsersRoles(Request request, CancellationToken token = default)
    {
        return CreateRequest<Response>(HttpMethod.Get, "users", $"{request.Id}", "roles")
            .ExecuteAsync(token);
    }
}