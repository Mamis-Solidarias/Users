namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task DeleteUser(int id, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Delete, "users", $"{id}")
            .ExecuteAsync(token);
    }
}