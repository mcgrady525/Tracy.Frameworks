using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.ComponentModel;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// 集合扩展
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// input==null || input.Count==0 时返回true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> input)
        {
            if (input == null || input.Count == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// input.Count>0 时返回true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasValue<T>(this ICollection<T> input)
        {
            return !input.IsNullOrEmpty();
        }

        /// <summary>
        /// list转dataTable(仅针对T中的公共属性)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            if (list == null)
            {
                return null;
            }

            Type type = typeof(T);
            var ps = type.GetProperties();
            Type targetType;
            NullableConverter nullableConvert;
            List<DataColumn> cols = new List<DataColumn>();
            foreach (var p in ps)
            {
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    nullableConvert = new NullableConverter(p.PropertyType);
                    targetType = nullableConvert.UnderlyingType;
                    cols.Add(new DataColumn(p.Name, targetType));
                }
                else {
                    cols.Add(new DataColumn(p.Name, p.PropertyType));
                }
            }
            
            DataTable dt = new DataTable();
            dt.Columns.AddRange(cols.ToArray());

            list.ForEach((l) => {
                List<object> objs = new List<object>();
                objs.AddRange(ps.Select(p => p.GetValue(l, null)));
                dt.Rows.Add(objs.ToArray());
            });

            return dt;
        }
        
        /// <summary>
        /// 集合去重(扩展distinct)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
