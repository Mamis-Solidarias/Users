namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public async Task UpdatePassword(int id,UpdatePasswordRequest request, CancellationToken token = default)
    {
        await CreateRequest(HttpMethod.Put, "users", id.ToString(), "password")
            .WithContent(new
            {
                request.OldPassword,
                request.NewPassword
            })
            .ExecuteAsync(token);
    }

    /// <param name="OldPassword">The current password of the user</param>
    /// <param name="NewPassword">The new password of the user</param>
    public sealed record UpdatePasswordRequest(string OldPassword, string NewPassword);
}