using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.Utils.Test;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Roles.GET;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

// ReSharper disable once InconsistentNaming
internal class Users_Roles_Get
{
    private Endpoint _endpoint = null!;

    [SetUp]
    public void Setup()
    {
        _endpoint = EndpointFactory
            .CreateEndpoint<Endpoint>()
            .Build();
    }

    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        // Arrange
        var services = Enum.GetValues<MamisSolidarias.Utils.Security.Services>()
            .Select(t => t.ToString())
            .ToArray();
        // Act
        await _endpoint.HandleAsync(default);
        var response = _endpoint.Response;
        
        //Assert
        response.Should().NotBeNull();
        response.Roles.Count().Should().Be(services.Length);
        response.Roles.Select(t => t.Service).Should().BeEquivalentTo(services);
    }
}