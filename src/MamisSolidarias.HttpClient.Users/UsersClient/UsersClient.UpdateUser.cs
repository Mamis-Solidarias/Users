using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> UpdateUser(Request request, CancellationToken token = default)
    {
        return CreateRequest<Response>(HttpMethod.Patch, "users", $"{request.Id}")
            .WithContent(new
            {
                request.Email,
                request.Name,
                request.Phone
            })
            .ExecuteAsync(token);
    }
}