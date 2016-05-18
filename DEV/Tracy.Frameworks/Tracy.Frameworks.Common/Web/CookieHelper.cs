using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Tracy.Frameworks.Common.Web
{
    /// <summary>
    /// 描述:Cookie常用操作类
    /// 作者:鲁宁
    /// 时间:2013/10/14 18:16:01
    /// </summary>
    public class CookieHelper
    {
        /// <summary>
        /// 保存Cookie(使用默认为一天)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void Set(string key, string value)
        {
            Set(key,value,DateTime.Now.AddDays(1.0));
        }

        /// <summary>
        /// 保存Cookie(使用指定有效期)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">有效期</param>
        public static void Set(string key, string value, DateTime expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie == null)
            {
                cookie = new HttpCookie(key) {Value=value,Expires=expires };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 读取指定key的Cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>指定键的值</returns>
        public static string Get(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                return HttpContext.Current.Request.Cookies[key].Value.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 清除指定Cookie
        /// </summary>
        /// <param name="key">键</param>
        public static void Remove(string key)
        { 
            HttpCookie cookie= HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1d); //assign a past expiration date on a cookie,to remove it
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}
