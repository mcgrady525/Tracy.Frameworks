using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Tracy.Frameworks.Common.FormatDate
{
    /// <summary>
    /// Description:日期时间操作辅助类
    /// Author:McgradyLu
    /// Time:8/19/2013 8:37:49 PM
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// UTC时间字符串转化为本地时间
        /// </summary>
        /// <param name="utcString">UTC字符串</param>
        /// <returns>本地时间</returns>
        public static DateTime UTCToLocal(string utcString)
        {
            //string utcString="Mon Aug 19 00:00:00 UTC+0800 2013";
            DateTime dt = DateTime.MinValue;
            if (utcString.ToUpper().IndexOf("UTC+") != -1)
                dt = DateTime.ParseExact(utcString, "ddd MMM d HH:mm:ss UTCK yyyy", new CultureInfo("en-GB"));
            return dt;
        }

        /// <summary>
        /// 获取指定年份,指定周数的开始日期和结束日期(开始日期为周日)
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="week">周数</param>
        /// <param name="first">开始日期</param>
        /// <param name="last">结束日期</param>
        /// <returns></returns>
        public static bool GetDaysOfWeeks(int year, int week, out DateTime first, out DateTime last)
        {
            first = DateTime.MinValue;
            last = DateTime.MinValue;

            try
            {
                if (year < 1700 || year > 9999)
                {
                    //"年份超限"
                    return false;
                }
                if (week < 1 || week > 53)
                {
                    //"周数错误"
                    return false;
                }
                DateTime startDay = new DateTime(year, 1, 1);  //该年第一天
                DateTime endDay = new DateTime(year + 1, 1, 1).AddMilliseconds(-1);
                int dayOfWeek = 0;
                if (Convert.ToInt32(startDay.DayOfWeek.ToString("d")) > 0)
                    dayOfWeek = Convert.ToInt32(startDay.DayOfWeek.ToString("d"));  //该年第一天为星期几
                if (dayOfWeek == 7) { dayOfWeek = 0; }
                if (week == 1)
                {
                    first = startDay;
                    if (dayOfWeek == 6)
                    {
                        last = first;
                    }
                    else
                    {
                        last = startDay.AddDays((6 - dayOfWeek));
                    }
                }
                else
                {
                    first = startDay.AddDays((7 - dayOfWeek) + (week - 2) * 7); //index周的起始日期
                    last = first.AddDays(6);
                    if (last > endDay)
                    {
                        last = endDay;
                    }
                }
                if (first > endDay)  //startDayOfWeeks不在该年范围内
                {
                    //"输入周数大于本年最大周数";
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取指定年份,指定月的开始日期和结束日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="first">月开始日期</param>
        /// <param name="last">月结束日期</param>
        /// <returns></returns>
        public static bool GetDaysOfMonths(int year, int month, out DateTime first, out DateTime last)
        {
            first = DateTime.MinValue;
            last = DateTime.MinValue;

            try
            {
                if (year < 1700 || year > 9999)
                {
                    //"年份超限"
                    return false;
                }
                if (month < 1 || month > 53)
                {
                    //"月份错误"
                    return false;
                }
                //获取该年该月的天数
                int days = DateTime.DaysInMonth(year, month);
                first = Convert.ToDateTime(String.Format("{0}-{1}-{2}", year.ToString(), month.ToString(), "1"));
                last = Convert.ToDateTime(String.Format("{0}-{1}-{2}", year.ToString(), month.ToString(), days.ToString()));
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取两个日期之间的天数
        /// </summary>
        /// <param name="oldDate">旧日期</param>
        /// <param name="newDate">新日期</param>
        /// <returns>相隔天数</returns>
        public static int DateDiff(DateTime oldDate, DateTime newDate)
        {
            if (oldDate == default(DateTime) || newDate == default(DateTime)) return 0;

            TimeSpan ts = newDate.Subtract(oldDate);
            return ts.Days;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
