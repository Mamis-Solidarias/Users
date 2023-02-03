using System;
using Bogus;
using MamisSolidarias.Infrastructure.Users;
using MamisSolidarias.Infrastructure.Users.Models;
using MamisSolidarias.WebAPI.Users.Services;

namespace MamisSolidarias.WebAPI.Users.Utils;

internal class UserBuilder
{
	private static readonly ITextHasher TextHasher = new TextHasher();
	private static readonly int AvailableServices = Enum.GetNames<MamisSolidarias.Utils.Security.Services>().Length;
    
	private static readonly Faker<Role> RoleGenerator = new Faker<Role>()
		.RuleFor(t=> t.Id, t=> t.IndexGlobal + 1)
		.RuleFor(t=> t.Service, t=> t.PickRandom<MamisSolidarias.Utils.Security.Services>())
		.RuleFor(t=> t.CanRead, t=> t.Random.Bool())
		.RuleFor(t=> t.CanWrite,t=> t.Random.Bool());
	
	private static readonly Faker<User> UserGenerator = new Faker<User>()
		.RuleFor(t => t.Id, f => f.IndexGlobal + 1)
		.RuleFor(t => t.Name, f => f.Name.FindName())
		.RuleFor(t => t.Password, f => f.Internet.Password())
		.RuleFor(t => t.Email, (f, u) => f.Internet.Email(u.Name).ToLowerInvariant())
		.RuleFor(t => t.Phone, f => f.Phone.PhoneNumber("+549##########"))
		.RuleFor(t => t.Salt, f => Convert.ToBase64String(f.Random.Bytes(16)))
		.RuleFor(t => t.Roles,t=> RoleGenerator.Generate(t.Random.Int(1,AvailableServices)))
		.RuleFor(t=> t.IsActive, _=> true);

	private readonly User _user = UserGenerator.Generate();
	private readonly UsersDbContext? _db;
	
	public UserBuilder(User user) => _user = user;
	public UserBuilder(UsersDbContext? db = null) => _db = db;
	
	public UserBuilder WithPassword(string password)
	{
		var (salt, hash) = TextHasher.Hash(password);
		_user.Password = hash;
		_user.Salt = Convert.ToBase64String(salt);
		return this;
	}
	
	public User Build()
	{
		_db?.Add(_user);
		_db?.SaveChanges();
		_db?.ChangeTracker.Clear();
		return _user;
	}
	public UserBuilder IsActive(bool active)
	{
		_user.IsActive = active;
		return this;
	}
}