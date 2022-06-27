using MamisSolidarias.HttpClient.Users.UsersClient;
using Microsoft.Extensions.DependencyInjection;

namespace MamisSolidarias.HttpClient.Users;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// It registers the UsersHttpClient using dependency injection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUsersHttpClient(this IServiceCollection services)
    {
        services.AddSingleton<IUsersClient, UsersClient.UsersClient>();
        return services;
    }
}