using Flurl;
using Flurl.Http;
using MamisSolidarias.HttpClient.Users.Models;
using MamisSolidarias.HttpClient.Users.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient : IUsersClient
{
    private readonly UsersConfiguration _configuration;
    private readonly HeaderService _headerService;

    public UsersClient(IHttpContextAccessor ctxa, IConfiguration configuration)
    {
        _configuration = new UsersConfiguration();
        configuration.GetSection("UsersHttpClient").Bind(_configuration);
        _headerService = new HeaderService(ctxa);
    }

    private ReadyRequest<T> CreateRequest<T>(params string[] urlParams)
    {
        var url = new Url(_configuration.BaseUrl).AppendPathSegments(urlParams as object[]);
        var authHeader = _headerService.GetAuthorization();
        var request = authHeader switch
        {
            null => new FlurlRequest(url),
            _ => new FlurlRequest(url).WithHeader("Authorization", authHeader)
        };

        return new ReadyRequest<T>(request, _configuration);
    }
}