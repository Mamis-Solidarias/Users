using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.Utils.Test;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;
using MamisSolidarias.WebAPI.Users.Utils;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Id_Delete
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
    public async Task WithValidParameters_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request{Id = user.Id};
        _mockDbService.Setup(
                t => t.GetUserById(It.Is<int>(r => r == user.Id),
                    It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(user);

        _mockDbService.Setup(
                t => t.SoftDeleteUser(It.Is<User>(r => r == user),
                    It.IsAny<CancellationToken>())
            )
            .Callback(() => user.IsActive = false)
            .Returns(Task.CompletedTask);
        
        var claims = new[] {new Claim("Id",$"{123}")};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
    }
    
    [Test]
    public async Task WithInvalidParameters_UserDoesNotExists_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request{Id = user.Id};
        
        _mockDbService.Setup(
                t => t.GetUserById(It.Is<int>(r => r == user.Id),
                    It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((User?)null);
        
        var claims = new[] {new Claim("Id",$"{123}")};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(404);
    }
    
    [Test]
    public async Task WithInvalidParameters_UserDeletesItself_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser();
        var request = new Request{Id = user.Id};
        
        _mockDbService.Setup(
                t => t.GetUserById(It.Is<int>(r => r == user.Id),
                    It.IsAny<CancellationToken>())
            )
            .ReturnsAsync((User?)null);
        
        var claims = new[] {new Claim("Id",$"{user.Id}")};
        
        _mockClaims.SetupGet(t => t.Identities)
            .Returns(new[] {new ClaimsIdentity(claims)});

        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
}