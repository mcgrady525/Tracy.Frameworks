using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tracy.Frameworks.Common.Extends
{
    public static class ReflectionExtension
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> propertiesDic = new ConcurrentDictionary<Type, PropertyInfo[]>();

        /// <summary>
        /// 获取实体字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetEntityProperties(this Type type)
        {
            return propertiesDic.GetOrAdd(type, item => type.GetProperties(BindingFlags.Instance | BindingFlags.Public));
        }
    }
}
