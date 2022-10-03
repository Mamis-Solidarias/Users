using MamisSolidarias.HttpClient.Users.Models;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    /// <inheritdoc />
    public Task<UpdateRolesResponse?> UpdateRoles(int id,IEnumerable<Role> roles, CancellationToken token = default)
    {
        return CreateRequest(HttpMethod.Put, "users", $"{id}", "roles")
            .WithContent(new
            {
                Roles = roles
            })
            .ExecuteAsync<UpdateRolesResponse>(token);
    }


    /// <param name="Roles">New roles</param>
    public sealed record UpdateRolesResponse(IEnumerable<Role> Roles);

}