using MamisSolidarias.HttpClient.Users.Models;
using MamisSolidarias.HttpClient.Users.Services;
using Microsoft.AspNetCore.Http;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

public partial class UsersClient : IUsersClient
{
    private readonly HeaderService _headerService;
    private readonly IHttpClientFactory _httpClientFactory;

    public UsersClient(IHttpContextAccessor? contextAccessor, IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _headerService = new HeaderService(contextAccessor);
    }

    private ReadyRequest<TResponse> CreateRequest<TResponse>(HttpMethod httpMethod, params string[] urlParams)
    {
        var client = _httpClientFactory.CreateClient("Users");
        var request = new HttpRequestMessage(httpMethod, string.Join('/', urlParams));

        var authHeader = _headerService.GetAuthorization();
        if (authHeader is not null)
            request.Headers.Add("Authorization", authHeader);

        return new ReadyRequest<TResponse>(client, request);
    }

    private ReadyRequest CreateRequest(HttpMethod httpMethod, params string[] urlParams)
    {
        var client = _httpClientFactory.CreateClient("Users");
        var request = new HttpRequestMessage(httpMethod, string.Join('/', urlParams));

        var authHeader = _headerService.GetAuthorization();
        if (authHeader is not null)
            request.Headers.Add("Authorization", authHeader);

        return new ReadyRequest(client, request);
    }
}