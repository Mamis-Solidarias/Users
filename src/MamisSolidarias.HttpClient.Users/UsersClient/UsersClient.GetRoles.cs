using MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> GetRoles(CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Get, "users", "roles")
            .ExecuteAsync<Response>(token);
    }
}