
namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task<SignInResponse?> SignIn(SignInRequest request, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Post, "users", "auth")
            .WithContent(request)
            .ExecuteAsync<SignInResponse>(token);
    }
    
    /// <param name="Email">Email of the user</param>
    /// <param name="Password">Password of the user</param>
    public sealed record SignInRequest(string Email, string Password); 
    
    /// <param name="Jwt">Generated token</param>
    public sealed record SignInResponse(string Jwt);
}