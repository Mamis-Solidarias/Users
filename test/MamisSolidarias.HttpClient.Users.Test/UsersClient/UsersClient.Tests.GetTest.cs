using System.Net.Http;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Flurl;
using Flurl.Http.Testing;
using MamisSolidarias.HttpClient.Users.Utils;
using MamisSolidarias.WebAPI.Users.Endpoints.Test;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace MamisSolidarias.HttpClient.Users.UsersClient;

internal class UsersClientTestsGetTest
{
    private HttpTest? _httpRequest;

    [SetUp]
    public void CreateHttpRequest()
    {
        _httpRequest = new HttpTest();
    }

    [TearDown]
    public void DisposeHttpRequest()
    {
        _httpRequest?.Dispose();
    }


    [Test]
    public async Task WithValidParameters_Succeeds()
    {
        // arrange
        var contextAccessor = A.Fake<IHttpContextAccessor>();
        var configuration = ConfigurationFactory.GetUsersConfiguration();

        var client = new UsersClient(contextAccessor, configuration);
        var request = new Request {Name = "lucas"};
        var expectedResponse = new Response
        {
            Name = request.Name,
            Email = "me@mail.com",
            Id = 123
        };


        _httpRequest?
            .ForCallsTo(Url.Combine("*", "user", request.Name))
            .WithVerb(HttpMethod.Get)
            .RespondWithJson(expectedResponse);

        // act
        var (status, response) = await client.GetTestAsync(request);

        // assert
        _httpRequest?.ShouldHaveMadeACall();
        status.Should().Be(200);
        response.Should().NotBeNull();
    }

    [Test]
    public async Task WithInvalidUser_ThrowsNotFound()
    {
        // arrange
        var contextAccessor = A.Fake<IHttpContextAccessor>();
        var configuration = ConfigurationFactory.GetUsersConfiguration();

        var client = new UsersClient(contextAccessor, configuration);
        var request = new Request {Name = "lucas"};


        _httpRequest?
            .ForCallsTo(Url.Combine("*", "user", request.Name))
            .WithVerb(HttpMethod.Get)
            .RespondWith(status: 404);

        // act
        var (status, response) = await client.GetTestAsync(request);

        // assert
        _httpRequest?.ShouldHaveMadeACall();
        response.Should().BeNull();
        status.Should().Be(404);
    }
}