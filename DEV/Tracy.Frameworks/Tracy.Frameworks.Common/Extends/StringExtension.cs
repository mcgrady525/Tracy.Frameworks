using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web;
using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// string字符串扩展
    /// 包括常用转换，序列化和解压缩等
    /// </summary>
    public static class StringExtension
    {
        #region 常用转换

        #region To int
        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string str, int defaultValue)
        {
            int v;
            if (int.TryParse(str, out v))
            {
                return v;
            }
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            return str.ToInt(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int? ToIntOrNull(this string str, int? defaultValue)
        {
            int v;
            if (int.TryParse(str, out v))
                return v;
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int? ToIntOrNull(this string str)
        {
            return str.ToIntOrNull(null);
        }

        /// <summary>
        /// 智慧轉換為 Int ，取字串中的第一個數位串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int SmartToInt(this string str, int defaultValue)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            //Match ma = Regex.Match(str, @"(\d+)");
            Match ma = Regex.Match(str, @"((-\s*)?\d+)");
            if (ma.Success)
            {
                return ma.Groups[1].Value.Replace(" ", "").ToInt(defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region To long
        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ToLong(this string str, long defaultValue)
        {
            long v;
            if (long.TryParse(str, out v))
                return v;
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToLong(this string str)
        {
            return str.ToLong(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long? ToLongOrNull(this string str)
        {
            long temp;
            if (long.TryParse(str, out temp))
                return temp;
            else
                return null;
        }

        #endregion

        #region To ulong
        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static ulong ToUlong(this string str, ulong defaultValue)
        {
            ulong v;
            if (ulong.TryParse(str, out v))
                return v;
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ulong ToUlong(this string str)
        {
            return str.ToUlong(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static ulong? ToUlongOrNull(this string str)
        {
            ulong temp;
            if (ulong.TryParse(str, out temp))
                return temp;
            else
                return null;
        }

        #endregion

        #region To Float
        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string str, float defaultValue)
        {
            float v;
            if (float.TryParse(str, out v))
                return v;
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            return str.ToFloat(0f);
        }

        /// <summary>
        /// 智慧轉換為 float ，取字串中的第一個數位串
        /// 可轉換 a1.2, 0.12 1.2aa , 1.2e+7
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float SmartToFloat(this string str, float defaultValue)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            //Regex rx = new Regex(@"((\d+)(\.?((?=\d)\d+))?(e\+\d)*)");
            Regex rx = new Regex(@"((-\s*)?(\d+)(\.?((?=\d)\d+))?(e[\+-]?\d+)*)");
            Match ma = rx.Match(str);
            if (ma.Success)
            {
                return ma.Groups[1].Value.Replace(" ", "").ToFloat(defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }
        #endregion

        #region To Double
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ToDouble(this string str, double defaultValue)
        {
            double v;
            if (double.TryParse(str, NumberStyles.Any, null, out v))
                return v;
            else
                return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            return str.ToDouble(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double? ToDoubleOrNull(this string str)
        {
            double temp;
            if (double.TryParse(str, out temp))
                return temp;
            else
                return null;
        }

        #endregion

        #region To Decimal

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str, decimal defaultValue)
        {
            decimal v;
            if (decimal.TryParse(str, NumberStyles.Any, null, out v))
                return v;
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            return str.ToDecimal(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ToDecimalOrNull(this string str)
        {
            decimal temp;
            if (decimal.TryParse(str, out temp))
                return temp;
            else
                return null;
        }

        /// <summary>
        /// 智慧轉換為 float ，取字串中的第一個數位串
        /// 可轉換 a1.2, 0.12 1.2aa , 1.2e+7
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal SmartToDecimal(this string str, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(str))
                return defaultValue;

            //Regex rx = new Regex(@"((\d+)(\.?((?=\d)\d+))?(e\+\d)*)");
            Regex rx = new Regex(@"((-\s*)?(\d+)(\.?((?=\d)\d+))?(e[\+-]?\d+)*)");
            Match ma = rx.Match(str);
            if (ma.Success)
            {
                return ma.Groups[1].Value.Replace(" ", "").ToDecimal(defaultValue);
            }
            else
            {
                return defaultValue;
            }
        }

        #endregion

        #region To byte
        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static byte ToByte(this string str, byte defaultValue)
        {
            byte v;
            if (byte.TryParse(str, out v))
            {
                return v;
            }
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte ToByte(this string str)
        {
            return str.ToByte(0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static byte? ToByteOrNull(this string str, byte? defaultValue)
        {
            byte v;
            if (byte.TryParse(str, out v))
                return v;
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte? ToByteOrNull(this string str)
        {
            return str.ToByteOrNull(null);
        }
        #endregion

        #region To byte[]

        /// <summary>
        /// string to byte[]
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string str)
        {
            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// byte[] to string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FromByteArray(this byte[] input)
        {
            return System.Text.Encoding.UTF8.GetString(input);
        }

        #endregion

        #region To Bool

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBool(this string str, bool defaultValue)
        {
            bool b;
            if (bool.TryParse(str, out b))
            {
                return b;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ToBool(this string str)
        {
            return str.ToBool(false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool? ToBoolOrNull(this string str)
        {
            bool temp;
            if (bool.TryParse(str, out temp))
                return temp;
            else
                return null;
        }


        #endregion

        #region To DateTime
        /// <summary>
        /// 轉換為日期，如果轉換失敗，返回預設值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeOrNull(this string str, DateTime? defaultValue = null)
        {
            DateTime d;
            if (DateTime.TryParse(str, out d))
                return d;
            else
            {
                if (DateTime.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy'/'MM'/'dd HH:mm:ss", "MM'/'dd'/'yyyy HH:mm:ss", "yyyy-M-d", "yyy-M-d hh:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                    return d;
                else
                    return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dateFmt"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeOrNull(this string str, string dateFmt, DateTime? defaultValue = null)
        {
            DateTime d;
            //if (DateTime.TryParse(str, out d))
            //    return d;
            //else {
            if (DateTime.TryParseExact(str, dateFmt, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                return d;
            else
                return defaultValue;
            //}        
        }

        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// 轉換日期，轉換失敗時，返回 defaultValue
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, DateTime defaultValue = default(DateTime))
        {
            DateTime d;
            if (DateTime.TryParse(str, out d))
                return d;
            else
            {
                if (DateTime.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "MM/dd/yyyy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                    return d;
                else
                    if (default(DateTime) == defaultValue)
                        return MinDate;
                    else
                        return defaultValue;
            }
        }

        /// <summary>
        /// 按給定日期格式進行日期轉換
        /// </summary>
        /// <param name="str"></param>
        /// <param name="dateFmt"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str, string dateFmt, DateTime defaultValue)
        {
            DateTime d;
            //if (DateTime.TryParse(str, out d))
            //    return d;
            //else {
            if (DateTime.TryParseExact(str, dateFmt, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                return d;
            else
                return defaultValue;
            //}            
        }

        /// <summary>
        /// 轉換為日期，轉換失敗時，返回null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeOrNull(this string str)
        {
            return str.ToDateTimeOrNull(null);
        }

        /// <summary>
        /// 轉換日期，轉換失敗時，返回當前時間
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return str.ToDateTime(DateTime.Now);
        }

        
        #endregion

        #region To TimeSpan
        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string str, TimeSpan defaultValue)
        {
            TimeSpan t;
            if (TimeSpan.TryParse(str, out t))
            {
                return t;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this string str)
        {
            return str.ToTimeSpan(new TimeSpan());
        }
        #endregion

        #region To Enum

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static T ToEnum<T>(this int value, T defaultValue) where T : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new Exception("T 必须是枚举类型");

            if (Enum.IsDefined(type, value))
            {
                return (T)Enum.ToObject(type, value);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToEnum<T>(default(T));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes")]
        public static T ToEnum<T>(this byte value, T defaultValue) where T : struct, IComparable, IConvertible, IFormattable
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new Exception("T 必须是枚举类型");

            if (Enum.IsDefined(type, value))
            {
                return (T)Enum.ToObject(type, value);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this byte value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToEnum<T>(default(T));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T defaultValue, bool ignoreCase) where T : struct, IComparable, IConvertible, IFormattable
        {

            T o;
            bool flag = Enum.TryParse<T>(value, ignoreCase, out o);
            if (flag && Enum.IsDefined(typeof(T), o))
                return o;
            else
                return defaultValue;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToEnum<T>(defaultValue, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToEnum<T>(default(T));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToEnum<T>(default(T), ignoreCase);
        }


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static int GetEnumValue<T>(this string value, int defaultValue, bool ignoreCase) where T : struct, IComparable, IConvertible, IFormattable
        {

            T o;
            bool flag = Enum.TryParse<T>(value, ignoreCase, out o);
            if (flag)

                return System.Convert.ToInt32(o);
            else
                return defaultValue;

        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetEnumValue<T>(this string value, int defaultValue) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.GetEnumValue<T>(defaultValue, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetEnumValue<T>(this T value, int defaultValue) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (!typeof(T).IsEnum)
            {
                return defaultValue;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetEnumValue<T>(this T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.GetEnumValue<T>(0);
        }

        #endregion

        #region To Guid
        /// <summary>
        /// 将字符串转换成GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str)
        {
            Guid g;
            if (Guid.TryParse(str, out g))
                return g;
            else
                return Guid.Empty;
        }
        #endregion

        #endregion

        #region 常用判断

        /// <summary>
        /// 判断字符串是否为null或空
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// 判断字符串是否为null，空或空白字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// 判断字符串是否日期时间格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string str)
        {
            //return Regex.IsMatch(str, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
            DateTime d;
            if (DateTime.TryParseExact(str, new string[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyyMMdd", "yyyyMMdd HH:mm:ss", "yyyy/MM/dd", "yyyy/MM/dd HH:mm:ss", "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d))
                return true;
            else
                return false;
        }

        #region In Between系列

        //ScottGu In扩展 改进
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool In<T>(this T t, params T[] c)
        {
            return c.Any(i => i.Equals(t));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool In<T>(this T t, IEnumerable<T> c)
        {
            return c.Any(i => i.Equals(t));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <param name="includeLowerBound"></param>
        /// <param name="includeUpperBound"></param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T input, T lowerBound, T upperBound, bool includeLowerBound = false, bool includeUpperBound = false) where T : IComparable<T>
        {
            if (null == input) throw new ArgumentNullException("input");

            var lowerCompareResult = input.CompareTo(lowerBound);
            var upperCompareResult = input.CompareTo(upperBound);

            return
                (includeLowerBound && lowerCompareResult == 0) ||
                (includeUpperBound && upperCompareResult == 0) ||
                (lowerCompareResult > 0 && upperCompareResult < 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <param name="comparer"></param>
        /// <param name="includeLowerBound"></param>
        /// <param name="includeUpperBound"></param>
        /// <returns></returns>
        public static bool IsBetween<T>(this T input, T lowerBound, T upperBound, IComparer<T> comparer, bool includeLowerBound = false, bool includeUpperBound = false)
        {
            if (null == input || null == comparer) throw new ArgumentNullException(null == input ? "input" : "comparer");

            var lowerCompareResult = comparer.Compare(input, lowerBound);
            var upperCompareResult = comparer.Compare(input, upperBound);

            return
                (includeLowerBound && lowerCompareResult == 0) ||
                (includeUpperBound && upperCompareResult == 0) ||
                (lowerCompareResult > 0 && upperCompareResult < 0);
        }


        #endregion

        #endregion

        #region 序列化

        #region Json序列化

        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isNeedFormat">默认false</param>
        /// <param name="isCanCyclicReference">默认true,生成的json每个对象会自动加上类似 "$id":"1","$ref": "1"</param>
        /// <param name="dateTimeFormat">默认null,即使用json.net默认的序列化机制，如："\/Date(1439335800000+0800)\/"</param>
        /// <param name="typeNameHandling">默认null,序列化时指定typename处理方式。None = 0,Objects = 1,Arrays = 2,All = 3,Auto = 4.对应的枚举是Newtonsoft.Json.TypeNameHandling</param>
        /// <param name="ignoreNullValue">值为null的不进行序列化（比如某个属性的值为null，序列化后没有此属性存在)</param>
        /// <returns></returns>
        public static string ToJson(this object input, bool isNeedFormat = false, bool isCanCyclicReference = true,
                                    string dateTimeFormat = null, int? typeNameHandling = null, bool ignoreNullValue = true, bool isEscapeHtml = false)
        {
            var format = isNeedFormat ? Formatting.Indented : Formatting.None;
            var settings = new JsonSerializerSettings();
            settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;//兼容<=4.5版本，默认序列化成微软的datetime json格式，e.g. "\/Date(1198908717056+0800)\/"，如果要输出ISO标准时间，可以通过dateTimeFormat进行设置。

            if (isCanCyclicReference)
            {
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            }
            if (!string.IsNullOrWhiteSpace(dateTimeFormat))
            {
                var jsonConverter = new List<JsonConverter>()
                {
                    new Newtonsoft.Json.Converters.IsoDateTimeConverter(){ DateTimeFormat = dateTimeFormat }//如： "yyyy-MM-dd HH:mm:ss"
                };
                settings.Converters = jsonConverter;
            }
            if (typeNameHandling.HasValue)
            {
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>();
                settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            }
            if (ignoreNullValue)
            {
                settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }
            if (isEscapeHtml)
            {
                settings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;//兼容JavaScriptSerializer对html字符串的处理。                
            }
            var json = JsonConvert.SerializeObject(input, format, settings);
            return json;
        }

        /// <summary>
        /// 从json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="dateTimeFormat">默认null,即使用json.net默认的序列化机制</param>
        /// <param name="typeNameHandling">默认null,反序列化时指定typename处理方式。None = 0,Objects = 1,Arrays = 2,All = 3,Auto = 4.对应的枚举是Newtonsoft.Json.TypeNameHandling</param>
        /// <returns></returns>
        public static T FromJson<T>(this string input, string dateTimeFormat = null, int? typeNameHandling = null)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            };
            if (typeNameHandling.HasValue)
            {
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>();
                settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            }

            if (!string.IsNullOrWhiteSpace(dateTimeFormat))
            {
                var jsonConverter = new List<JsonConverter>()
                {
                    new Newtonsoft.Json.Converters.IsoDateTimeConverter(){ DateTimeFormat = dateTimeFormat }//如： "yyyy-MM-dd HH:mm:ss"
                };
                settings.Converters = jsonConverter;
            }

            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        /// <summary>
        /// 从json字符串反序列化为对象
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type">要反序化的类型</param>
        /// <param name="dateTimeFormat">默认null,即使用json.net默认的序列化机制</param>
        /// <param name="typeNameHandling">默认null,反序列化时指定typename处理方式。None = 0,Objects = 1,Arrays = 2,All = 3,Auto = 4.对应的枚举是Newtonsoft.Json.TypeNameHandling</param>
        /// <returns></returns>
        public static object FromJson(this string input, Type type, string dateTimeFormat = null, int? typeNameHandling = null)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            };
            if (typeNameHandling.HasValue)
            {
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>();
                settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            }

            if (!string.IsNullOrWhiteSpace(dateTimeFormat))
            {
                var jsonConverter = new List<JsonConverter>()
                {
                    new Newtonsoft.Json.Converters.IsoDateTimeConverter(){ DateTimeFormat = dateTimeFormat }//如： "yyyy-MM-dd HH:mm:ss"
                };
                settings.Converters = jsonConverter;
            }

            return JsonConvert.DeserializeObject(input, type, settings);
        }

        #endregion

        #region Xml序列化

        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="isOmitXmlDeclaration">去掉xml默认的声明，默认为false</param>
        /// <param name="isOmitDefaultNamespaces">去掉默认的命名空间，默认为false</param>
        /// <param name="isNeedFormat">//是否换行缩进,默认为false</param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj, bool isOmitXmlDeclaration = false, bool isOmitDefaultNamespaces = false, bool isNeedFormat = false, Action<Exception> onError = null)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                    settings.OmitXmlDeclaration = isOmitXmlDeclaration;
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = isNeedFormat;
                    using (var writer = System.Xml.XmlWriter.Create(stream, settings))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        var namespaces = new XmlSerializerNamespaces();
                        if (isOmitDefaultNamespaces)
                        {
                            namespaces.Add("", "");
                        }
                        serializer.Serialize(writer, obj, namespaces);
                    }
                    return Encoding.UTF8.GetString(stream.GetBuffer());
                }
            }
            catch (Exception ex)
            {
                if (onError != null)
                {
                    onError(ex);
                }
                return null;
            }
        }

        /// <summary>
        /// 从xml字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string xml, Action<Exception> onError = null)
        {
            try
            {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                if (onError != null)
                {
                    onError(ex);
                }
                return default(T);
            }
        }

        #endregion

        #endregion

        #region 解压缩

        #region GZip

        /// <summary>
        /// 将字符串压缩为byte数组(GZip)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(this string input)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(input);

            using (MemoryStream msTemp = new MemoryStream())
            {
                using (GZipStream gz = new GZipStream(msTemp, CompressionMode.Compress, true))
                {
                    gz.Write(buffer, 0, buffer.Length);
                    gz.Close();

                    return msTemp.GetBuffer();
                }
            }
        }

        /// <summary>
        /// 从byte数组解压成字符串(GZip)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GZipDecompress(this byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                byte[] buffer = new byte[0x1000];
                int length = 0;

                using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (MemoryStream msTemp = new MemoryStream())
                    {
                        while ((length = gz.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            msTemp.Write(buffer, 0, length);
                        }

                        return System.Text.Encoding.UTF8.GetString(msTemp.ToArray());
                    }
                }
            }
        }

        #endregion

        #region LZ4

        /// <summary>
        /// 将字符串压缩为byte数组(LZ4)
        /// LZ4的压缩速度是GZip的7倍
        /// LZ4的解压速度是GZip的5倍
        /// LZ4的压缩率是GZip的2/3
        /// LZ4的CPU使用率较GZip减小了16%
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] LZ4Compress(this string input)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            var result = LZ4.LZ4Codec.Wrap(buffer);
            return result;
        }

        /// <summary>
        /// 从byte数组解压成字符串(LZ4)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string LZ4Decompress(this byte[] data)
        {
            var result = LZ4.LZ4Codec.Unwrap(data);
            return Encoding.UTF8.GetString(result);
        }

        #endregion

        #region 二进制

        /// <summary>
        /// 将对象转换成二进制(先序列化成json再压缩)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ToBinary(this object input)
        {
            var jsonStr = input.ToJson();
            if (!jsonStr.IsNullOrEmpty())
            {
                return jsonStr.LZ4Compress();
            }
            return null;
        }

        /// <summary>
        /// 从byte数组还原成对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T FromBinary<T>(this byte[] data)
        {
            var jsonStr = data.LZ4Decompress();
            if (!jsonStr.IsNullOrEmpty())
            {
                return jsonStr.FromJson<T>();
            }
            return default(T);
        }

        #endregion

        #endregion

    }
}
