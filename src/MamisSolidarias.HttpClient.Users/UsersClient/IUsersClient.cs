using MamisSolidarias.WebAPI.Users.Endpoints.Test;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public interface IUsersClient
{
    Task<(int, Response?)> GetTestAsync(Request requestParameters, CancellationToken token = default);
}