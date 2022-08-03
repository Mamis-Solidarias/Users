using MamisSolidarias.HttpClient.Users.Models;
using MamisSolidarias.HttpClient.Users.UsersClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MamisSolidarias.HttpClient.Users;

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
        
        builder.Services.AddSingleton<IUsersClient, UsersClient.UsersClient>();
        builder.Services.AddHttpClient("Users", client =>
        {
            client.BaseAddress = new Uri(configuration.BaseUrl);
            client.Timeout = TimeSpan.FromMilliseconds(configuration.Timeout);
        }).AddTransientHttpErrorPolicy(t =>
            t.WaitAndRetryAsync(configuration.Retries,
                    retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)))
        );
    }
}  