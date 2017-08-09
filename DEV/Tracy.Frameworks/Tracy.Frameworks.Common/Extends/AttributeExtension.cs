using System;
using System.Reflection;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// 特性(Attribute)扩展
    /// </summary>
    public static class AttributeExtension
    {
        public static T GetAttribute<T>(this object obj) where T : class
        {
            Type type = obj.GetType();
            return type.GetAttribute<T>();
        }

        /// <summary>
        /// 获取自定义attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type) where T : class
        {
            Attribute customAttribute = Attribute.GetCustomAttribute(type, typeof(T));
            T result;
            if (customAttribute!= null)
            {
                result = (customAttribute as T);
            }
            else
            {
                result = default(T);
            }
            return result;
        }
    }
}
