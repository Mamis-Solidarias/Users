using System.Collections.Generic;
using System.Linq;
using Bogus;
using MamisSolidarias.Infrastructure.Users.Models;

namespace MamisSolidarias.HttpClient.Users.Utils;

internal static  class DataFactory
{
    private static readonly Faker<User> UserGenerator = new Faker<User>()
        .RuleFor(t => t.Id, f => f.Random.Int())
        .RuleFor(t => t.Name, f => f.Name.FindName())
        .RuleFor(t => t.Password, f => f.Internet.Password())
        .RuleFor(t => t.Email, (f, u) => f.Internet.Email(u.Name))
        .RuleFor(t => t.Phone, f => f.Phone.PhoneNumber("+549##########"))
        .RuleFor(t => t.Salt, f => f.Random.String(10));
    
    public static User GetUser()
    {
        return UserGenerator.Generate();
    }

    public static IEnumerable<User> GetUsers(int n)
    {
        return Enumerable.Range(0, n).Select(_ => GetUser());
    }
}