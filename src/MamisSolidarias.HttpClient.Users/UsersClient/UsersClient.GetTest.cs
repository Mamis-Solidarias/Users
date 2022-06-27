using Flurl.Http;
using MamisSolidarias.WebAPI.Users.Endpoints.Test;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<(int, Response?)> GetTestAsync(Request requestParameters, CancellationToken token = default)
    {
        return CreateRequest<Response>("user", requestParameters.Name)
            .ExecuteAsync(
                (request, cancellationToken) => request.GetJsonAsync<Response>(cancellationToken),
                token
            );
    }
}