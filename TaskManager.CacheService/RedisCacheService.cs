using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Windows.Markup;
namespace TaskManager.CacheService;

public class RedisCacheService : IRedisCacheService
{
    private readonly IConfiguration _configuration;
    private readonly int _expireInSec;
    private readonly Lazy<ConnectionMultiplexer> _lazyConnectionMultiplexer;



    private ConnectionMultiplexer Connection => _lazyConnectionMultiplexer.Value;

    public RedisCacheService(IConfiguration configuration)
    {
        this._expireInSec = Convert.ToInt32(configuration["Cache:ExpireInSec"]);
        bool ssl = Convert.ToBoolean(configuration["Cache:Ssl"] ?? "false");
        var options = new ConfigurationOptions
        {
            EndPoints = { { configuration["Cache:ConnectionString"]!.ToString(), Convert.ToInt32(configuration["Cache:Port"]) } },
            User = configuration["Cache:User"]!.ToString(),
            Password = configuration["Cache:Password"]!.ToString(),
            Ssl = ssl,
            AbortOnConnectFail = false,
            ConnectRetry = 3
        };
        _lazyConnectionMultiplexer = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
    }

    public bool SetString(string key, string value, int expireinSec = 0)
    {
        try
        {
            var db = Connection.GetDatabase();
            TimeSpan expire = TimeSpan.FromSeconds(expireinSec == 0 ? _expireInSec : expireinSec);
            db.StringSet(key, value, expire);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public string? GetString(string key)
    {
        try
        {
            var db = Connection.GetDatabase();
            RedisValue? result = db.StringGet(key);
            return result?.ToString();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public void RemoveKey(string key)
    {
        var db = Connection.GetDatabase();
        db.KeyDelete(key);
    }
}
