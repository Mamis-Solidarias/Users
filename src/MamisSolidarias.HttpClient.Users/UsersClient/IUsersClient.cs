


using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Password.PUT;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public interface IUsersClient
{
    Task<WebAPI.Users.Endpoints.Users.POST.Response?> CreateUser(WebAPI.Users.Endpoints.Users.POST.Request parameters, CancellationToken token = default);

    Task<WebAPI.Users.Endpoints.Users.Auth.POST.Response?> SignIn(WebAPI.Users.Endpoints.Users.Auth.POST.Request request, CancellationToken token = default);

    Task UpdatePassword(Request request, CancellationToken token = default);

    Task<WebAPI.Users.Endpoints.Users.GET.Response?> GetUsers(WebAPI.Users.Endpoints.Users.GET.Request request, CancellationToken token = default);
    Task<WebAPI.Users.Endpoints.Users.Roles.GET.Response?> GetRoles(CancellationToken token = default);
}