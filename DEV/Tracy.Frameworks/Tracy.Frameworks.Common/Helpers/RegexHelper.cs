using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tracy.Frameworks.Common.Helpers
{
    /// <summary>
    /// 正则表达式helper
    /// 封装常用验证
    /// </summary>
    public class RegexHelper
    {
        /// <summary>
        /// 是否有效IP地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

    }
}
