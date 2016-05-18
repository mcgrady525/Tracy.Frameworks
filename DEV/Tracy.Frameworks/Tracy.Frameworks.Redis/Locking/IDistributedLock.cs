using StackExchange.Redis;
namespace Tracy.Frameworks.Redis.Locking
{
    /// <summary>
    /// Distributed lock interface
    /// </summary>
    public interface IDistributedLock
    {
        bool Lock(string key, string value, int lockTimeout, ConnectionMultiplexer connection);
        bool Unlock(string key, string value, ConnectionMultiplexer connection);
    }
}