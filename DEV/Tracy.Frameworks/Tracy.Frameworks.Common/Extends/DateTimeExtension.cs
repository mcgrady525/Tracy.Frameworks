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
        /// </summary>
        /// <param name="input"></param>
        /// <returns>返回结果，例如：2016-12-06 23:59:59</returns>
        public static DateTime AddLastSecond(this DateTime input)
        {
            return input.Date.AddDays(1).AddSeconds(-1);
        }

        /// <summary>
        /// 当前日期加上23:59:59
        /// </summary>
        /// <param name="input"></param>
        /// <returns>返回结果，例如：2016-12-06 23:59:59，如果为null则返回DateTime.MinValue</returns>
        public static DateTime AddLastSecond(this DateTime? input)
        {
            if (!input.HasValue)
            {
                return DateTime.MinValue;
            }
            return AddLastSecond(input.Value);
        }

    }
}
