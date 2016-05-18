using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Tracy.Frameworks.Redis
{
    public static class ConnectionMultiplexerFactory
    {
        private static ConcurrentDictionary<string, ConnectionMultiplexer> connectionDic = new ConcurrentDictionary<string, ConnectionMultiplexer>();

        static ConnectionMultiplexerFactory()
        {
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
        }

        static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            if (connectionDic != null)
            {
                connectionDic.Keys.ToList().ForEach(key => {
                    ConnectionMultiplexer c = null;
                    if (connectionDic.TryRemove(key, out c))
                    {
                        c.Dispose();
                        c = null;
                    }
                });
                connectionDic = null;
            }
        }

        public static ConnectionMultiplexer GetConnection(string configName)
        {
            var connection = connectionDic.GetOrAdd(configName, (config) =>
            {
                var lockObject = string.Intern(configName);
                lock (lockObject)
                {
                    if (connectionDic.ContainsKey(configName))
                    {
                        return connectionDic[configName];
                    }
                    return ConnectionWrapper.GetConnetionMultiplexer(config);
                }
            });
            return connection;
        }
    }
}
