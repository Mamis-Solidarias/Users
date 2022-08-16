using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using MamisSolidarias.Infrastructure.Users.Models;

namespace MamisSolidarias.WebAPI.Users.Utils;

internal static class DataFactory
{
    private static readonly int AvailableServices = Enum.GetNames<Infrastructure.Users.Models.Services>().Length;
    
    private static readonly Faker<Role> RoleGenerator = new Faker<Role>()
            .RuleFor(t=> t.Id, t=> t.Random.Int(0))
            .RuleFor(t=> t.Service, t=> t.PickRandom<Infrastructure.Users.Models.Services>())
            .RuleFor(t=> t.CanRead, t=> t.Random.Bool())
            .RuleFor(t=> t.CanWrite,t=> t.Random.Bool());

    private static readonly Faker<User> UserGenerator = new Faker<User>()
        .RuleFor(t => t.Id, f => f.Random.Int(0))
        .RuleFor(t => t.Name, f => f.Name.FindName())
        .RuleFor(t => t.Password, f => f.Internet.Password())
        .RuleFor(t => t.Email, (f, u) => f.Internet.Email(u.Name))
        .RuleFor(t => t.Phone, f => f.Phone.PhoneNumber("+549##########"))
        .RuleFor(t => t.Salt, f => Convert.ToBase64String(f.Random.Bytes(16)))
        .RuleFor(t => t.Roles,t=> RoleGenerator.Generate(t.Random.Int(1,AvailableServices)))
        .RuleFor(t=> t.IsActive, _=> true);

    public static User GetUser()
    {
        return UserGenerator.Generate();
    }

    public static IEnumerable<User> GetUsers(int n)
    {
        return Enumerable.Range(0, n).Select(_ => GetUser());
    }
}