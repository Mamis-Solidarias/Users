using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Password.PUT;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public async Task UpdatePassword(Request request, CancellationToken token = default)
    {
        await CreateRequest(HttpMethod.Put, "users", request.Id.ToString(), "password")
            .WithContent(new
            {
                request.OldPassword,
                request.NewPassword
            })
            .ExecuteAsync(token);
    }
}