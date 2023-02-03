using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Endpoints.Users.Id.Roles.PUT;
using MamisSolidarias.WebAPI.Users.Services;
using MamisSolidarias.WebAPI.Users.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace MamisSolidarias.WebAPI.Users.Endpoints;

internal class Users_Id_Roles_Put : EndpointTest<Endpoint>
{
	private readonly Mock<ClaimsPrincipal> _mockClaims = new() { CallBase = true };
	private readonly Mock<IRolesCache> _rolesCache = new();

	protected override object?[] ConstructorArguments => new object?[]
	{
		_dbContext, _rolesCache.Object
	};

	public override void Teardown()
	{
		base.Teardown();
		_mockClaims.Reset();
	}


	[Test]
	public async Task WithValidParameters_Succeeds()
	{
		// Arrange
		var user = _dataFactory.GenerateUser().Build();

		var request = new Request
		{
			Id = user.Id,
			Roles = new[] { new RoleRequest("Users", true, true) }
		};

		// Act
		await _endpoint.HandleAsync(request, default);
		var response = _endpoint.Response;

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(200);
		response.Should().NotBeNull();

		var result = await _dbContext.Users.Include(t => t.Roles)
			.SingleAsync(t => t.Id == user.Id);

		result.Roles.Should().BeEquivalentTo(new Role[]
		{
			new()
			{
				Service = MamisSolidarias.Utils.Security.Services.Users,
				CanRead = true,
				CanWrite = true
			}
		}, options => options.Excluding(t => t.Id));
	}

	[Test]
	public async Task WithValidParameters_RemoveRoles_Succeeds()
	{
		// Arrange
		var user = _dataFactory.GenerateUser().Build();

		var request = new Request
		{
			Id = user.Id,
			Roles = new List<RoleRequest>()
		};

		// Act
		await _endpoint.HandleAsync(request, default);
		var response = _endpoint.Response;

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(200);
		response.Should().NotBeNull();
		response.Roles.Count().Should().Be(0);

		var result = _dbContext.Users.Include(t => t.Roles)
			.SingleAsync(t => t.Id == user.Id);

		result.Result.Roles.Should().BeEmpty();
	}

	[Test]
	public async Task WithInvalidParameters_UserDoesNotExists_Fails()
	{
		// Arrange
		const int userId = 123;

		var request = new Request
		{
			Id = userId,
			Roles = ArraySegment<RoleRequest>.Empty
		};

		// Act
		await _endpoint.HandleAsync(request, default);

		// Assert
		_endpoint.HttpContext.Response.StatusCode.Should().Be(404);
	}
}