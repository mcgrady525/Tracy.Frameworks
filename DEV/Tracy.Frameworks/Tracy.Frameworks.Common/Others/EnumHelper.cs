using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Common.Others
{
    /// <summary>
    /// 描述:枚举操作辅助类
    /// 作者:鲁宁
    /// 时间:2013/11/28 19:01
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举枚型中常数值的数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetEnumList<T>()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("not enum type");

            List<T> lst = new List<T>();
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                lst.Add(item);
            }
            return lst;
        }

        /// <summary>
        /// 依据描述获取枚举类型
        /// </summary>
        /// <typeparam name="EnumType">枚举类型</typeparam>
        /// <param name="description">描述</param>
        /// <returns></returns>
        public static T GetValueByDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException("not enum type");
            foreach (var enumName in Enum.GetNames(type))
            {
                var enumValue = Enum.Parse(type, enumName);
                if (description == ((Enum)enumValue).GetDescription())
                    return (T)enumValue;
            }
            throw new ArgumentException("There is no value with this description among specified enum type values.");
        }
    }
}
