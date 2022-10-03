using MamisSolidarias.HttpClient.Users.Models;
using MamisSolidarias.HttpClient.Users.Services;
using MamisSolidarias.HttpClient.Users.UsersClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MamisSolidarias.HttpClient.Users;

/// <summary>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// It registers the UsersHttpClient using dependency injection
    /// </summary>
    /// <param name="builder"></param>
    public static void AddUsersHttpClient(this WebApplicationBuilder builder)
    {
        var configuration = new UsersConfiguration();
        builder.Configuration.GetSection("UsersHttpClient").Bind(configuration);

        ArgumentNullException.ThrowIfNull(configuration.BaseUrl);
        ArgumentNullException.ThrowIfNull(configuration.Timeout);
        ArgumentNullException.ThrowIfNull(configuration.Retries);

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IUsersClient, UsersClient.UsersClient>();
        builder.Services.AddHttpClient("Users", (services,client) =>
        {
            client.BaseAddress = new Uri(configuration.BaseUrl);
            client.Timeout = TimeSpan.FromMilliseconds(configuration.Timeout);

            var contextAccessor = services.GetService<IHttpContextAccessor>();
            if (contextAccessor is not null)
            {
                var authHeader = new HeaderService(contextAccessor).GetAuthorization();
                if (authHeader is not null)
                    client.DefaultRequestHeaders.Add("Authorization", authHeader);
            }
            
        }).AddTransientHttpErrorPolicy(t =>
            t.WaitAndRetryAsync(configuration.Retries,
                retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)))
        );
    }
}