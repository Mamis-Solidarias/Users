using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.POST;
using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Endpoint = MamisSolidarias.WebAPI.Users.Endpoints.Users.POST.Endpoint;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

internal class Users_Post
{
    private readonly Mock<ILogger<Endpoint<Request, Response>>> _mockLogger = new();
    private readonly ITextHasher _textHasher = new TextHasher();
    private readonly Mock<DbAccess> _mockDbService = new ();
    private Endpoint _endpoint = null!;

    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory.CreateEndpoint<Endpoint, Request, Response>(
            s => s.AddSingleton(_mockLogger.Object),
            _textHasher, null,_mockDbService.Object
        );
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
        //arrange
        var user = DataFactory.GetUser();
        var req = new Request
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Phone = user.Phone
        };

        _mockDbService.Setup(t => t!.AddUser(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns((User us, CancellationToken _) =>
            {
                us.Id = 1;
                return Task.CompletedTask;
            });

        // act
        await _endpoint.HandleAsync(req, default);
        var response = _endpoint.Response;
        
        // assert
        _endpoint.ValidationFailed.Should().BeFalse();
        response.Name.Should().Be(user.Name);
        response.Id.Should().BePositive();
        response.Email.Should().Be(user.Email);
        response.Phone.Should().Be(user.Phone);
    }

    [Test]
    public async Task WithRepeatedEmail_Fails()
    {
        //arrange
        var user = DataFactory.GetUser();
        var req = new Request
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Phone = user.Phone
        };
        
        _mockDbService.Setup(t => t.AddUser(It.Is<User>(r=> r.Email == user.Email), It.IsAny<CancellationToken>()))
            .Throws<DbUpdateException>();

        // act
        await _endpoint.HandleAsync(req, default);
        
        // assert
        _endpoint.ValidationFailed.Should().BeTrue();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);

    }
    
    [Test]
    public async Task WithRepeatedPhone_Fails()
    {
        // arrange
        var user = DataFactory.GetUser();
        var req = new Request
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Phone = user.Phone
        };
        
        _mockDbService.Setup(t => t.AddUser(It.Is<User>(r=> r.Phone == user.Phone), It.IsAny<CancellationToken>()))
            .Throws<DbUpdateException>();

        // act
        await _endpoint.HandleAsync(req, default);
        
        // assert
        _endpoint.ValidationFailed.Should().BeTrue();
        _endpoint.HttpContext.Response.StatusCode.Should().Be(400);
    }
}






















