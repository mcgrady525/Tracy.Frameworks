using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/7/16 9:37:07
 * 描述说明：DateTime的扩展
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Extends
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 两个时间，是否是同一天,年月日是否相等
        /// </summary>
        public static bool IsSameDay(this DateTime d1, DateTime date)
        {
            return string.Equals(d1.ToString("yyyyMMdd"), date.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 日期比较(不比较时间), 相同：0，小于 :负数, 大于:正数
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="date"></param>
        /// <returns>相同：0，小于 :负数, 大于:正数, 间隔天数</returns>
        public static int CompareDate(this DateTime d1, DateTime date)
        {
            DateTime t1 = d1.GetDate();
            DateTime t2 = date.GetDate();

            return (t1 - t2).Days;
        }

        /// <summary>
        /// 只获取日期部份
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime GetDate(this DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day);
        }

        /// <summary>
        /// 只获取时间部份
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static TimeSpan GetTime(this DateTime d)
        {
            return d - d.GetDate();
        }

        /// <summary>
        /// To時間加上23:59
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? AddLastMinute(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.AddLastMinute();
            }
            return date;
        }

        public static DateTime AddLastMinute(this DateTime date)
        {
            return date.Date.AddDays(1).AddSeconds(-1);
        }
    }
}
