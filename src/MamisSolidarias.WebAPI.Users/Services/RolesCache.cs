namespace MamisSolidarias.WebAPI.Users.Services;
using StackExchange.Redis;

internal class RolesCache : IRolesCache
{
	private readonly ConnectionMultiplexer _redis;
	public RolesCache(ConnectionMultiplexer redis)
	{
		_redis = redis;
	}
	
	
	public async Task SetPermissions(int userId, IEnumerable<string> permissions)
	{
		var db = _redis.GetDatabase();
		await ClearPermissions(userId);
		await db.ListRightPushAsync($"roles-{userId}", permissions.Select(t => (RedisValue)t).ToArray());
	}
	
	public async Task ClearPermissions(int userId)
	{
		var db = _redis.GetDatabase();
		await db.KeyDeleteAsync($"roles-{userId}");
	}
}

internal interface IRolesCache
{
	Task SetPermissions(int userId, IEnumerable<string> permissions);
	Task ClearPermissions(int userId);
}