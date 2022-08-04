using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;
using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;


namespace MamisSolidarias.WebAPI.Users.Endpoints;

internal class Users_Auth_Post
{
    private readonly Mock<ILogger<Endpoint<Request, Response>>> _mockLogger = new();
    private readonly Mock<ITextHasher> _mockedTextHasher = new();
    private readonly Mock<DbAccess> _mockDbAccess = new();
    private Endpoint _endpoint = null!;
    private const string JwtKey = "kjdaskjdlaksjdlaksjdlaksjdlaskdjlaskjdlaskjdalksjdaljks";

    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"JWT:Key", JwtKey},
            { "JWT:ExpiresIn","10"},
        };

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        
        _endpoint = EndpointFactory.CreateEndpoint<Endpoint, Request, Response>(
            s => s.AddSingleton(_mockLogger.Object),
            null, _mockDbAccess.Object,_mockedTextHasher.Object, config
        );
    }

    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        // Arrange
        var jwtHandler = new JwtSecurityTokenHandler();
        var user = DataFactory.GetUser();
        var request = new Request
        {
            Email = user.Email,
            Password = user.Password
        };

        _mockDbAccess.Setup(t => t.FindUserByEmail(It.Is<string>(r => r == user.Email)))
            .ReturnsAsync(user);

        _mockedTextHasher.Setup(t => t.Hash(It.IsAny<string>(),It.IsAny<byte[]>()))
            .Returns((Convert.FromBase64String(user.Salt), user.Password));

        // Act
        await _endpoint.HandleAsync(request,default);
        var response = _endpoint.Response;
        
        // Assert
        response.Should().NotBeNull();
        
        var jwtToken = jwtHandler.ReadJwtToken(response.Jwt);
        jwtToken.Claims.First(t => t.Type is "Id").Value.Should().Be(user.Id.ToString());
        jwtToken.Claims.First(t => t.Type is "Email").Value.Should().Be(user.Email);
        jwtToken.Claims.First(t => t.Type is "Name").Value.Should().Be(user.Name);

        foreach (var role in user.Roles)
        {
            if (role.CanRead)
                jwtToken.Claims.FirstOrDefault(t => t.Type is "permissions" && t.Value == $"{role.Service}/read")
                    .Should().NotBeNull();
            if (role.CanWrite)
                jwtToken.Claims.FirstOrDefault(t => t.Type is "permissions" && t.Value == $"{role.Service}/write")
                    .Should().NotBeNull();
        }
    }

    [Test]
    public async Task WithInvalidEmail_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        _mockDbAccess.Setup(t => t.FindUserByEmail(It.Is<string>(r=> r == user.Email)))
            .ReturnsAsync((User?)null);
        var request = new Request
        {
            Email = user.Email,
            Password = user.Password
        };
        
        // Act
        await _endpoint.HandleAsync(request, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(401);
    }
    
    [Test]
    public async Task WithInvalidPassword_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        _mockDbAccess.Setup(t => t.FindUserByEmail(It.Is<string>(r=> r == user.Email)))
            .ReturnsAsync(user);
        _mockedTextHasher.Setup(t => t.Hash(It.IsAny<string>(),It.IsAny<byte[]>()))
            .Returns((Convert.FromBase64String(user.Salt), "other password"));
        var request = new Request
        {
            Email = user.Email,
            Password = user.Password
        };
        
        // Act
        await _endpoint.HandleAsync(request, default);

        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(401);
    }
}