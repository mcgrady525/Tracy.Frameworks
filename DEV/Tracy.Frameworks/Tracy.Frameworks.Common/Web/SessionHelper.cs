using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tracy.Frameworks.Common.Web
{
    /// <summary>
    /// 描述:Session常用操作类
    /// 作者:鲁宁
    /// 时间:2013/10/14 18:30:30
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 保存Session(默认20分钟有效期)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(string key, object value)
        {
            Set(key,value,20);
        }

        /// <summary>
        /// 保存Session(指定有效期)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="timeout">过期期限,以分钟计</param>
        public static void Set(string key, object value,int timeout)
        {
            if (HttpContext.Current.Session[key] == null)
            {
                HttpContext.Current.Session[key] = value; //默认20分钟过期，可以sessionState 配置节的 timeout 特性控制会话状态生存期
                HttpContext.Current.Session.Timeout= timeout;
            }
        }

        /// <summary>
        /// 读取Session
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>session的值，需要转换类型</returns>
        public static object Get(string key)
        {
            object obj = null;
            if (HttpContext.Current.Session[key] != null)
            {
                obj= HttpContext.Current.Session[key];
            }
            return obj;
        }

        /// <summary>
        /// 移除指定Session
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null) HttpContext.Current.Session[key] = null; 
        }

        /// <summary>
        /// 判断该Session是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExist(string key)
        {
            if (HttpContext.Current.Session[key] != null) return true;
            return false;
        }

    }
}
