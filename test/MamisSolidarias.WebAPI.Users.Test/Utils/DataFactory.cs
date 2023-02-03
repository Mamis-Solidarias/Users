using System.Collections.Generic;
using System.Linq;
using MamisSolidarias.Infrastructure.Users;

namespace MamisSolidarias.WebAPI.Users.Utils;

internal class DataFactory
{
	private readonly UsersDbContext _dbContext;
	public DataFactory(UsersDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public static UserBuilder GetUser()
    {
	    return new UserBuilder();
    }

    public UserBuilder GenerateUser()
    {
	    return new UserBuilder(_dbContext);
    }

    public static IEnumerable<UserBuilder> GetUsers(int n)
    {
        return Enumerable.Range(0, n).Select(_ => GetUser());
    }
}