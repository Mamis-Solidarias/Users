using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace MamisSolidarias.HttpClient.Users.Utils;

internal class ConfigurationFactory
{
    internal static IConfiguration GetUsersConfiguration(
        string baseUrl = "https://test.com", int retries = 3, int timeout = 5
    )
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"UsersHttpClient:BaseUrl", baseUrl},
            {"UsersHttpClient:Retries", retries.ToString()},
            {"UsersHttpClient:Timeout", timeout.ToString()}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }
}