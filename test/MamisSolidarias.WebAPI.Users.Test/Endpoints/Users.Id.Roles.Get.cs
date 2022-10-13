using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.GET;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using EndpointFactory = MamisSolidarias.Utils.Test.EndpointFactory;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Id_Roles_Get
{
    private readonly Mock<ILogger<Endpoint<Request,Response>>> _mockLogger = new();
    private readonly Mock<DbAccess> _mockDbAccess = new(); 
    private readonly Mock<ClaimsPrincipal> _mockClaims = new(){CallBase = true};
    private Endpoint _endpoint = null!;
    
    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory
            .CreateEndpoint<Endpoint>(null, _mockDbAccess.Object)
            .WithClaims(_mockClaims.Object)
            .Build();
    }

    [TearDown]
    public void Dispose()
    {
        _mockClaims.Reset();
        _mockLogger.Reset();
        _mockDbAccess.Reset();
    }

    [Test]
    public async Task WithValidParameters_WithPermission_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var claims = new[] {new Claim(Constants.PermissionsClaimType,"Users/read")};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        _mockDbAccess.Setup(t => t.GetUserById(It.Is<int>(r=> r == user.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        var request = new Request {Id = user.Id};
        
        // Act
        await _endpoint.HandleAsync(request, default);
        var response = _endpoint.Response;
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
        response.Should().NotBeNull();
        response.Roles.Should().BeEquivalentTo(
            user.Roles.Select(t =>
                new RoleResponse(t.Service.ToString(), t.CanWrite, t.CanRead)
            )
        );
        
    }

    [Test]
    public async Task WithValidParameters_AsAccountOwner_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var claims = new[] {new Claim("Id",user.Id.ToString())};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        _mockDbAccess.Setup(t => t.GetUserById(It.Is<int>(r=> r == user.Id), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        var request = new Request {Id = user.Id};
        
        // Act
        await _endpoint.HandleAsync(request, default);
        var response = _endpoint.Response;
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
        response.Should().NotBeNull();
        response.Roles.Should().BeEquivalentTo(
            user.Roles.Select(t =>
                new RoleResponse(t.Service.ToString(), t.CanWrite, t.CanRead)
            )
        );
    }

    [Test]
    public async Task WithInvalidParameters_NoPermission_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        Claim[] claims = System.Array.Empty<Claim>();
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        var request = new Request {Id = user.Id};
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(403);
    }

    [Test]
    public async Task WithInvalidParameters_NotAccountOwner_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var claims = new[] {new Claim("Id", 123.ToString())};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        var request = new Request {Id = user.Id};
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(403);
    }

    [Test]
    public async Task WithInvalidParameters_UserNotFound_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var claims = new[] {new Claim(Constants.PermissionsClaimType,"Users/read")};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        _mockDbAccess.Setup(t => t.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        
        var request = new Request {Id = user.Id};
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(404);
    }
}