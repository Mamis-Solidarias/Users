using System.Security.Claims;
using EntityFramework.Exceptions.Sqlite;
using FastEndpoints;
using MamisSolidarias.Infrastructure.Users;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using EndpointFactory = MamisSolidarias.Utils.Test.EndpointFactory;

namespace MamisSolidarias.WebAPI.Users.Utils;

internal abstract class EndpointTest<TEndpoint>
	where TEndpoint : class, IEndpoint
{
	protected DataFactory _dataFactory = null!;
	protected UsersDbContext _dbContext = null!;
	protected TEndpoint _endpoint = null!;
	protected abstract object?[] ConstructorArguments { get; }
	protected virtual ClaimsPrincipal? Claims { get; } = null;

	[SetUp]
	public virtual void Setup()
	{
		var connection = new SqliteConnection("DataSource=:memory:");
		connection.Open();
		var options = new DbContextOptionsBuilder<UsersDbContext>()
			.UseSqlite(connection)
			.UseExceptionProcessor()
			.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
			.Options;

		_dbContext = new UsersDbContext(options);
		_dbContext.Database.EnsureCreated();

		_dataFactory = new DataFactory(_dbContext);

		var builder = EndpointFactory
			.CreateEndpoint<TEndpoint>(
			ConstructorArguments
		);
		
		if (Claims is not null)
		{
			builder.WithClaims(Claims);
		}
		
		_endpoint = builder.Build();
	}

	[TearDown]
	public virtual void Teardown()
	{
		_dbContext.Database.EnsureDeleted();
		_dbContext.Dispose();
	}
}