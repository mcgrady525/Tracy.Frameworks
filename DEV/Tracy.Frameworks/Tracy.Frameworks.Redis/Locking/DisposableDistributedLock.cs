using StackExchange.Redis;
using System;

namespace Tracy.Frameworks.Redis.Locking
{
    /// <summary>
    /// distributed lock class that follows the Resource Allocation Is Initialization pattern
    /// </summary>
    public class DisposableDistributedLock : IDisposable
    {
        private readonly IDistributedLock myLock;
        private readonly bool isLocked;
        private readonly ConnectionMultiplexer myConnection;
        private readonly string globalLockKey;
        private readonly string lockValue;

        /// <summary>
        /// Lock
        /// </summary>
        /// <param name="client"></param>
        /// <param name="globalLockKey"></param>
        /// <param name="acquisitionTimeout">in seconds</param>
        /// <param name="lockTimeout">in seconds</param>
        public DisposableDistributedLock(ConnectionMultiplexer connection, string globalLockKey, int lockTimeout)
        {
            myLock = new DistributedLock();
            myConnection = connection;
            this.globalLockKey = globalLockKey;
            this.lockValue = Guid.NewGuid().ToString();
            isLocked = myLock.Lock(globalLockKey, this.lockValue, lockTimeout, myConnection);
        }
        
        public bool IsLocked
        {
            get { return isLocked; }
        }
 

        /// <summary>
        /// unlock
        /// </summary>
        public void Dispose()
        {
            myLock.Unlock(globalLockKey, lockValue, myConnection);
        }
    }
}
