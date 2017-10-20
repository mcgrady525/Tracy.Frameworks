using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tracy.Frameworks.Configurations;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Redis
{
    /// <summary>
    /// 基于StackExchange.Redis的原生封装
    /// </summary>
    public class RedisWrapper: IRedisWrapper
    {
        #region 连接对象

        private static ConnectionMultiplexer _multiplexer;
        private static readonly ConfigurationOptions Options;
        private static readonly object locker = new object();

        static RedisWrapper()
        {
            var redisConfig = System.Configuration.ConfigurationManager.GetSection("redis") as RedisConfigurationSection;
            Options = ConfigurationOptions.Parse(redisConfig.HostName);
            Options.CommandMap = CommandMap.Create(new HashSet<string>
                            {
                                "INFO",
                                "CONFIG",
                                "PING",
                                "ECHO",
                                "CLIENT"
                            }, false);
        }

        private static ConnectionMultiplexer Conn
        {
            get
            {
                if (_multiplexer == null || !_multiplexer.IsConnected)
                {
                    lock (locker)
                    {
                        if (_multiplexer == null || !_multiplexer.IsConnected)
                        {
                            _multiplexer = ConnectionMultiplexer.Connect(Options);
                            _multiplexer.PreserveAsyncOrder = false;
                        }
                    }
                }

                return _multiplexer;
            }
        }

        private static IDatabase Db
        {
            get { return Conn.GetDatabase(); }
        }

        #endregion

        #region Key处理

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return Db.KeyDelete(key);
        }
        
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsExists(string key)
        {
            return Db.KeyExists(key);
        }
        
        /// <summary>
        /// 给key设置过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool KeyExpire(string key, DateTime? expiry)
        {
            return Db.KeyExpire(key, expiry);
        }
        
        /// <summary>
        /// 返回key的剩余过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeSpan? KeyTimeToLive(string key)
        {
            return Db.KeyTimeToLive(key);
        }

        #endregion

        #region 字符串

        #region Add新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值(string)</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        public bool Add(string key, string value, int seconds = 0)
        {
            return Db.StringSet(key, value, seconds.ToRedisTimeSpan(), When.NotExists);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值(对象)</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        public bool Add<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return Db.StringSet(key, t.ToJson(), seconds.ToRedisTimeSpan(), When.NotExists);
        }
        
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="kvs"></param>
        /// <returns></returns>
        public bool Add(IDictionary<string, string> kvs)
        {
            return Db.StringSet(kvs.ToKeyValuePairArray(), When.NotExists);
        }

        #endregion

        #region Update修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        public bool Update(string key, string value, int seconds = 0)
        {
            return Db.StringSet(key, value, seconds.ToRedisTimeSpan());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值(对象)</param>
        /// <param name="seconds">缓存时间(秒)，默认不过期</param>
        /// <returns></returns>
        public bool Update<T>(string key, T t, int seconds = 0) where T : class, new()
        {
            return Db.StringSet(key, t.ToJson(), seconds.ToRedisTimeSpan());
        }
        
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="kvs"></param>
        /// <returns></returns>
        public bool Update(IDictionary<string, string> kvs)
        {
            return Db.StringSet(kvs.ToKeyValuePairArray());
        }

        #endregion

        #region Get查询

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回string</returns>
        public string Get(string key)
        {
            return Db.StringGet(key);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对象</returns>
        public T Get<T>(string key) where T : class
        {
            return Get(key).FromJson<T>();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public string[] Get(string[] keys)
        {
            return Db.StringGet(keys.ToRedisKeyArray()).ToStringArray();
        }

        /// <summary>
        /// 查询(不存在则新增)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="func"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public string GetOrAdd(string key, Func<string> func, int seconds = 0)
        {
            var data = Get(key);
            if (data.IsNullOrEmpty())
            {
                data = func();
                if (!data.IsNullOrEmpty())
                    Add(key, data, seconds);
            }
            return data;
        }

        /// <summary>
        /// 查询(不存在则新增)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="func"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public T GetOrAdd<T>(string key, Func<T> aquire, int seconds = 0) where T : class, new()
        {
            var data = Get<T>(key);
            if (data.IsNull())
            {
                data = aquire();
                if (data.IsNotNull())
                    Add(key, data, seconds);
            }
            return data;
        }

        #endregion

        #region Increment

        public long Increment(string key, long value = 1)
        {
            return Db.StringIncrement(key, value);
        }

        #endregion

        #region Decrement

        public long Decrement(string key, long value = 1)
        {
            return Db.StringDecrement(key, value);
        }

        #endregion

        #region GetSet

        public string GetSet(string key, string value)
        {
            return Db.StringGetSet(key, value);
        }

        #endregion

        #endregion

        #region 哈希

        #region HashSet

        public bool HashSet(string key, string field, string value)
        {
            return Db.HashSet(key, field, value);
        }

        public void HashSet<T>(string key, T entity) where T : class, new()
        {
            var hashEntry = entity.GetHashEntry(typeof(T).GetEntityProperties());
            Db.HashSet(key, hashEntry);
        }

        public void HashSet<T>(string key, Expression<Func<T>> expression) where T : class, new()
        {
            var hashEntry = RedisExpression<T>.HashEntry(expression);
            Db.HashSet(key, hashEntry);
        }

        #endregion

        #region HashIncrement

        public long HashIncrement(string key, string field, long value = 1)
        {
            return Db.HashIncrement(key, field, value);
        }

        #endregion

        #region HashDecrement

        public long HashDecrement(string key, string field, long value = 1)
        {
            return Db.HashDecrement(key, field, value);
        }

        #endregion

        #region HashExists

        public bool HashExists(string key, string field)
        {
            return Db.HashExists(key, field);
        }

        #endregion

        #region HashDelete

        public bool HashDelete(string key, string field)
        {
            return Db.HashDelete(key, field);
        }

        public long HashDelete(string key, string[] fields)
        {
            return Db.HashDelete(key, fields.ToRedisValueArray());
        }

        #endregion

        #region HashGet

        public string HashGet(string key, string field)
        {
            return Db.HashGet(key, field);
        }

        #endregion

        #region HashGetAll

        public KeyValuePair<string, string>[] HashGetAll(string key)
        {
            return Db.HashGetAll(key).ToHashPairs();
        }

        #endregion

        #region HashKeys

        public string[] HashKeys(string key)
        {
            return Db.HashKeys(key).ToStringArray();
        }

        #endregion

        #region HashValues

        public string[] HashValues(string key)
        {
            return Db.HashValues(key).ToStringArray();
        }

        #endregion

        #region HashLength

        public long HashLength(string key)
        {
            return Db.HashLength(key);
        }

        #endregion

        #region HashScan

        public KeyValuePair<string, string>[] HashScan(string key, string pattern = null, int pageSize = 10, long cursor = 0, int pageOffset = 0)
        {
            return Db.HashScan(key, pattern, pageSize, cursor, pageOffset).ToHashPairs();
        }

        #endregion

        #endregion

        #region 列表

        #region ListLeftPush

        public long ListLeftPush(string key, string value)
        {
            return Db.ListLeftPush(key, value);
        }

        public long ListLeftPush(string key, string[] values)
        {
            return Db.ListLeftPush(key, values.ToRedisValueArray());
        }

        #endregion

        #region ListRightPush

        public long ListRightPush(string key, string value)
        {
            return Db.ListRightPush(key, value);
        }
        public long ListRightPush(string key, string[] values)
        {
            return Db.ListRightPush(key, values.ToRedisValueArray());
        }

        #endregion

        #region ListLeftPop

        public string ListLeftPop(string key)
        {
            return Db.ListLeftPop(key);
        }

        #endregion

        #region ListRightPop

        public string ListRightPop(string key)
        {
            return Db.ListRightPop(key);
        }

        #endregion

        #region ListLength

        public long ListLength(string key)
        {
            return Db.ListLength(key);
        }

        #endregion

        #region ListRange

        public string[] ListRange(string key, long start = 0, long stop = -1)
        {
            return Db.ListRange(key, start, stop).ToStringArray();
        }

        #endregion

        #region ListRemove

        public long ListRemove(string key, string value, long count = 0)
        {
            return Db.ListRemove(key, value, count);
        }

        #endregion

        #region ListTrim

        public void ListTrim(string key, long start, long stop)
        {
            Db.ListTrim(key, start, stop);
        }

        #endregion

        #region ListInsertAfter

        public long ListInsertAfter(string key, string pivot, string value)
        {
            return Db.ListInsertAfter(key, pivot, value);
        }

        #endregion

        #region ListInsertBefore

        public long ListInsertBefore(string key, string pivot, string value)
        {
            return Db.ListInsertBefore(key, pivot, value);
        }

        #endregion

        #region ListGetByIndex

        public string ListGetByIndex(string key, long index)
        {
            return Db.ListGetByIndex(key, index);
        }

        #endregion

        #region ListSetByIndex

        public void ListSetByIndex(string key, long index, string value)
        {
            Db.ListSetByIndex(key, index, value);
        }

        #endregion

        #endregion

        #region 集合

        #region SetAdd

        public bool SetAdd(string key, string value)
        {
            return Db.SetAdd(key, value);
        }

        public long SetAdd(string key, string[] values)
        {
            return Db.SetAdd(key, values.ToRedisValueArray());
        }

        #endregion

        #region SetCombine

        public string[] SetCombine(RedisSetOperation operation, string first, string second)
        {
            return Db.SetCombine(operation.ToSetOperation(), first, second).ToStringArray();
        }

        public string[] SetCombine(RedisSetOperation operation, string[] keys)
        {
            return Db.SetCombine(operation.ToSetOperation(), keys.ToRedisKeyArray()).ToStringArray();
        }

        #endregion

        #region SetCombineAndStore

        public long SetCombineAndStore(RedisSetOperation operation, string desctination, string first, string second)
        {
            return Db.SetCombineAndStore(operation.ToSetOperation(), desctination, first, second);
        }

        public long SetCombineAndStore(RedisSetOperation operation, string desctination, string[] keys)
        {
            return Db.SetCombineAndStore(operation.ToSetOperation(), desctination, keys.ToRedisKeyArray());
        }

        #endregion

        #region SetContains

        public bool SetContains(string key, string value)
        {
            return Db.SetContains(key, value);
        }

        #endregion

        #region SetLength

        public long SetLength(string key)
        {
            return Db.SetLength(key);
        }

        #endregion

        #region SetMembers

        public string[] SetMembers(string key)
        {
            return Db.SetMembers(key).ToStringArray();
        }

        #endregion

        #region SetMove

        public bool SetMove(string source, string desctination, string value)
        {
            return Db.SetMove(source, desctination, value);
        }

        #endregion

        #region SetPop

        public string SetPop(string key)
        {
            return Db.SetPop(key);
        }

        #endregion

        #region SetRandomMember

        public string SetRandomMember(string key)
        {
            return Db.SetRandomMember(key);
        }

        #endregion

        #region SetRandomMembers

        public string[] SetRandomMembers(string key, long count)
        {
            return Db.SetRandomMembers(key, count).ToStringArray();
        }

        #endregion

        #region SetRemove

        public bool SetRemove(string key, string value)
        {
            return Db.SetRemove(key, value);
        }

        public long SetRemove(string key, string[] values)
        {
            return Db.SetRemove(key, values.ToRedisValueArray());
        }

        #endregion

        #region SetScan

        public string[] SetScan(string key, string pattern = null, int pageSize = 0, long cursor = 0, int pageOffset = 0)
        {
            return Db.SetScan(key, pattern, pageSize, cursor, pageOffset).ToArray().ToStringArray();
        }

        #endregion

        #endregion

        #region 有序集合

        #region SortedSetAdd

        public bool SortedSetAdd(string key, string member, double score)
        {
            return Db.SortedSetAdd(key, member, score);
        }

        public long SortedSetAdd(string key, IDictionary<string, double> members)
        {
            return Db.SortedSetAdd(key, members.ToSortedSetEntry());
        }

        #endregion

        #region SortedSetCombineAndStore

        public long SortedSetCombineAndStore(RedisSetOperation operation, string desctination, string first, string second, RedisAggregate aggregate = RedisAggregate.Sum)
        {
            return Db.SortedSetCombineAndStore(operation.ToSetOperation(), desctination, first, second, aggregate.ToAggregate());
        }

        public long SortedSetCombineAndStore(RedisSetOperation operation, string desctination, string[] keys, double[] weights = null, RedisAggregate aggregate = RedisAggregate.Sum)
        {
            return Db.SortedSetCombineAndStore(operation.ToSetOperation(), desctination, keys.ToRedisKeyArray(), weights, aggregate.ToAggregate());
        }

        #endregion

        #region SortedSetDecrement

        public double SortedSetDecrement(string key, string member, double value)
        {
            return Db.SortedSetDecrement(key, member, value);
        }

        #endregion

        #region SortedSetIncrement

        public double SortedSetIncrement(string key, string member, double value)
        {
            return Db.SortedSetIncrement(key, member, value);
        }

        #endregion

        #region SortedSetLength

        public long SortedSetLength(string key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetLength(key, min, max, exclude.ToExclude());
        }

        #endregion

        #region SortedSetLengthByValue

        public long SortedSetLengthByValue(string key, string min, string max, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetLengthByValue(key, min, max, exclude.ToExclude());
        }

        #endregion

        #region SortedSetRangeByRank

        public string[] SortedSetRangeByRank(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending)
        {
            return Db.SortedSetRangeByRank(key, start, stop, order.ToOrder()).ToStringArray();
        }

        #endregion

        #region SortedSetRangeByRankWithScores

        public KeyValuePair<string, double>[] SortedSetRangeByRankWithScores(string key, long start = 0, long stop = -1, RedisOrder order = RedisOrder.Ascending)
        {
            return Db.SortedSetRangeByRankWithScores(key, start, stop, order.ToOrder()).ToSortedPairs();
        }

        #endregion

        #region SortedSetRangeByScore

        public string[] SortedSetRangeByScore(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByScore(key, start, stop, exclude.ToExclude(), order.ToOrder(), skip, take).ToStringArray();
        }

        #endregion

        #region SortedSetRangeByScoreWithScores

        public KeyValuePair<string, double>[] SortedSetRangeByScoreWithScores(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, RedisExclude exclude = RedisExclude.None, RedisOrder order = RedisOrder.Ascending, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByScoreWithScores(key, start, stop, exclude.ToExclude(), order.ToOrder(), skip, take).ToSortedPairs();
        }
        
        #endregion

        #region SortedSetRangeByValue

        public string[] SortedSetRangeByValue(string key, string min = null, string max = null, RedisExclude exclude = RedisExclude.None, long skip = 0, long take = -1)
        {
            return Db.SortedSetRangeByValue(key, min, max, exclude.ToExclude(), skip, take).ToStringArray();
        }
        
        #endregion

        #region SortedSetRank

        public long? SortedSetRank(string key, string member, RedisOrder order = RedisOrder.Ascending)
        {
            return Db.SortedSetRank(key, member, order.ToOrder());
        }

        #endregion

        #region SortedSetRemove

        public bool SortedSetRemove(string key, string member)
        {
            return Db.SortedSetRemove(key, member);
        }

        public long SortedSetRemove(string key, string[] members)
        {
            return Db.SortedSetRemove(key, members.ToRedisValueArray());
        }

        #endregion

        #region SortedSetRemoveRangeByRank

        public long SortedSetRemoveRangeByRank(string key, long start, long stop)
        {
            return Db.SortedSetRemoveRangeByRank(key, start, stop);
        }

        #endregion

        #region SortedSetRemoveRangeByScore

        public long SortedSetRemoveRangeByScore(string key, double start, double stop, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetRemoveRangeByScore(key, start, stop, exclude.ToExclude());
        }

        #endregion

        #region SortedSetRemoveRangeByValue

        public long SortedSetRemoveRangeByValue(string key, string min, string max, RedisExclude exclude = RedisExclude.None)
        {
            return Db.SortedSetRemoveRangeByValue(key, min, max, exclude.ToExclude());
        }

        #endregion

        #region SortedSetScan

        public KeyValuePair<string, double>[] SortedSetScan(string key, string pattern = null, int pageSize = 10, int cursor = 0, int pageOffset = 0)
        {
            return Db.SortedSetScan(key, pattern, pageSize, cursor, pageOffset).ToSortedPairs();
        }

        #endregion

        #region SortedSetScore

        public double? SortedSetScore(string key, string member)
        {
            return Db.SortedSetScore(key, member);
        }

        #endregion

        #endregion
    }
}
