
using MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient
{
    public Task<Response?> GetUsers(Request request, CancellationToken token = default)
    {
        return CreateRequest<Response>(HttpMethod.Get,"users")
            .WithQuery(
                ("Search",request.Search),
                ("Page",request.Page.ToString()),
                ("PageSize",request.PageSize.ToString())
            )
            .ExecuteAsync(token);
    }
}