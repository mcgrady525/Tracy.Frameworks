using StackExchange.Redis;
using System;
using System.Diagnostics;

namespace Tracy.Frameworks.Redis.Locking
{
    public class DistributedLock : IDistributedLock
    {
        public const int LOCK_NOT_ACQUIRED = 0;
        public const int LOCK_ACQUIRED = 1;
        public const int LOCK_RECOVERED = 2;

        /// <summary>
        /// acquire distributed, non-reentrant lock on key
        /// </summary>
        /// <param name="key">global key for this lock</param>
        /// <param name="acquisitionTimeout">timeout for acquiring lock</param>
        /// <param name="lockTimeout">timeout for lock, in seconds (stored as value against lock key) </param>
        /// <param name="client"></param>
        /// <param name="lockExpire"></param>
        public virtual bool Lock(string key,string value, int lockTimeout, ConnectionMultiplexer connection)
        {
           var   isLocked = connection.GetDatabase().LockTake(key, value, TimeSpan.FromSeconds(lockTimeout));
           return isLocked;
        }


        /// <summary>
        /// unlock key
        /// </summary>
        public virtual bool Unlock(string key,string value, ConnectionMultiplexer connection)
        {
            var isReleased = connection.GetDatabase().LockRelease(key, value, CommandFlags.None);
            return isReleased;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        private static long CalculateLockExpire(TimeSpan ts, int timeout)
        {
            return (long)(ts.TotalSeconds + timeout + 1.5);
        }

    }
}