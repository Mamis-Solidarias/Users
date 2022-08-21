using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.POST;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task ActivateUser(Request request, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Post, "users", $"{request.Id}")
            .ExecuteAsync(token);
    }
}