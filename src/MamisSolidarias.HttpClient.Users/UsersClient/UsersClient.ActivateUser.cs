namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{

    /// <param name="Id">Id of the user to reactivate</param>
    public sealed record ActivateUserRequest(int Id);

    /// <inheritdoc />
    public Task ActivateUser(ActivateUserRequest request, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Post, "users", $"{request.Id}")
            .ExecuteAsync(token);
    }
}