
using MamisSolidarias.HttpClient.Users.Models;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

/// <summary>
/// Client for the Users API
/// </summary>
public interface IUsersClient
{

    /// <summary>
    /// It creates a new user
    /// </summary>
    /// <param name="parameters"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.CreateUserResponse?> CreateUser(UsersClient.CreateUserRequest parameters, CancellationToken token);

    /// <summary>
    /// It creates a Jwt token for the user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.SignInResponse?> SignIn(UsersClient.SignInRequest request, CancellationToken token);

    /// <summary>
    /// It updates the user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task UpdatePassword(int id,UsersClient.UpdatePasswordRequest request, CancellationToken token);

    /// <summary>
    /// It retrieves a list of users
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.GetUsersResponse?> GetUsers(UsersClient.GetUsersRequest request, CancellationToken token);

    /// <summary>
    /// It retrieves the list of roles
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.GetRolesResponse?> GetRoles(CancellationToken token);

    /// <summary>
    /// It retrieves the user's information
    /// </summary>
    /// <param name="id">User's id</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.GetUserResponse?> GetUser(int id, CancellationToken token);

    /// <summary>
    /// It returns the roles of the user
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.GetUsersRolesResponse?> GetUsersRoles(int id, CancellationToken token);

    /// <summary>
    /// It deactivates a user
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task DeleteUser(int id, CancellationToken token);

    /// <summary>
    /// It updates a user's information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.UpdateUserResponse?> UpdateUser(int id, UsersClient.UpdateUserRequest request, CancellationToken token);

    /// <summary>
    /// It updates the roles of a user
    /// </summary>
    /// <param name="id"></param>
    /// <param name="roles"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UsersClient.UpdateRolesResponse?> UpdateRoles(int id, IEnumerable<Role> roles, CancellationToken token);

    /// <summary>
    /// It reactivates a user
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task ActivateUser(UsersClient.ActivateUserRequest request, CancellationToken token);
}