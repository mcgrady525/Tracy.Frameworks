using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Const
{
    /// <summary>
    /// 日期，时间格式定义
    /// </summary>
    public static class DateFormat
    {
        /// <summary>
        /// 完整日期時间格式 yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string DATETIME = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 日期格式 yyyy-MM-dd
        /// </summary>
        public const string DATE = "yyyy-MM-dd";

        /// <summary>
        /// 時间格式 HH:mm:ss
        /// </summary>
        public const string TIME = "HH:mm:ss";

        /// <summary>
        /// 短年月格式 yyyy-MM
        /// </summary>
        public const string YEARMONTH = "yyyy-MM";

        /// <summary>
        /// 短日期格式 MM-dd
        /// </summary>
        public const string MONTHDAY = "MM-dd";

        /// <summary>
        /// 短日期時间格式 MM-dd HH:mm
        /// </summary>
        public const string MONTHDAYTIME = "MM-dd HH:mm";

        /// <summary>
        /// 短日期時间格式 yyyy-MM-dd HH:mm
        /// </summary>
        public const string DATEHOURMINUTE = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// 日/月/年 時：分: 秒 by fylv
        /// </summary>
        public const string DAYMONTHYEARHOURMINUTESEC = "dd'/'MM'/'yyyy HH:mm:ss";

        /// <summary>
        /// 日/月/年 時：分 by zhaot
        /// </summary>
        public const string DAYMONTHYEARTIME = "dd'/'MM'/'yyyy HH:mm";

        /// <summary>
        /// 日/月/年 by zhaot
        /// </summary>
        public const string DAYMONTHYEAR = "dd'/'MM'/'yyyy";

        /// <summary>
        /// 時：分 by zhaot
        /// </summary>
        public const string HOURMINUTE = "HH:mm";

        /// <summary>
        /// 完整日期時间格式（中文） yyyy年MM月dd日 HH時mm分ss秒
        /// </summary>
        public const string CNDATETIME = "yyyy年MM月dd日 HH時mm分ss秒";

        /// <summary>
        /// 日期格式（中文） yyyy年MM月dd日
        /// </summary>
        public const string CNDATE = "yyyy年MM月dd日";

        /// <summary>
        /// 時间格式（中文） HH時mm分ss秒
        /// </summary>
        public const string CNTIME = "HH時mm分ss秒";

        /// <summary>
        /// 短年月格式（中文） yyyy年MM月
        /// </summary>
        public const string CNYEARMONTH = "yyyy年MM月";

        /// <summary>
        /// 短日期格式（中文） MM月dd日
        /// </summary>
        public const string CNMONTHDAY = "MM月dd日";

        /// <summary>
        /// 短日期時间格式（中文） MM月dd日 HH時mm分
        /// </summary>
        public const string CNMONTHDAYTIME = "MM月dd日 HH時mm分";

        /// <summary>
        /// 短日期時间格式（中文） yyyy年MM月dd日 HH時mm分
        /// </summary>
        public const string CNDATEHOURMINUTE = "yyyy年MM月dd日 HH時mm分";

        /// <summary>
        /// 日期小时分钟秒毫秒
        /// </summary>
        public const string CNDATEHOURMINUTESECONDMILLSECOND = "yyyyMMddHHmmssfff";
    }
}
