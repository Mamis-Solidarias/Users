using MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> SignIn(Request request, CancellationToken token = default)
    {
        return CreateRequest<Response>(HttpMethod.Post, "users", "auth")
            .WithContent(request)
            .ExecuteAsync(token);
    }
}