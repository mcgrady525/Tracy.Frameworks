using System.Net;

namespace Tracy.Frameworks.Redis
{
    /// <summary>
    /// Redis客户端信息
    /// </summary>
    public class RedisClientInfo
    {
        public RedisClientInfo() { }


        public EndPoint Address { get; set; }

        public int AgeSeconds { get; set; }

        /// <summary>
        /// Normal = 0,
        /// Slave = 1,
        /// PubSub = 2
        /// </summary>
        public int ClientType { get; set; }

        public int Database { get; set; }

        /// <summary>
        /// None = 0,
        /// SlaveMonitor = 1,
        /// Slave = 2,
        /// Master = 4,
        /// Transaction = 8,
        /// Blocked = 16,
        /// TransactionDoomed = 32,
        /// Closing = 64,
        /// Unblocked = 128,
        /// CloseASAP = 256
        /// </summary>
        public int Flags { get; set; }

        public string FlagsRaw { get; set; }

        public string Host { get; set; }

        public long Id { get; set; }

        public int IdleSeconds { get; set; }

        public string LastCommand { get; set; }

        public string Name { get; set; }

        public int PatternSubscriptionCount { get; set; }

        public int Port { get; set; }

        public string Raw { get; set; }

        public int SubscriptionCount { get; set; }

        public int TransactionCommandLength { get; set; }
    }
}
