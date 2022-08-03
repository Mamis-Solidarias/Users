using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.HttpClient.Users.Utils;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

internal class CreateUserTest
{
    private readonly MockHttpMessageHandler _httpHandlerMock = new();
    private readonly Mock<IHttpClientFactory> _httpClientFactory = new();
    private readonly IConfiguration _configuration = ConfigurationFactory.GetUsersConfiguration();
    private UsersClient _client = null!;

    [SetUp]
    public void CreateHttpRequest()
    {
        _httpClientFactory.Setup(t => t.CreateClient("Users"))
            .Returns(new System.Net.Http.HttpClient(_httpHandlerMock)
            {
                BaseAddress = new Uri(_configuration["UsersHttpClient:BaseUrl"])
            });
        _client = new UsersClient(null,_httpClientFactory.Object);
    }

    [TearDown]
    public void DisposeHttpRequest()
    {
        _httpHandlerMock.Clear();
    }
    
    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser();
        
        var request = new Request
        {
            Email = user.Email,
            Name = user.Name,
            Password = user.Password,
            Phone =user.Phone
        };

        _httpHandlerMock.When(HttpMethod.Post, _configuration["UsersHttpClient:BaseUrl"] + "/users")
            .Respond(HttpStatusCode.Created, JsonContent.Create(request));
        
        // Act
        var result = await _client.CreateUser(request);

        // Assert
        result.Should().NotBeNull();
        result?.Email.Should().Be(user.Email);
        result?.Name.Should().Be(user.Name);
        result?.Phone.Should().Be(user.Phone);
    }
    
    
    [Test]
    public async Task WithInvalidParameters_Fails()
    {
        // Arrange
        var request = new Request();
        
        _httpHandlerMock.When(HttpMethod.Post, _configuration["UsersHttpClient:BaseUrl"] + "/users")
            .Respond(HttpStatusCode.BadRequest, JsonContent.Create(request));

        // Act
        var action = async () => await _client.CreateUser(request);

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>();

    }
    
}