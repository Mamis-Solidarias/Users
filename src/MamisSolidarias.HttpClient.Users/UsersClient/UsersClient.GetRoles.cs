
using MamisSolidarias.HttpClient.Users.Models;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task<GetRolesResponse?> GetRoles(CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Get, "users", "roles")
            .ExecuteAsync<GetRolesResponse>(token);
    }
    
    /// <param name="Roles">Roles assigned to the user</param>
    public sealed record GetRolesResponse(IEnumerable<Role> Roles);
    
    
}