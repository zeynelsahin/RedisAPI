using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data;

public class RedisPlatformRepo : IPlatformRepo
{
    private readonly IConnectionMultiplexer _redis;
    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    public void CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        var db = _redis.GetDatabase();
        var serialPLatform = JsonSerializer.Serialize(platform);
        db.StringSet(platform.Id, serialPLatform);
        db.SetAdd("PlatformSet", serialPLatform);
    }

    public IEnumerable<Platform?>? GetAllPlatform()
    {
        var db = _redis.GetDatabase();

        var copmleSet = db.SetMembers("PlatformSet");
        if (copmleSet.Length > 0)
        {
            var obj = Array.ConvertAll(copmleSet, val => JsonSerializer.Deserialize<Platform>(val)).ToList();
            return obj;
        }


        return null;
    }

    public Platform? GetPlatformById(string id)
    {
        var db = _redis.GetDatabase();
        var platform = db.StringGet(id);
        if (!string.IsNullOrEmpty(platform))
        {
            return JsonSerializer.Deserialize<Platform>(platform);
        }

        return null;
    }
}