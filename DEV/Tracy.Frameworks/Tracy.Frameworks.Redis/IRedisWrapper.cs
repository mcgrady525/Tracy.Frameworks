using StackExchange.Redis;
using System;
using System.Collections.Generic;
using Tracy.Frameworks.Redis.Locking;

namespace Tracy.Frameworks.Redis
{
    /// <summary>
    /// 访问Redis的API
    /// </summary>
    public interface IRedisWrapper
    {
        /// <summary>
        /// string类型通用保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expireTime"></param>
        /// <param name="when"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool BaseSet(string key, RedisValue data, TimeSpan? expireTime = null, When when = When.Always, CommandFlags flag = CommandFlags.None);

        /// <summary>
        /// string类型保存方法，value如果是自定义类型则序列化成二进制保存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expireTime"></param>
        /// <param name="when"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool Set<T>(string key, T data, TimeSpan? expireTime = null, When when = When.Always, CommandFlags flag = CommandFlags.None);
        
        /// <summary>
        /// string类型通用查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        RedisValue BaseGet(string key);

        /// <summary>
        /// string类型查询，value如果是自定义类型则反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// string类型查询，不反序列化直接返回对象的json字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetJson(string key);
        
        /// <summary>
        /// hash类型保存，value如果是自定义类型则序列化为二进制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="filedKey"></param>
        /// <param name="data"></param>
        /// <param name="when"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool HashSet<T>(string key, string filedKey, T data, When when = When.Always, CommandFlags flag = CommandFlags.None);

        /// <summary>
        /// hash类型批量保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entrys"></param>
        /// <param name="flag"></param>
        void HashSet(string key, Dictionary<string, object> entrys, CommandFlags flag = CommandFlags.None);

        /// <summary>
        /// hash类型查询，value如果是自定义类型则反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        T HashGet<T>(string key, string fieldKey);

        /// <summary>
        /// hash类型批量查询
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldKeyTypes"></param>
        /// <returns></returns>
        List<object> HashGet(string key, Dictionary<string, Type> fieldKeyTypes);

        /// <summary>
        /// hash类型删除key-field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filedKey"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool HashRemove(string key, string fieldKey, CommandFlags flag = CommandFlags.None);

        /// <summary>
        /// hash类型批量删除key-field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldKeys"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        bool HashRemove(string key, List<string> fieldKeys, CommandFlags flag = CommandFlags.None);
        
        /// <summary>
        /// 删除所有指定的key
        /// </summary>
        /// <param name="keys"></param>
        void RemoveAll(List<string> keys);

        /// <summary>
        /// 刪除Key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 根據Pattern刪除Key
        /// </summary>
        /// <param name="patterns"></param>
        void RemoveByPatterns(List<string> patterns);

        /// <summary>
        /// 根據Pattern刪除Key
        /// </summary>
        /// <param name="pattern"></param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 搜索Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        List<string> SearchKeys(string key);
        
        /// <summary>
        /// 判斷Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);

        /// <summary>
        /// 設置Key的過期時間
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        bool ExpireKeyAt(string key, TimeSpan? expireTime);

        /// <summary>
        /// 設置Key的過期時間
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        bool ExpireKeyAt(string key, DateTime? expireTime);

        /// <summary>
        /// 獲取Subscriber订阅
        /// </summary>
        /// <returns></returns>
        ISubscriber GetSubscriber();

        /// <summary>
        /// 获取redis客户端列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<RedisClientInfo> GetClientList();

        /// <summary>
        /// 获取指定channel的订阅者数量
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        long GetSubscriberCount(string channel);

        /// <summary>
        /// 获取指定key的剩余过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TimeSpan GetTimeToLive(string key);

        /// <summary>
        /// 獲取一個可以使用USING代碼塊的分佈式鎖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockedSeconds">秒</param>
        /// <returns></returns>
        DisposableDistributedLock GetLock(string key, int lockedSeconds);

        /// <summary>
        /// 创建一个事务
        /// </summary>
        /// <returns></returns>
        ITransaction CreateTransaction();

    }
}
