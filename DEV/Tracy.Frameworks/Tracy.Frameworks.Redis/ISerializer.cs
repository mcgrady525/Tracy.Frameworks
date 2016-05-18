using System;

namespace Tracy.Frameworks.Redis
{
    public interface ISerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        byte[] Serialize<T>(T data);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        byte[] Serialize(object data);

        /// <summary>
        /// 反序列化为类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        T Deserialize<T>(byte[] data);

        /// <summary>
        /// 反序列化为object
        /// </summary>
        /// <param name="t"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        object Deserialize(Type t, byte[] data);
    }
}
