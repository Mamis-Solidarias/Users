


namespace MamisSolidarias.HttpClient.Users.UsersClient;

public interface IUsersClient
{
    Task<WebAPI.Users.Endpoints.Users.POST.Response?> CreateUser(WebAPI.Users.Endpoints.Users.POST.Request parameters, CancellationToken token = default);

    Task<WebAPI.Users.Endpoints.Users.Auth.POST.Response?> SignIn(WebAPI.Users.Endpoints.Users.Auth.POST.Request request, CancellationToken token = default);
}