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
    /// 包括list
    /// </summary>
    public static class CollectionExtension
    {
        /// <summary>
        /// 如果沒有找到，返回預設值, 而不是返回null
        /// </summary>
        /// <param name="coll"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Get(this NameValueCollection coll, string key, string defaultValue)
        {
            if (null == coll)
                throw new ArgumentNullException("coll");
            if (coll[key] == null)
                return defaultValue;
            else
                return coll[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
        {
            if (null == dic)
                throw new ArgumentNullException("dic");
            if (dic.ContainsKey(key))
                return dic[key];
            else
                return defaultValue;
        }

        /// <summary>
        /// 如果没有找到，抛出带键值的异常.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            if (null == dic)
                throw new ArgumentNullException("dic");
            if (dic.ContainsKey(key))
                return dic[key];
            else
                throw new ArgumentOutOfRangeException("key", key, "关键字不存在于给定的字典中");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (null == dic)
                throw new ArgumentNullException("dic");

            if (dic.ContainsKey(key))
                dic[key] = value;
            else
                dic.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static T ToEntity<T>(this IDictionary<string, object> dict) where T : new()
        {
            var t = typeof(T);
            var ps = t.GetProperties();
            T tmp = new T();

            foreach (var p in ps)
            {
                if (!p.CanWrite)
                    continue;
                //var v = dict.Get(p.Name);
                if (dict.ContainsKey(p.Name))
                {
                    p.SetValue(tmp, Convert.ChangeType(dict[p.Name], p.PropertyType), null);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static List<T> Integration<T>(this List<List<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static IList<T> Integration<T>(this IList<List<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static IList<T> Integration<T>(this IList<IList<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static List<T> Integration<T>(this ICollection<List<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static ICollection<T> Integration<T>(this ICollection<ICollection<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static ICollection<T> Integration<T>(this IEnumerable<ICollection<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static List<T> Integration<T>(this IQueryable<List<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static IQueryable<T> Integration<T>(this IQueryable<IQueryable<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).AsQueryable();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static IQueryable<T> Integration<T>(this IEnumerable<IQueryable<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).AsQueryable();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static List<T> Integration<T>(this IEnumerable<List<T>> nestList)
        {
            return Integration<T>(nestList as IEnumerable<IEnumerable<T>>).ToList();
        }

        /// <summary>
        /// 将嵌套列表整合为单维列表
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="nestList">嵌套列表</param>
        /// <returns>整合后的单维列表</returns>
        public static IEnumerable<T> Integration<T>(this IEnumerable<IEnumerable<T>> nestList)
        {
            var res = new List<T>();
            if (null == nestList || nestList.Count() == 0) return res;
            foreach (var innerList in nestList)
            {
                if (null != innerList && innerList.Count() > 0)
                    res.AddRange(innerList.ToList());
            }
            return res;
        }

        /// <summary>
        /// List转DataTable(仅针对T中的公共属性)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            if (list == null)
                return null;

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
            //var cols = from p in ps
            //           let pp = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)
            //           select new DataColumn(p.Name, p.PropertyType);

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
        /// 如果Source 中不存在指定的值，返回指定的默認值
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="defaultValue">指定的默認值</param>
        /// <returns></returns>
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, TSource defaultValue)
        {
            var result = source.FirstOrDefault(predicate);
            if (result == null)
                return defaultValue;
            else
                return result;
        }

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
        /// 扩展distinct
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
