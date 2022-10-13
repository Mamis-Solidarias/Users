using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentAssertions;

using MamisSolidarias.WebAPI.Users.Endpoints.Users.GET;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using EndpointFactory = MamisSolidarias.Utils.Test.EndpointFactory;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Get
{
    private readonly Mock<ILogger<Endpoint<Request, Response>>> _mockLogger = new();
    private readonly Mock<DbAccess> _mockDbService = new ();
    private Endpoint _endpoint = null!;

    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory
            .CreateEndpoint<Endpoint>(null,_mockDbService.Object)
            .Build();
    }

    [TearDown]
    public void Dispose()
    {
        _mockLogger.Reset();
        _mockDbService.Reset();
    }

    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        // Arrange
        var users = DataFactory.GetUsers(10).ToArray();
        var request = new Request
        {
            PageSize = 10,
            Page = 0
        };
        
        _mockDbService.Setup(t => t.GetPaginatedUsers(
            It.Is<string?>(r => r == null), It.Is<int>(r => r == request.Page),
            It.Is<int>(r => r == request.PageSize), It.IsAny<CancellationToken>()
        )).ReturnsAsync(users);

        _mockDbService.Setup(t => t.GetTotalEntries(It.Is<string?>(r => r == null), It.IsAny<CancellationToken>()))
            .ReturnsAsync(25);
        
        // Act
        await _endpoint.HandleAsync(request, default);
        var response = _endpoint.Response;
        
        // Assert
        response.Should().NotBeNull();

        response.Entries.Should().BeEquivalentTo(users.Select(t=> 
            new UserResponse(t.Id,t.Name,t.Email,t.Phone,t.IsActive ?? true, t.Roles.Select(r=>
                new RoleResponse(r.Service.ToString(),r.CanWrite,r.CanRead)))
            )
        );
        response.Page.Should().Be(0);
        response.TotalPages.Should().Be(3);
    }
    
    [Test]
    public async Task WithValidParameters_NoUsers_Succeeds()
    {
        // Arrange
        var users = DataFactory.GetUsers(0).ToArray();
        var request = new Request
        {
            PageSize = 10,
            Page = 0
        };
        
        _mockDbService.Setup(t => t.GetPaginatedUsers(
            It.Is<string?>(r => r == null), It.Is<int>(r => r == request.Page),
            It.Is<int>(r => r == request.PageSize), It.IsAny<CancellationToken>()
        )).ReturnsAsync(users);

        _mockDbService.Setup(t => t.GetTotalEntries(It.Is<string?>(r => r == null), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        
        // Act
        await _endpoint.HandleAsync(request, default);
        var response = _endpoint.Response;
        
        // Assert
        response.Should().NotBeNull();
        response.Entries.Should().BeEmpty();
        response.Page.Should().Be(0);
        response.TotalPages.Should().Be(0);
    }
}

















