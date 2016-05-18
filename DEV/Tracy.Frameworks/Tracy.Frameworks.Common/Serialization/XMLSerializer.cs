using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Tracy.Frameworks.Common.Serialization
{
    /// <summary>
    /// XML列化器，采用XmlSerializer进行序列化和反序列化，要求对象类及其属性上有Serializable特性声明
    /// </summary>
    public class XMLSerializer : ISerializer
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
                    XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                    xmlSer.Serialize(ms, input);
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
            if (null == input)
                return String.Empty;
            try
            {
                using (var ms = new MemoryStream())
                {
                    XmlSerializer xmlSer = new XmlSerializer(input.GetType());
                    xmlSer.Serialize(ms, input);
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
                using (var stream = new MemoryStream(bytes))
                {
                    XmlSerializer xmlSer = new XmlSerializer(type);
                    res = xmlSer.Deserialize(stream);
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
}
