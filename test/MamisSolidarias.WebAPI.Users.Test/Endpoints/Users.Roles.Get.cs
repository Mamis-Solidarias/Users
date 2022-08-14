using System;
using System.Linq;
using System.Threading.Tasks;
using FastEndpoints;
using FluentAssertions;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;
using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Roles_Get
{
    private readonly Mock<ILogger<EndpointWithoutRequest<Response>>> _mockLogger = new();
    private readonly ITextHasher _textHasher = new TextHasher();
    private Endpoint _endpoint = null!;

    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory.CreateEndpointWithoutRequest<Endpoint, Response>(
            s => s.AddSingleton(_mockLogger.Object)
        );
    }

    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        // Arrange
        var services = Enum.GetValues<Infrastructure.Users.Models.Services>()
            .Select(t => t.ToString())
            .ToArray();
        // Act
        await _endpoint.HandleAsync(default);
        var response = _endpoint.Response;
        
        //Assert
        response.Should().NotBeNull();
        response.Roles.Count().Should().Be(services.Length);
        response.Roles.Select(t => t.Service).Should().BeEquivalentTo(services);
        response.Roles.Should().Satisfy(t => t.CanRead && t.CanRead);
    }
}