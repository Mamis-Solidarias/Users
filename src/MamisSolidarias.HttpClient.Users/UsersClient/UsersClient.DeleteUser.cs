using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task DeleteUser(Request request, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Delete, "users", $"{request.Id}")
            .ExecuteAsync(token);
    }
}