using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.Utils.Test;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.POST;
using MamisSolidarias.WebAPI.Users.Utils;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Id_Post
{
    private Endpoint _endpoint = null!;
    private readonly Mock<DbAccess> _mockDbAccess = new();

    [SetUp]
    public void SetUp()
    {
        _endpoint = EndpointFactory
            .CreateEndpoint<Endpoint>(null, _mockDbAccess.Object)
            .Build();
    }

    [TearDown]
    public void Teardown()
    {
        _mockDbAccess.Reset();
    }

    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        // Arrange
        var user = DataFactory.GetUser()
	        .IsActive(false)
	        .Build();
        var request = new Request {Id = user.Id};
        
        _mockDbAccess.Setup(t =>
            t.GetUserById(It.Is<int>(r => r == user.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(user);
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(200);
        user.IsActive.Should().BeTrue();
    }

    [Test]
    public async Task WithInvalidParameters_UserNotFound_Fails()
    {
        // Arrange
        var request = new Request {Id = 123};
        
        _mockDbAccess.Setup(t =>
            t.GetUserById(It.Is<int>(r => r == request.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync((User?)null);
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(404);
    }

    [Test]
    public async Task WithInvalidParameters_UserAlreadyActive_Fails()
    {
        // Arrange
        var user = DataFactory.GetUser().Build();
        var request = new Request {Id = user.Id};
        
        _mockDbAccess.Setup(t =>
            t.GetUserById(It.Is<int>(r => r == user.Id), It.IsAny<CancellationToken>())
        ).ReturnsAsync(user);
        
        // Act
        await _endpoint.HandleAsync(request, default);
        
        // Assert
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
}