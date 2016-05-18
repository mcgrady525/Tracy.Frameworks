using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;
using System.Dynamic;
#if EF6
using System.Data.Entity.Core.Objects;
#else
using System.Data.Objects;
#endif
namespace Tracy.Frameworks.Common.Serialization
{
    #region StandardSerializer

    /// <summary>
    /// 标准序列化器
    /// 根据对象特征和序列化格式智能选择采用何种序列化器进行进行序列化和反序列化
    /// 可能的序列化器包括DataContractSerializer、DataContractJsonSerializer、JavaScriptSerializer和JSON.NET
    /// JSON:
    ///     普通,采用DataContractJsonSerializer
    ///     动态类型,采用JavaScriptSerializer
    ///     具有[DataContract(IsReference = true)]特性,采用JSON.NET
    /// XML:
    ///     普通,采用DataContractSerializer
    ///     动态类型,返回空
    ///     具有[DataContract(IsReference = true)]特性,采用DataContractSerializer外加ProxyDataContractResolver
    /// </summary>
    public class StandardSerializer : ISerializer
    {
        #region Smart Methods

        //类型及类型中属性类型探查，返回值为1 全是普通类型, 2 包含动态类型, 4 包含IsReference = true
        static byte TypeSearch(Type type, ref List<Type> routeTypes, bool isDynamicSearch = true)
        {
            byte flag = 1;
            var attrs = type.GetCustomAttributes(typeof(DataContractAttribute), true).OfType<DataContractAttribute>();
            if (attrs != null && attrs.Any(p => p.IsReference == true))
            {
                return 4; //找到IsReference = true的，最strong，返回
            }
            else
            {
                if (isDynamicSearch && type == typeof(DynamicObject) || type == typeof(ExpandoObject) || type == typeof(DynamicMetaObject) ||
                    type.IsSubclassOf(typeof(DynamicObject)) || type.IsSubclassOf(typeof(ExpandoObject)) || type.IsSubclassOf(typeof(DynamicMetaObject)))
                {
                    flag = 2; //找到动态的，记录下来，还要看类中属性的类型的表现
                }

                var insideTypes = type.GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Select(p => p.GetType()).Distinct();
                if (insideTypes.Any())
                {
                    insideTypes = insideTypes.Where(p => p.IsClass && !p.FullName.StartsWith("System"));
                    if (insideTypes.Any())
                    {
                        var newTypes = insideTypes.Except(routeTypes); //发现之前没有探查过的类型
                        if (newTypes.Any())
                        {
                            routeTypes = routeTypes.Concat(newTypes).ToList(); //记录已探查类型
                            foreach (var item in newTypes)
                            {
                                var result = TypeSearch(item, ref routeTypes, isDynamicSearch); //递归检查新发现的类型
                                if (result > flag)
                                    flag = result;
                            }
                        }
                    }
                }

                return flag;
            }
        }

        // 智能选择序列化器
        static ISerializer TakeSerializer(Type type, SerializationFormat format)
        {
            List<Type> routeTypes = new List<Type> { type };
            var level = TypeSearch(type, ref routeTypes, format == SerializationFormat.Json);
            routeTypes = null;
            if (format == SerializationFormat.Json)
            {
                if (level == 4)
                {
                    return new JsonDoNetSerializer();
                }
                else if (level == 2)
                {
                    return new JsSerializer();
                }
                else
                {
                    return new EntityContractJsonSerializer();
                }
            }
            else
            {
                if (level == 4)
                {
                    return new DbContextSerializer();
                }
                else
                {
                    return new EntityContractSerializer();
                }
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <param name="format">序列化格式,默认json</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString<T>(T input, SerializationFormat format = SerializationFormat.Json)
        {
            return TakeSerializer(typeof(T), format).Serialize(input);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <param name="format">序列化格式,默认json</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString(object input, SerializationFormat format = SerializationFormat.Json)
        {
            return TakeSerializer(input.GetType(), format).Serialize(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <param name="format">反序列化格式,默认json</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public static T GetObject<T>(string input, SerializationFormat format = SerializationFormat.Json)
        {
            return TakeSerializer(typeof(T), format).Deserialize<T>(input);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <param name="format">反序列化格式,默认json</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public static object GetObject(string input, Type type, SerializationFormat format = SerializationFormat.Json)
        {
            return TakeSerializer(type, format).Deserialize(input, type);
        }

        #endregion

        #region Interface Impl

        /// <summary>
        /// 构造函数
        /// </summary>
        public StandardSerializer(Type type, SerializationFormat format = SerializationFormat.Json) 
        {
            this.DeclaringSerializer = TakeSerializer(type, format);
        }

        /// <summary>
        /// 获取智能选择实际调用的序列化器
        /// </summary>
        public ISerializer DeclaringSerializer { get; internal protected set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize<T>(T input)
        {
            return this.DeclaringSerializer.Serialize(input);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize(object input)
        {
            return this.DeclaringSerializer.Serialize(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public T Deserialize<T>(string input)
        {
            return this.DeclaringSerializer.Deserialize<T>(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public object Deserialize(string input, Type type)
        {
            return this.DeclaringSerializer.Deserialize(input, type);
        }

        #endregion

        #region DataContractResolver Customizing


        /// <summary>
        /// 序列化,注意,因为仅XML格式支持DataContractResolver,所有此方法一定是采用DataContracSerializer进行XML序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <param name="resolver">自定义DataContractResolver</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString<T>(T input, DataContractResolver resolver)
        {
            try
            {
                using (var ms = new MemoryStream())
                {   
                    DataContractSerializer xmlSer = new DataContractSerializer(typeof(T),
                        null, Int32.MaxValue, false, true, null, resolver);
                    xmlSer.WriteObject(ms, input);

                    var res = Encoding.UTF8.GetString(ms.ToArray());
                    return res;
                }
            }
            catch
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// 序列化,注意,因为仅XML格式支持DataContractResolver,所有此方法一定是采用DataContracSerializer进行XML序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <param name="resolver">自定义DataContractResolver</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString(object input, DataContractResolver resolver)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    DataContractSerializer xmlSer = new DataContractSerializer(input.GetType(),
                        null, Int32.MaxValue, false, true, null, resolver);
                    xmlSer.WriteObject(ms, input);

                    var res = Encoding.UTF8.GetString(ms.ToArray());
                    return res;
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 反序列化,注意,因为仅XML格式支持DataContractResolver,所有此方法一定是采用DataContracSerializer进行XML序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <param name="resolver">自定义DataContractResolver</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public static T GetObject<T>(string input, DataContractResolver resolver)
        {
            T res = default(T);
            var obj = GetObject(input, typeof(T), resolver);
            if (null != obj) res = (T)obj;
            return res;
        }
        /// <summary>
        /// 反序列化,注意,因为仅XML格式支持DataContractResolver,所有此方法一定是采用DataContracSerializer进行XML序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <param name="resolver">自定义DataContractResolver</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public static object GetObject(string input, Type type, DataContractResolver resolver)
        {
            try
            {
                object res = null;
                var bytes = Encoding.UTF8.GetBytes(input);
                using (var stream = new MemoryStream(bytes))
                {
                    DataContractSerializer xmlSer = new DataContractSerializer(type,
                        null, Int32.MaxValue, false, true, null, resolver);
                    res = xmlSer.ReadObject(stream);
                }
                return res;
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }

    #endregion

    #region EntityContractJsonSerializer

    /// <summary>
    /// DataContractJsonSerializer的包装类,用于处理JSON,要求对象类及其属性上有DataContract及DataMember特性声明
    /// </summary>       
    public class EntityContractJsonSerializer : ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString<T>(T input)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    DataContractJsonSerializer xmlSer = new DataContractJsonSerializer(typeof(T));
                    xmlSer.WriteObject(ms, input);
                    var res = Encoding.UTF8.GetString(ms.ToArray());
                    return res;
                }
            }
            catch
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize<T>(T input)
        {
            return GetString(input);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString(object input)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    DataContractJsonSerializer xmlSer = new DataContractJsonSerializer(input.GetType());
                    xmlSer.WriteObject(ms, input);
                    var res = Encoding.UTF8.GetString(ms.ToArray());
                    return res;
                }
            }
            catch
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize(object input)
        {
            return GetString(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public static T GetObject<T>(string input)
        {
            T res = default(T);
            var obj = GetObject(input, typeof(T));
            if (null != obj) res = (T)obj;
            return res;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public T Deserialize<T>(string input)
        {
            return GetObject<T>(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public static object GetObject(string input, Type type)
        {
            try
            {
                object res = null;
                var bytes = Encoding.UTF8.GetBytes(input);
                using (var ms = new MemoryStream(bytes))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    DataContractJsonSerializer xmlSer = new DataContractJsonSerializer(type);
                    res = xmlSer.ReadObject(ms);
                }
                return res;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public object Deserialize(string input, Type type)
        {
            return GetObject(input, type);
        }
    }
    
    #endregion

    #region EntityContractSerializer

    /// <summary>
    /// DataContractSerializer的包装类,用于处理XML,要求对象类及其属性上有DataContract及DataMember特性声明
    /// </summary>       
    public class EntityContractSerializer : ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString<T>(T input)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    DataContractSerializer xmlSer = new DataContractSerializer(typeof(T));
                    xmlSer.WriteObject(ms, input);
                    var res = Encoding.UTF8.GetString(ms.ToArray());
                    return res;
                }
            }
            catch
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize<T>(T input)
        {
            return GetString(input);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString(object input)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    DataContractSerializer xmlSer = new DataContractSerializer(input.GetType());
                    xmlSer.WriteObject(ms, input);
                    var res = Encoding.UTF8.GetString(ms.ToArray());
                    return res;
                }
            }
            catch
            {
                return String.Empty;
            }
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize(object input)
        {
            return GetString(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public static T GetObject<T>(string input)
        {
            T res = default(T);
            var obj = GetObject(input, typeof(T));
            if (null != obj) res = (T)obj;
            return res;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public T Deserialize<T>(string input)
        {
            return GetObject<T>(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public static object GetObject(string input, Type type)
        {
            try
            {
                object res = null;
                var bytes = Encoding.UTF8.GetBytes(input);
                using (var ms = new MemoryStream(bytes))
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    DataContractSerializer xmlSer = new DataContractSerializer(type);
                    res = xmlSer.ReadObject(ms);
                }
                return res;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public object Deserialize(string input, Type type)
        {
            return GetObject(input, type);
        }
    }

    #endregion

    #region DbContextSerializer

    /// <summary>
    /// 支持EF DbContext特别XML序列化器，采用DataContracSerializer外加ProxyDataContractResolver进行序列化和反序列化，要求对象类及其属性上有DataContract及DataMember特性声明
    /// 且支持具有[DataContract(IsReference = true)]和[System.ComponentModel.DataAnnotations.MetadataType]特性的对象
    /// </summary>       
    public class DbContextSerializer : ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString<T>(T input)
        {
            return StandardSerializer.GetString(input, new ProxyDataContractResolver());
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize<T>(T input)
        {
            return GetString(input);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString(object input)
        {
            return StandardSerializer.GetString(input, new ProxyDataContractResolver());
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public string Serialize(object input)
        {
            return GetString(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public static T GetObject<T>(string input)
        {
            return StandardSerializer.GetObject<T>(input, new ProxyDataContractResolver());
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        public T Deserialize<T>(string input)
        {
            return GetObject<T>(input);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public static object GetObject(string input, Type type)
        {
            return StandardSerializer.GetObject(input, type, new ProxyDataContractResolver());
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        public object Deserialize(string input, Type type)
        {
            return GetObject(input, type);
        }
    }

    #endregion
}
