
using MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public interface IUsersClient
{
    Task<Response?> CreateUser(Request parameters, CancellationToken token = default);
}