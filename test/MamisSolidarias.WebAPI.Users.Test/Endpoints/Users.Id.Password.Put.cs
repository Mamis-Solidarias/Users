using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.Utils.Test;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Password.PUT;
using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Id_Password_Put
{
    private readonly Mock<ILogger<Endpoint<Request>>> _mockLogger = new();
    private readonly Mock<TextHasher> _mockedTextHasher = new() {CallBase = true};
    private readonly Mock<DbAccess> _mockDbAccess = new(); 
    private readonly Mock<ClaimsPrincipal> _mockClaims = new(){CallBase = true};
    private Endpoint _endpoint = null!;
    
    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory
            .CreateEndpoint<Endpoint>(_mockedTextHasher.Object, null, _mockDbAccess.Object)
            .WithClaims(_mockClaims.Object)
            .Build();
    }

    [TearDown]
    public void Dispose()
    {
        _mockClaims.Reset();
        _mockLogger.Reset();
        _mockDbAccess.Reset();
        _mockedTextHasher.Reset();
    }

    [Test]
    public async Task IsAdmin_WithValidArguments_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request
        {
            Id = user.Id,
            OldPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        var claims = new[] {new Claim(Constants.PermissionsClaimType,"Users/write")};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        _mockDbAccess.Setup(t => t.FindUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        _mockDbAccess.Setup(t => t.UpdatePassword(
            It.IsAny<User>(), It.IsAny<string>(), It.IsAny<CancellationToken>())
        ).Returns(Task.CompletedTask);
        
        _mockedTextHasher.Setup(t => t.Hash(It.Is<string>(r => r == request.OldPassword), It.IsAny<byte[]>()))
            .Returns((Convert.FromBase64String(user.Salt), user.Password));
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
    }
    
    [Test]
    public async Task IsAccountOwner_WithValidArguments_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request
        {
            Id = user.Id,
            OldPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        var claims = new[] {new Claim("Id",user.Id.ToString())};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        _mockDbAccess.Setup(t => t.FindUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockDbAccess.Setup(t => t.UpdatePassword(
            It.IsAny<User>(), It.IsAny<string>(), It.IsAny<CancellationToken>())
        ).Returns(Task.CompletedTask);
        _mockedTextHasher.Setup(t => t.Hash(It.Is<string>(r => r == request.OldPassword), It.IsAny<byte[]>()))
            .Returns((Convert.FromBase64String(user.Salt), user.Password));
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
    }
    
    [Test]
    public async Task NotAuthorized_WithValidArguments_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request
        {
            Id = user.Id,
            OldPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(403);
    }
    
    [Test]
    public async Task WithInvalidArguments_UserNotFound_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request
        {
            Id = user.Id,
            OldPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        var claims = new[] {new Claim(Constants.PermissionsClaimType,"Users/write")};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        _mockDbAccess.Setup(t => t.FindUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.ValidationFailed.Should().BeFalse();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(404);
    }
    
    [Test]
    public async Task WithInvalidArguments_OldPasswordDoesNotMatch_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request
        {
            Id = user.Id,
            OldPassword = "OldPassword123!",
            NewPassword = "NewPassword123!"
        };

        var claims = new[] {new Claim("Id",user.Id.ToString())};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        _mockDbAccess.Setup(t => t.FindUserById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        
        _mockedTextHasher.Setup(t => t.Hash(It.Is<string>(r => r == request.OldPassword), It.IsAny<byte[]>()))
            .Returns((Convert.FromBase64String(user.Salt), "Some other hash"));
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.ValidationFailed.Should().BeTrue();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
}