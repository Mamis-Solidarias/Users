using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.Utils.Test;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.PATCH;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using static FastEndpoints.Security.Constants;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Id_Patch
{
    private readonly Mock<DbAccess> _mockDbService = new ();
    private readonly Mock<ClaimsPrincipal> _mockClaims = new() {CallBase = true};
    private Endpoint _endpoint = null!;

    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory
            .CreateEndpoint<Endpoint>(null, _mockDbService.Object)
            .WithClaims(_mockClaims.Object)
            .Build();
    }

    [Test]
    public async Task WithValidParameters_UpdatingAllValues_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        
        var claims = new[] {new Claim(PermissionsClaimType,"Users/write")};
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        _mockDbService.Setup(t => t.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockDbService.Setup(t => t.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var req = new Request
        {
            Id = user.Id,
            Phone = "+5491190901010",
            Email = "new@mail.com",
            Name = "Lucas Dell'Isola"
        };
        
        // Act
        await _endpoint.HandleAsync(req, default);
        var response = _endpoint.Response;
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
        response.Should().NotBeNull();
        response.Phone.Should().Be(req.Phone);
        response.Email.Should().Be(req.Email);
        response.Name.Should().Be(req.Name);
    }
    
    [Test]
    public async Task WithValidParameters_UpdatingSomeValues_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        
        var claims = new[] {new Claim(PermissionsClaimType,"Users/write")};
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        _mockDbService.Setup(t => t.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockDbService.Setup(t => t.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var req = new Request
        {
            Id = user.Id,
            Email = "new@mail.com",
        };
        
        // Act
        await _endpoint.HandleAsync(req, default);
        var response = _endpoint.Response;
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
        response.Should().NotBeNull();
        response.Phone.Should().Be(user.Phone);
        response.Email.Should().Be(req.Email);
        response.Name.Should().Be(user.Name);
    }
    
    [Test]
    public async Task WithValidParameters_AsAccountOwner_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        
        var claims = new[] {new Claim("Id",$"{user.Id}")};
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        _mockDbService.Setup(t => t.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockDbService.Setup(t => t.UpdateUser(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var req = new Request
        {
            Id = user.Id,
            Phone = "+5491190901010",
            Email = "new@mail.com",
            Name = "Lucas Dell'Isola"
        };
        
        // Act
        await _endpoint.HandleAsync(req, default);
        var response = _endpoint.Response;
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
        response.Should().NotBeNull();
        response.Phone.Should().Be(req.Phone);
        response.Email.Should().Be(req.Email);
        response.Name.Should().Be(req.Name);
    }
    
    [Test]
    public async Task WithInvalidParameters_NoPermissions_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity()});

        var req = new Request
        {
            Id = user.Id,
            Phone = "+5491190901010",
            Email = "new@mail.com",
            Name = "Lucas Dell'Isola"
        };
        
        // Act
        await _endpoint.HandleAsync(req, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(403);
    }
    
    [Test]
    public async Task WithInvalidParameters_OtherUser_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        
        var claims = new[] {new Claim("Id",$"{123}")};
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        var req = new Request
        {
            Id = user.Id,
            Phone = "+5491190901010",
            Email = "new@mail.com",
            Name = "Lucas Dell'Isola"
        };
        
        // Act
        await _endpoint.HandleAsync(req, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(403);
    }
    
    [Test]
    public async Task WithInvalidParameters_UserDoesNotExist_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        
        var claims = new[] {new Claim(PermissionsClaimType,"Users/write")};
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        _mockDbService.Setup(t => t.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var req = new Request
        {
            Id = user.Id,
            Phone = "+5491190901010",
            Email = "new@mail.com",
            Name = "Lucas Dell'Isola"
        };
        
        // Act
        await _endpoint.HandleAsync(req, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(404);
    }

    [Test]
    public async Task WithInvalidParameters_EmailAlreadyExists_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        
        var claims = new[] {new Claim(PermissionsClaimType,"Users/write")};
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        _mockDbService.Setup(t => t.GetUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockDbService.Setup(t => t.UpdateUser(
                It.Is<User>(r=> r.Email == user.Email), 
                It.IsAny<CancellationToken>())
            )
            .ThrowsAsync(new DbUpdateException());

        var req = new Request
        {
            Id = user.Id,
            Phone = "+5491190901010",
            Email = user.Email,
            Name = "Lucas Dell'Isola"
        };
        
        // Act
        await _endpoint.HandleAsync(req, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }

}