using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.HttpClient.Users.Utils;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

internal class SignInTest
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
            Password = user.Password,
        };
        var response = new Response
        {
            Jwt =
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJFbWFpbCI6ImFkbWluQG1haWwuY29tIiwiSWQiOiIxMSIsIk5hbWUiOiJBZ" +
                "G1pbiBBZG1pbiIsIm5iZiI6MTY1OTYxODAwNiwiZXhwIjoxNjYwNDgyMDA2LCJpYXQiOjE2NTk2MTgwMDZ9.KFofld5Z2JSjDJ" +
                "pq-jTnfql0mWya5x1YJlhqtHL07XY"
        };
        

        _httpHandlerMock.When(HttpMethod.Post, _configuration["UsersHttpClient:BaseUrl"] + "/users/auth")
            .Respond(HttpStatusCode.OK, JsonContent.Create(response));
        
        // Act
        var result = await _client.SignIn(request);

        // Assert
        result.Should().NotBeNull();
        response.Should().Be(response);
    }
    
    
    
    [Test]
    public async Task WithIncorrectEmail_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        
        var request = new Request
        {
            Email = "invalid@mail.com",
            Password = user.Password,
        };

        _httpHandlerMock.When(HttpMethod.Post, _configuration["UsersHttpClient:BaseUrl"] + "/users/auth")
            .Respond(HttpStatusCode.Unauthorized);
        
        // Act
        var action = async () => await _client.SignIn(request);

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>();
    }
    
    [Test]
    public async Task WithIncorrectPassword_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        
        var request = new Request
        {
            Email = user.Email,
            Password = "InvalidPass123!",
        };

        _httpHandlerMock.When(HttpMethod.Post, _configuration["UsersHttpClient:BaseUrl"] + "/users/auth")
            .Respond(HttpStatusCode.Unauthorized);
        
        // Act
        var action = async () => await _client.SignIn(request);

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>();
    }
    
    [Test]
    public async Task WithInvalidEmail_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        
        var request = new Request
        {
            Email = "not an email",
            Password = user.Password,
        };

        _httpHandlerMock.When(HttpMethod.Post, _configuration["UsersHttpClient:BaseUrl"] + "/users/auth")
            .Respond(HttpStatusCode.BadRequest);
        
        // Act
        var action = async () => await _client.SignIn(request);

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>();
    }
    
    [Test]
    public async Task WithInvalidPassword_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        
        var request = new Request
        {
            Email = user.Email,
            Password = "easy",
        };

        _httpHandlerMock.When(HttpMethod.Post, _configuration["UsersHttpClient:BaseUrl"] + "/users/auth")
            .Respond(HttpStatusCode.BadRequest);
        
        // Act
        var action = async () => await _client.SignIn(request);

        // Assert
        await action.Should().ThrowAsync<HttpRequestException>();
    }
}