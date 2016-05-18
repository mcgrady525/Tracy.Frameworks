using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Serialization
{
    /// <summary>
    /// JavaScriptSerializer的包装类,用于处理JSON,要求对象类及其属性上有DataContract及DataMember特性声明
    /// 支持具有[DataContract(IsReference = true)]特性的对象
    /// 也支持DynamicObject和ExpandoObject对象，但序列化结果不标准
    /// </summary>       
    public class JsonDoNetSerializer : ISerializer
    {
        /// <summary>
        /// 序列化xml
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        public static string GetString<T>(T input)
        {
            return GetString((object)input);
        }
        /// <summary>
        /// 序列化xml
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
                return Newtonsoft.Json.JsonConvert.SerializeObject(input);
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
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(input);
            }
            catch
            {
                return default(T);
            }
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
                return Newtonsoft.Json.JsonConvert.DeserializeObject(input);
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
}
