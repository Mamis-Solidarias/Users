using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Auth.POST;
using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.Utils;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

internal class UsersAuthPost : EndpointTest<Endpoint>
{
	private readonly Mock<IRolesCache> _mockRolesCache = new();
	private readonly ITextHasher _textHasher = new TextHasher();

	protected override object?[] ConstructorArguments => new object?[]
	{
		_dbContext, _textHasher, _mockRolesCache.Object
	};


	[Test]
	public async Task WithValidParameters_Succeeds()
	{
		// Arrange
		const string plainTextPassword = "Hello1234!";
		var user = _dataFactory.GenerateUser()
			.WithPassword(plainTextPassword)
			.Build();

		var request = new Request
		{
			Email = user.Email,
			Password = plainTextPassword
		};

		// Act
		await _endpoint.HandleAsync(request, default);
		var response = _endpoint.Response;

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(200);
		response.Should().NotBeNull();
		response.Jwt.Should().NotBeNull();

		var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(response.Jwt);
		jwtToken.Claims.First(t => t.Type is "Id").Value.Should().Be(user.Id.ToString());
		jwtToken.Claims.First(t => t.Type is "Email").Value.Should().Be(user.Email);
		jwtToken.Claims.First(t => t.Type is "Name").Value.Should().Be(user.Name);
		jwtToken.Claims.Should().NotContain(t => t.Type == "permissions");


	}

	[Test]
	public async Task WithInvalidEmail_Fails()
	{
		// Arrange
		const string email = "invalid email";
		const string plainTextPassword = "Hello1234!";
		var request = new Request
		{
			Email = email,
			Password = plainTextPassword
		};

		// Act
		await _endpoint.HandleAsync(request, default);

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(401);
	}

	[Test]
	public async Task WithInvalidPassword_Fails()
	{
		// Arrange
		const string invalidTextPassword = "Hello1234!";
		const string realTextPassword = "SecurePass123";
		var user = _dataFactory.GenerateUser()
			.WithPassword(realTextPassword)
			.Build();

		var request = new Request
		{
			Email = user.Email,
			Password = invalidTextPassword
		};

		// Act
		await _endpoint.HandleAsync(request, default);

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(401);
	}
}