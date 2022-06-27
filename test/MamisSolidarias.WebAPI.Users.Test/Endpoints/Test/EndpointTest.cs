using System.Threading.Tasks;
using FakeItEasy;
using FastEndpoints;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints.Test;

internal class EndpointTest
{
    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        //arrange
        var logger = A.Fake<ILogger<Endpoint<Request, Response>>>();
        var endpoint = EndpointFactory.CreateEndpoint<Endpoint, Request, Response>(s => s.AddSingleton(logger));

        var req = new Request
        {
            Name = "lucas"
        };

        //act
        await endpoint.HandleAsync(req, default);
        var response = endpoint.Response;

        //assert
        response.Should().NotBeNull();
        endpoint.ValidationFailed.Should().BeFalse();
        response.Name.Should().Be("Lucassss");
        response.Id.Should().BePositive().And.BeLessThanOrEqualTo(10);
        response.Email.Should().Be("mymail@mail.com");
    }

    [Test]
    public async Task WithNonExistentUser_ThrowsNotFound()
    {
        //arrange
        var logger = A.Fake<ILogger<Endpoint<Request, Response>>>();
        var endpoint = EndpointFactory.CreateEndpoint<Endpoint, Request, Response>(s => s.AddSingleton(logger));

        var req = new Request
        {
            Name = "Not lucas"
        };

        //act
        await endpoint.HandleAsync(req, default);
        var statusCode = endpoint.HttpContext.Response.StatusCode;

        //assert
        endpoint.ValidationFailed.Should().BeFalse();
        statusCode.Should().Be(404);
    }
}