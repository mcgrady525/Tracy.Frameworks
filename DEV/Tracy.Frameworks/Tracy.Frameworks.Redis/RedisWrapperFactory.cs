using System;
using System.Collections.Concurrent;

namespace Tracy.Frameworks.Redis
{
    /// <summary>
    /// 访问Redis的入口
    /// 适用于没有使用依赖注入时使用
    /// </summary>
    public class RedisWrapperFactory
    {
        private static ConcurrentDictionary<string, IRedisWrapper> iRedisDic;

        static RedisWrapperFactory()
        {
            iRedisDic = new ConcurrentDictionary<string, IRedisWrapper>();
        }

        /// <summary>
        /// 获取访问Redis包装器的实例
        /// </summary>
        /// <param name="configName">redis服务器名</param>
        /// <returns></returns>
        public static IRedisWrapper GetInstance(string configName)
        {
            if (string.IsNullOrEmpty(configName))
            {
                throw new ArgumentNullException("configName could not be empty or null!");
            }

            var redisCache = iRedisDic.GetOrAdd(configName, key =>
            {
                lock (string.Intern(key))
                {
                    if (iRedisDic.ContainsKey(key))
                    {
                        return iRedisDic[key];
                    }
                    return new RedisWrapper(key);
                }
            });
            return redisCache;
        }
    }
}
