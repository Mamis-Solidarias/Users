using MamisSolidarias.HttpClient.Users.Models;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task<GetUserResponse?> GetUser(int id, CancellationToken token = default) 
        => CreateRequest(HttpMethod.Get, "users", id.ToString())
            .ExecuteAsync<GetUserResponse>(token);


    /// <param name="User">Expected user</param>
    public sealed record GetUserResponse(User User);

    
}