using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tracy.Frameworks.Redis.Locking;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Redis
{
    public class RedisWrapper : IRedisWrapper, IDisposable
    {
        private ConnectionMultiplexer connection;
        private object connectionLock = new object();
        private bool isConnectionInit = false;
        private bool isDisposing = false;
        private static ISerializer serializer;
        public string configName;
        public static string DefaultConfigName;
        static RedisWrapper()
        {
            serializer = new JsonNetSerializer();
        }

        public RedisWrapper()
            : this(DefaultConfigName)
        {

        }

        public RedisWrapper(string configName)
        {
            this.configName = configName;
        }

        /// <summary>
        /// 获取访问redis数据库连接
        /// </summary>
        public ConnectionMultiplexer Connection
        {
            get
            {
                if (isConnectionInit == false)
                {
                    //必須使用懶惰初始化器加載對象,確保對象唯一,這在IIS回收切換過程中很重要
                    LazyInitializer.EnsureInitialized<ConnectionMultiplexer>(ref connection, ref isConnectionInit, ref connectionLock, () =>
                    {
                        return ConnectionWrapper.GetConnetionMultiplexer(configName);
                    });
                }
                return connection;
            }
        }

        /// <summary>
        /// string类型通用保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expireTime"></param>
        /// <param name="when"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool BaseSet(string key, RedisValue data, TimeSpan? expireTime = null, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            var database = Connection.GetDatabase();
            return database.StringSet(key, data, expireTime, when, flag);
        }

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
        public bool Set<T>(string key, T data, TimeSpan? expireTime = null, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            var database = Connection.GetDatabase();
            RedisValue v = ChangeToRedisValue(data);
            return database.StringSet(key, v, expireTime, when, flag);
        }

        /// <summary>
        /// string类型通用查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue BaseGet(string key)
        {
            var database = Connection.GetDatabase();
            return database.StringGet(key);
        }

        /// <summary>
        /// string类型查询，value如果是自定义类型则反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            RedisValue v = Connection.GetDatabase().StringGet(key);
            if (IsBaseValueType<T>())
            {
                return (T)ChangeRedisTypeToBaseType<T>(v);

            }
            if (v.HasValue)
            {
                byte[] data = v;

                //先解压，再反序列化
                var jsonStr = UnCompress(data);
                return jsonStr.FromJson<T>();

            }
            return default(T);
        }

        /// <summary>
        /// string类型查询，不反序列化直接返回对象的json字符串
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetJson(string key)
        {
            RedisValue v = Connection.GetDatabase().StringGet(key);
            if (v.HasValue)
            {
                byte[] data = v;
                using (System.IO.MemoryStream stream = new System.IO.MemoryStream(data))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            return string.Empty;
        }

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
        public bool HashSet<T>(string key, string filedKey, T data, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            var database = Connection.GetDatabase();
            RedisValue value = ChangeToRedisValue(data);
            return database.HashSet(key, filedKey, value, when, flag);
        }

        /// <summary>
        /// hash类型批量保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entrys"></param>
        /// <param name="flag"></param>
        public void HashSet(string key, Dictionary<string, object> entrys, CommandFlags flag = CommandFlags.None)
        {
            var database = Connection.GetDatabase();
            List<HashEntry> entries = new List<HashEntry>();
            foreach (var e in entrys)
            {
                RedisValue v = ChangeToRedisValue(e.Value);
                entries.Add(new HashEntry(e.Key, ChangeToRedisValue(e.Value)));
            }
            database.HashSet(key, entries.ToArray(), flag);
        }

        /// <summary>
        /// hash类型查询，value如果是自定义类型则反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="fieldKey"></param>
        /// <returns></returns>
        public T HashGet<T>(string key, string fieldKey)
        {
            var database = Connection.GetDatabase();

            RedisValue v = database.HashGet(key, fieldKey); ;
            if (IsBaseValueType<T>())
            {
                return (T)ChangeRedisTypeToBaseType<T>(v);

            }
            if (v.HasValue)
            {
                byte[] data = v;

                //先解压，再反序列化
                var jsonStr = UnCompress(data);
                return jsonStr.FromJson<T>();
            }
            return default(T);
        }

        /// <summary>
        /// hash类型批量查询
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldKeyTypes"></param>
        /// <returns></returns>
        public List<object> HashGet(string key, Dictionary<string, Type> fieldKeyTypes)
        {
            var database = Connection.GetDatabase();
            List<object> result = new List<object>();
            if (fieldKeyTypes != null && fieldKeyTypes.Any())
            {
                RedisValue[] fields = fieldKeyTypes.Keys.Select(i => (RedisValue)(i)).ToArray();
                RedisValue[] values = database.HashGet(key, fields);
                if (values != null)
                {
                    var types = fieldKeyTypes.Values.ToArray();
                    for (int i = 0; i < values.Length; i++)
                    {
                        var v = values[i];
                        var t = types[i];
                        if (IsBaseValueType(t))
                        {
                            result.Add(ChangeRedisTypeToBaseType(v, t));

                        }
                        if (v.HasValue)
                        {
                            byte[] data = v;

                            //先解压，再反序列化
                            var jsonStr = UnCompress(data);
                            result.Add(jsonStr.FromJson(t));

                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// hash类型删除key-field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filedKey"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool HashRemove(string key, string fieldKey, CommandFlags flag = CommandFlags.None)
        {
            var database = Connection.GetDatabase();
            return database.HashDelete(key, fieldKey, flag);
        }

        /// <summary>
        /// hash类型批量删除key-field
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldKeys"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool HashRemove(string key, List<string> fieldKeys, CommandFlags flag = CommandFlags.None)
        {
            if (fieldKeys != null && fieldKeys.Any())
            {
                var database = Connection.GetDatabase();

                return database.HashDelete(key, fieldKeys.Select(i => (RedisValue)i).ToArray(), flag) > 0;
            }
            return false;
        }

        /// <summary>
        /// 删除所有指定的key
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveAll(List<string> keys)
        {
            var database = Connection.GetDatabase();
            if (keys != null && keys.Any())
            {
                var items = keys.Select(i => (RedisKey)i).ToArray();
                database.KeyDelete(items, CommandFlags.FireAndForget);
            }
        }

        /// <summary>
        /// 刪除Key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            var database = Connection.GetDatabase();
            database.KeyDelete(key, CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 根據Pattern刪除Key
        /// </summary>
        /// <param name="patterns"></param>
        public void RemoveByPatterns(List<string> patterns)
        {
            List<string> keys = new List<string>();
            foreach (var p in patterns)
            {
                var tempKeys = SearchKeys(p);
                if (tempKeys != null && tempKeys.Any())
                {
                    keys.AddRange(tempKeys);
                }
            }
            Connection.GetDatabase().KeyDelete(keys.Select(i => (RedisKey)i).ToArray(), CommandFlags.FireAndForget);
        }

        /// <summary>
        /// 根據Pattern刪除Key
        /// </summary>
        /// <param name="pattern"></param>
        public void RemoveByPattern(string pattern)
        {
            RemoveByPatterns(new List<string> { pattern });
        }

        /// <summary>
        /// 搜索Key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> SearchKeys(string key)
        {
            List<string> keys = new List<string>();
            foreach (var e in Connection.GetEndPoints())
            {
                var items = Connection.GetServer(e).Keys(pattern: key, pageSize: 9999999);
                if (items != null)
                {
                    keys.AddRange(items.Select(i => i.ToString()));
                }
            }
            return keys.Distinct().ToList();
        }

        /// <summary>
        /// 判斷Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return Connection.GetDatabase().KeyExists(key, CommandFlags.None);
        }

        /// <summary>
        /// 設置Key的過期時間
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public bool ExpireKeyAt(string key, TimeSpan? expireTime)
        {
            return Connection.GetDatabase().KeyExpire(key, expireTime);
        }

        /// <summary>
        /// 設置Key的過期時間
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expireTime"></param>
        /// <returns></returns>
        public bool ExpireKeyAt(string key, DateTime? expireTime)
        {
            expireTime = expireTime == null ? expireTime : (expireTime.Value.Kind == DateTimeKind.Unspecified ? expireTime.Value.ToLocalTime() : expireTime.Value);
            return Connection.GetDatabase().KeyExpire(key, expireTime);
        }

        /// <summary>
        /// 獲取Subscriber订阅
        /// </summary>
        /// <returns></returns>
        public ISubscriber GetSubscriber()
        {
            ISubscriber sub = Connection.GetSubscriber();
            return sub;
        }

        /// <summary>
        /// 获取redis客户端列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RedisClientInfo> GetClientList()
        {
            var result = new List<RedisClientInfo>();

            var endPointList = Connection.GetEndPoints();
            foreach (var endPoint in endPointList)
            {
                var server = Connection.GetServer(endPoint);
                var seClients = server.ClientList();
                if (seClients != null && seClients.Length > 0)
                {
                    foreach (var c in seClients)
                    {
                        result.Add(new RedisClientInfo
                        {
                            Address = c.Address,
                            AgeSeconds = c.AgeSeconds,
                            ClientType = (int)c.ClientType,
                            Database = c.Database,
                            Flags = (int)c.Flags,
                            FlagsRaw = c.FlagsRaw,
                            Host = c.Host,
                            Id = c.Id,
                            IdleSeconds = c.IdleSeconds,
                            LastCommand = c.LastCommand,
                            Name = c.Name,
                            PatternSubscriptionCount = c.PatternSubscriptionCount,
                            Port = c.Port,
                            Raw = c.Raw,
                            SubscriptionCount = c.SubscriptionCount,
                            TransactionCommandLength = c.TransactionCommandLength
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定channel的订阅者数量
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public long GetSubscriberCount(string channel)
        {
            long result = 0;
            var endPointList = Connection.GetEndPoints();
            foreach (var endPoint in endPointList)
            {
                var server = Connection.GetServer(endPoint);
                var count = server.SubscriptionSubscriberCount(channel);
                result += count;
            }

            return result;
        }

        /// <summary>
        /// 获取指定key的剩余过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeSpan GetTimeToLive(string key)
        {
            var database = this.Connection.GetDatabase();
            var result = database.KeyTimeToLive(key);

            return result ?? TimeSpan.FromMinutes(0);
        }

        /// <summary>
        /// 獲取一個可以使用USING代碼塊的分佈式鎖
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockedSeconds">秒</param>
        /// <returns></returns>
        public DisposableDistributedLock GetLock(string key, int lockedSeconds)
        {
            return new DisposableDistributedLock(this.Connection, key, lockedSeconds);
        }

        /// <summary>
        /// 创建一个事务
        /// </summary>
        /// <returns></returns>
        public ITransaction CreateTransaction()
        {
            var database = this.Connection.GetDatabase();
            return database.CreateTransaction();
        }

        #region Private method
        /// <summary>
        /// 基础类型转换成redis类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static RedisValue ChangeBaseTypeToRedisValue<T>(T data)
        {
            RedisValue v = RedisValue.Null;
            if (data == null)
            {
                return RedisValue.Null;
            }
            var t = data.GetType();
            if (t == typeof(string))
            {
                v = (string)(object)data;
                return v;
            }
            if (t == typeof(byte))
            {
                v = (byte)(object)data;
                return v;
            }
            if (t == typeof(sbyte))
            {
                v = (sbyte)(object)data;
                return v;
            }
            if (t == typeof(sbyte))
            {
                v = (sbyte?)(object)data;
                return v;
            }
            if (t == typeof(byte))
            {
                v = (byte?)(object)data;
                return v;
            }
            if (t == typeof(Int16))
            {
                v = (int)(object)data;
                return v;
            }
            if (t == typeof(Int32))
            {
                v = (int)(object)data;
                return v;
            }
            if (t == typeof(Int64))
            {
                v = (long)(object)data;
                return v;
            }
            if (t == typeof(decimal))
            {
                v = (double)(decimal)(object)data;
                return v;
            }
            if (t == typeof(double))
            {
                v = (double)(object)data;
                return v;
            }
            if (t == typeof(float))
            {
                v = (float)(object)data;
                return v;
            }
            return v;
        }

        /// <summary>
        /// redis类型转换成基础类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static object ChangeRedisTypeToBaseType<T>(RedisValue data)
        {
            if (data == RedisValue.Null)
            {
                return null;
            }
            var t = typeof(T);

            if (t == typeof(string))
            {
                return (string)data;

            }
            if (t == typeof(byte))
            {
                return (byte)data;
            }
            if (t == typeof(sbyte))
            {
                return (sbyte)data;

            }
            if (t == typeof(byte?))
            {
                return (byte)data;

            }
            if (t == typeof(sbyte?))
            {
                return (sbyte)data;

            }
            if (t == typeof(Int16))
            {
                return (Int16)data;

            }
            if (t == typeof(Int16?))
            {
                return (Int16)data;

            }
            if (t == typeof(Int32))
            {
                return (Int32)data;

            }
            if (t == typeof(Int32?))
            {
                return (Int32)data;

            }
            if (t == typeof(Int64))
            {
                return (Int64)data;

            }
            if (t == typeof(Int64?))
            {
                return (Int64)data;

            }
            if (t == typeof(decimal))
            {
                return (decimal)(double)data;

            }
            if (t == typeof(double))
            {
                return (double)data;

            }
            if (t == typeof(double?))
            {
                return (double)data;

            }
            if (t == typeof(float))
            {
                return (float)data;

            }
            if (t == typeof(float?))
            {
                return (float)data;

            }
            return default(T);

        }

        /// <summary>
        /// redis类型转换成基础类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static object ChangeRedisTypeToBaseType(RedisValue data, Type t)
        {
            if (data == RedisValue.Null)
            {
                return null;
            }

            if (t == typeof(string))
            {

                return (string)data;

            }
            if (t == typeof(byte))
            {
                return (byte)data;
            }
            if (t == typeof(sbyte))
            {
                return (sbyte)data;

            }
            if (t == typeof(byte?))
            {
                return (byte)data;

            }
            if (t == typeof(sbyte?))
            {
                return (sbyte)data;

            }
            if (t == typeof(Int16))
            {
                return (Int16)data;

            }
            if (t == typeof(Int16?))
            {
                return (Int16)data;

            }
            if (t == typeof(Int32))
            {
                return (Int32)data;

            }
            if (t == typeof(Int32?))
            {
                return (Int32)data;

            }
            if (t == typeof(Int64))
            {
                return (Int64)data;

            }
            if (t == typeof(Int64?))
            {
                return (Int64)data;

            }
            if (t == typeof(decimal))
            {
                return (decimal)(double)data;

            }
            if (t == typeof(double))
            {
                return (double)data;

            }
            if (t == typeof(double?))
            {
                return (double)data;

            }
            if (t == typeof(float))
            {
                return (float)data;

            }
            if (t == typeof(float?))
            {
                return (float)data;

            }
            return null;

        }

        /// <summary>
        /// 判断是否为基础类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static bool IsBaseValueType<T>()
        {
            return IsBaseValueType(typeof(T));
        }

        /// <summary>
        /// 判断是否为基础类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool IsBaseValueType(Type type)
        {
            return type.IsEquivalentTo(typeof(Byte))
                          || type.IsEquivalentTo(typeof(SByte))
                          || type.IsEquivalentTo(typeof(Int32))
                          || type.IsEquivalentTo(typeof(Int16))
                          || type.IsEquivalentTo(typeof(Int64))
                          || type.IsEquivalentTo(typeof(Double))
                          || type.IsEquivalentTo(typeof(Decimal))
                          || type.IsEquivalentTo(typeof(string))
                          || type.IsEquivalentTo(typeof(System.Nullable<Byte>))
                          || type.IsEquivalentTo(typeof(System.Nullable<SByte>))
                          || type.IsEquivalentTo(typeof(System.Nullable<Int32>))
                          || type.IsEquivalentTo(typeof(System.Nullable<Int16>))
                          || type.IsEquivalentTo(typeof(System.Nullable<Int64>))
                          || type.IsEquivalentTo(typeof(System.Nullable<Double>))
                          || type.IsEquivalentTo(typeof(System.Nullable<Decimal>));
        }

        /// <summary>
        /// object转换成redis类型
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private RedisValue ChangeToRedisValue(object data)
        {
            RedisValue value = RedisValue.Null;
            bool isBaseType = false;

            if (data != null)
            {
                isBaseType = IsBaseValueType(data.GetType());
            }

            if (isBaseType)
            {
                value = ChangeBaseTypeToRedisValue(data);

            }
            else
            {

                if (data != null)
                {
                    //先序列化，再压缩
                    var buffer = serializer.Serialize(data);
                    byte[] persistent = LZ4.LZ4Codec.Wrap(buffer);
                    value = persistent;
                }

            }
            return value;
        }

        /// <summary>
        /// 解压，向下兼容LZ4和GZip
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string UnCompress(byte[] input)
        {
            var result = string.Empty;
            if (input != null && input.Length > 0)
            {
                try
                {
                    //LZ4
                    var bufferLZ4 = LZ4.LZ4Codec.Unwrap(input);
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream(bufferLZ4))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    //未压缩
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream(input))
                    {
                        using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        public void Dispose()
        {
            if (isDisposing == false)
            {
                isDisposing = true;
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }

        }
    }
}
