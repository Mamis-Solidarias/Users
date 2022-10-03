
namespace MamisSolidarias.HttpClient.Users.UsersClient;

/// <inheritdoc />
public partial class UsersClient
{
    /// <inheritdoc />
    public async Task<CreateUserResponse?> CreateUser(CreateUserRequest parameters, CancellationToken token = default)
    {
        return await CreateRequest(HttpMethod.Post, "users")
            .WithContent(parameters)
            .ExecuteAsync<CreateUserResponse>(token);
    }

    
    /// <param name="Name">The name of the new user</param>
    /// <param name="Email">The email of the new user</param>
    /// <param name="Password">The password of the user</param>
    /// <param name="Phone">The phone of the user</param>
    public sealed record CreateUserRequest(
        string Name,
        string Email,
        string Password,
        string Phone
    );
    
    /// <param name="Name">The name of the new user</param>
    /// <param name="Email">The email of the new user</param>
    /// <param name="Id">Id of the user</param>
    /// <param name="Phone">The phone of the user</param>
    public sealed record CreateUserResponse(
        string Id,
        string Name,
        string Email,
        string Phone
    );
}