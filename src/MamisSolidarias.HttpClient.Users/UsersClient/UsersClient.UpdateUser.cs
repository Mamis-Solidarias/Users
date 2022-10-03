namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task<UpdateUserResponse?> UpdateUser(int id, UpdateUserRequest request, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Patch, "users", $"{id}")
            .WithContent(new
            {
                request.Email,
                request.Name,
                request.Phone
            })
            .ExecuteAsync<UpdateUserResponse>(token);
    }
    

    /// <param name="Email">Optional: new email</param>
    /// <param name="Name">Optional: new Name</param>
    /// <param name="Phone">Optional: new Phone</param>
    public sealed record UpdateUserRequest(string? Email, string? Name, string? Phone);
    
    /// <param name="Email">Email of the user</param>
    /// <param name="Name">Name of the user</param>
    /// <param name="Phone">Phone of the user</param>
    public sealed record UpdateUserResponse(string Email, string Name, string Phone);
}