using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// datetime扩展
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 当前日期加上23:59:59
        /// 如：2016-12-06 23:59:59
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime? AddLastSecond(this DateTime? input)
        {
            if (input.HasValue)
            {
                return input.Value.Date.AddDays(1).AddSeconds(-1);
            }
            return input;
        }
    }
}
