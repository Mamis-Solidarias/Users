
namespace MamisSolidarias.HttpClient.Users.UsersClient;

public interface IUsersClient
{
    Task<WebAPI.Users.Endpoints.Users.POST.Response?> CreateUser(
        WebAPI.Users.Endpoints.Users.POST.Request parameters,
        CancellationToken token = default
        );

    Task<WebAPI.Users.Endpoints.Users.Auth.POST.Response?> SignIn(
        WebAPI.Users.Endpoints.Users.Auth.POST.Request request, 
        CancellationToken token = default
        );

    Task UpdatePassword(
        MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Password.PUT.Request request,
        CancellationToken token = default
        );

    Task<WebAPI.Users.Endpoints.Users.GET.Response?> GetUsers(
        WebAPI.Users.Endpoints.Users.GET.Request request,
        CancellationToken token = default
        );

    Task<WebAPI.Users.Endpoints.Users.Roles.GET.Response?> GetRoles(CancellationToken token = default);

    Task<WebAPI.Users.Endpoints.Users.Id.GET.Response?> GetUser(
        WebAPI.Users.Endpoints.Users.Id.GET.Request request,
        CancellationToken token = default
        );

    Task<WebAPI.Users.Endpoints.Users.Id.Roles.GET.Response?> GetUsersRoles(
        WebAPI.Users.Endpoints.Users.Id.Roles.GET.Request request, 
        CancellationToken token = default
        );

    Task DeleteUser(WebAPI.Users.Endpoints.Users.Id.DELETE.Request request, CancellationToken token = default);

    Task<MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH.Response?> UpdateUser(
        MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH.Request request,
        CancellationToken token = default
    );

    Task<MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT.Response?> UpdateRoles(
        MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT.Request request,
        CancellationToken token = default
    );

    Task ActivateUser(
        MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.POST.Request request,
        CancellationToken token = default
    );
}