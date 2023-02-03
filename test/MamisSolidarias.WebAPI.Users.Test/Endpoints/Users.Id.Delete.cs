using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.DELETE;
using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.Utils;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

internal class Users_Id_Delete : EndpointTest<Endpoint>
{
	private readonly Mock<ClaimsPrincipal> _mockClaims = new() { CallBase = true };
	private readonly Mock<IRolesCache> _mockRolesCache = new();

	protected override object?[] ConstructorArguments => new object?[]
	{
		_dbContext, _mockRolesCache.Object
	};

	protected override ClaimsPrincipal? Claims => _mockClaims.Object;


	[Test]
	public async Task WithValidParameters_Succeeds()
	{
		// Arrange
		var user = _dataFactory.GenerateUser().Build();
		var request = new Request { Id = user.Id };
		_mockClaims.SetUpClaims(new Claim("Id", $"{123}"));

		// Act
		await _endpoint.HandleAsync(request, default);

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(200);

		_dbContext.Users.Should().ContainSingle(t => t.Id == user.Id && t.IsActive == false);
	}

	[Test]
	public async Task WithInvalidParameters_UserDoesNotExists_Fails()
	{
		// Arrange
		const int userId = 999;
		var request = new Request { Id = userId };
		_mockClaims.SetUpClaims(new Claim("Id", $"{123}"));

		// Act
		await _endpoint.HandleAsync(request, default);

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(404);
	}

	[Test]
	public async Task WithInvalidParameters_UserDeletesItself_Fails()
	{
		// Arrange
		var user = _dataFactory.GenerateUser().Build();
		var request = new Request { Id = user.Id };
		_mockClaims.SetUpClaims(new Claim("Id", $"{user.Id}"));

		// Act
		await _endpoint.HandleAsync(request, default);

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(400);
	}
}