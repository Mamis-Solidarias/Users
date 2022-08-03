using MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public async Task<Response?> CreateUser(Request parameters, CancellationToken token = default)
    {
        return await CreateRequest<Response>(HttpMethod.Post,"users")
            .WithContent(parameters)
            .ExecuteAsync(token);
    }
}