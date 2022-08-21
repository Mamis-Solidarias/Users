using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.GET;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> GetUser(Request request, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Get, "users", request.Id.ToString())
            .ExecuteAsync<Response>(token);
    }
}