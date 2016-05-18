using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Serialization
{
    /// <summary>
    /// 序列化器公共接口
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        string Serialize<T>(T input);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">输入对象</param>
        /// <returns>序列化结果,如果序列化过程中出现错误，则返回String.Empty</returns>
        string Serialize(object input);  

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="input">输入字符串</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回该类型的默认值</returns>
        T Deserialize<T>(string input);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="type">反序列化的目标对象类型</param>
        /// <returns>反序列化结果,如果序列化过程中出现错误，则返回null</returns>
        object Deserialize(string input, Type type);
    }
}
