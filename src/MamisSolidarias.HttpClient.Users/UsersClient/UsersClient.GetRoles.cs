using MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> GetRoles(CancellationToken token = default)
    {
        return CreateRequest<Response>(HttpMethod.Get, "users", "roles")
            .ExecuteAsync(token);
    }
}