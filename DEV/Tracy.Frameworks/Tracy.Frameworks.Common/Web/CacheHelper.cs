using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Tracy.Frameworks.Common.Web
{
    /// <summary>
    /// 描述:缓存常用操作类
    /// 作者:鲁宁
    /// 时间:2013/8/15 14:45:01
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 保存缓存(默认到期为1分钟)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(string key, object value)
        {
            Set(key,value,TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// 保存缓存,使用指定到期策略
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expire">到期策略</param>
        public static void Set(string key, object value, TimeSpan expire)
        {
            Cache cache= HttpRuntime.Cache;
            if (cache[key] == null)
            {
                cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, expire); //滑动到期，注意滑动到期与绝对到期之间的区别
            }
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static object Get(string key)
        {
            object obj = null;
            if (HttpRuntime.Cache[key] != null)
            {
                obj = HttpRuntime.Cache[key];
            }
            return obj;
        }

        public static bool IsExist(string key)
        {
            if (HttpRuntime.Cache[key] != null) return true;
            return false;
        }
    }
}
