using MamisSolidarias.HttpClient.Users.Models;


namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task<GetUsersRolesResponse?> GetUsersRoles(int id, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Get, "users", $"{id}", "roles")
            .ExecuteAsync<GetUsersRolesResponse>(token);
    }
    
    /// <param name="Roles">Roles of the user</param>
    public sealed record GetUsersRolesResponse(
        IEnumerable<Role> Roles
    );
}