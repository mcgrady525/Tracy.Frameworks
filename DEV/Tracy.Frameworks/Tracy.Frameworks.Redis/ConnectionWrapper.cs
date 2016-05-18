using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Tracy.Frameworks.RedisConfig;

namespace Tracy.Frameworks.Redis
{
    public static class ConnectionWrapper
    {
        public static int ConnectionTimeout = 1000 * 10;
        public static int AsyncTimeOut = 1000 * 50;
        public static bool AllowAdmin = true;
        public static int ConnectRetry = 4;
        public static bool AbortConnect = false;
        public static int KeepAlive = 180;
        private static DebugLogger debugLogger = new DebugLogger();
        public static Task<ConnectionMultiplexer> GetConnetionMultiplexerAsync(string redisConfigName, System.IO.TextWriter logger = null)
        {
            return ConnectionMultiplexer.ConnectAsync(GetConnectionString(redisConfigName), logger);
        }
        public static string GetConnectionString(string redisConfigName)
        {
            var hosts = GetRedisHostConfig(redisConfigName);
            var connectionTimeOutConfig = string.Format("connectTimeout={0}", ConnectionTimeout);
            var asyncTimeOutConfig = string.Format("syncTimeout={0}", AsyncTimeOut);
            var allAdminConfig = string.Format("allowAdmin={0}", AllowAdmin);
            var connectRetryConfig = string.Format("connectRetry={0}", ConnectRetry);
            var abortConnect = string.Format("abortConnect={0}", AbortConnect);
            var keepAlive = string.Format("keepAlive={0}", KeepAlive);
            hosts.Add(connectionTimeOutConfig);
            hosts.Add(asyncTimeOutConfig);
            hosts.Add(allAdminConfig);
            hosts.Add(connectRetryConfig);
            hosts.Add(abortConnect);
            return string.Join(",", hosts);
        }
        private static List<string> GetRedisHostConfig(string redisConfigName)
        {
            if (string.IsNullOrEmpty(redisConfigName))
            {
                throw new ArgumentNullException("redisConfigName");
            }
            RedisConfigSection redisConfig = null;
            HostGroup group = null;
            redisConfig = (RedisConfigSection)ConfigurationManager.GetSection("redisConfig");

            foreach (HostGroup item in redisConfig.HostGroups)
            {
                if (item.Name.Equals(redisConfigName, StringComparison.OrdinalIgnoreCase))
                {
                    group = item;
                    break;
                }
            }
            if (group == null)
            {
                throw new Exception(string.Format("Redis配置错误，根據服務器配置組名:{0}找不到Redis服務器组", redisConfigName));

            }
            if (group.IsSentinel == true)
            {
                throw new Exception("StackExchange.Redis客戶端不支持連接Sentinel請指定Master和Slave的服務器IP和端口,IConnectionMultiplexer會管理和監聽Failover的行為.");
            }
            var redisHosts = new List<string>();
            if (group.Hosts == null || group.Hosts.Count == 0)
            {
                throw new Exception("Redis配置错误，组下面没有配置节点");

            }
            foreach (HostConfig host in group.Hosts)
            {
                var hostIPPort = string.Format("{0}:{1}", host.IP, host.Port);
                redisHosts.Add(hostIPPort);
            }
            return redisHosts;
        }
        public static ConnectionMultiplexer GetConnetionMultiplexer(string redisConfigName, System.IO.TextWriter logger = null)
        {
            if (logger == null)
            {
                logger = debugLogger;
            }
            return ConnectionMultiplexer.Connect(GetConnectionString(redisConfigName), logger);
        }
    }
}
