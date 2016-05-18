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
    /// 描述:字符串扩展类
    /// 作者:鲁宁
    /// 时间:2013/10/14 17:24:45
    /// </summary>
    public static class StringExtension
    {
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
            else {
                return defaultValue;
            }
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
            else {
                return defaultValue;
            }
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
            else {
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

        #region ToBool

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
            else {
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

        #region ToDouble
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

        #region OtherTransform

        /// <summary>
        /// 用指定連接子連接字串清單所有元素
        /// </summary>
        /// <param name="input">字串清單</param>
        /// <param name="separator">連接子</param>
        /// <returns></returns>
        public static string ToString(this List<string> input, string separator)
        {
            if (input == null)
            {
                return string.Empty;
            }
            return string.Join(separator, input);
        }

        /// <summary>
        /// 數字格式轉換顯示
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToString(this Object input, string n)
        {
            if (input == null)
            {
                return string.Empty;
            }
            return input.ToString().ToDecimal().ToString(n);
        }

        /// <summary>
        /// 轉成一般常用格式 yyyy-MM-dd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringNormal(this DateTime input)
        {
            return input.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 轉成一般常用格式 yyyy-MM-dd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToStringNormal(this Object input)
        {
            if (null != input)
            {
                if (input.ToString().IsDateTime())
                {
                    return input.ToString().ToDateTime().ToString("yyyy-MM-dd");
                }
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SafeTrim(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return "";
            }
            else {
                return input.Trim();
            }
        }
        /// <summary>
        /// 取得網站的RootURL,結果有加"/"
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetRootURL(this Uri input)
        {
            if (null == input)
                throw new ArgumentNullException("input");
            return input.AbsoluteUri.Replace(input.PathAndQuery, "/");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hasPathChar">是否在末尾帶"/"</param>
        /// <returns></returns>
        public static string GetApplicationURL(this HttpRequest input, bool hasPathChar = true)
        {
            if (null == input)
                throw new ArgumentNullException("input");
            if (hasPathChar)
            {
                return input.Url.AbsoluteUri.Replace(input.Url.PathAndQuery, input.ApplicationPath.TrimEnd('/') + "/");
            }
            else {
                return input.Url.AbsoluteUri.Replace(input.Url.PathAndQuery, input.ApplicationPath.TrimEnd('/'));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="hasPathChar"></param>
        /// <returns></returns>
        public static string GetApplicationUrl(this HttpRequestBase input, bool hasPathChar = true)
        {
            if (null == input)
                throw new ArgumentNullException("input");
            if (hasPathChar)
            {
                return input.Url.AbsoluteUri.Replace(input.Url.PathAndQuery, input.ApplicationPath.TrimEnd('/') + "/");
            }
            else {
                return input.Url.AbsoluteUri.Replace(input.Url.PathAndQuery, input.ApplicationPath.TrimEnd('/'));
            }
        }

        /// <summary>
        /// 只從QueryString或Form裡讀內容
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Form2(this HttpRequest input, string key)
        {
            if (null == input)
                throw new ArgumentNullException("input");
            string str = input.QueryString[key];
            if (str != null)
            {
                return str;
            }
            str = input.Form[key];
            if (str != null)
            {
                return str;
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// 在後邊取多少個字元
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Right(this string input, int length)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= length)
            {
                return input;
            }
            else {
                return input.Substring(input.Length - length);
            }
        }

        /// <summary>
        /// 在左邊取多少個字元
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Left(this string input, int length)
        {
            if (string.IsNullOrEmpty(input) || input.Length <= length)
            {
                return input;
            }
            else {
                return input.Substring(0, length);
            }
        }

        #endregion

        #region Enum

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
            else {
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
            else {
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
            else {
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

        #region IPAddress
        /// <summary>
        /// 如果轉換失敗，返回 IPAddress.None
        /// </summary>
        /// <param name="ipstring"></param>
        /// <returns></returns>
        public static IPAddress ToIPAddress(this string ipstring)
        {
            IPAddress ip;
            if (IPAddress.TryParse(ipstring, out ip))
                return ip;
            else
                return IPAddress.None;
        }
        #endregion

        #region DateTime
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
            else {
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
            else {
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

        /// <summary>
        /// 是否為日期型字串
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
        #endregion

        #region ToTimeSpan
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
            else {
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

        #region Guid
        /// <summary>
        ///
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

        #region Url

        /// <summary>
        /// 從 URL 中取出鍵的值, 如果不存在,返回空
        /// </summary>
        /// <param name="s"></param>
        /// <param name="key"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string ParseString(this string s, string key, bool ignoreCase)
        {
            if (s == null)
                return ""; //必須這樣,請不要修改

            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            Dictionary<string, string> kvs = s.ParseString(ignoreCase);
            key = ignoreCase ? key.ToLower() : key;
            if (kvs.ContainsKey(key))
            {
                return kvs[key];
            }
            return "";
        }

        /// <summary>
        /// 從URL中取 Key / Value
        /// </summary>
        /// <param name="s"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseString(this string s, bool ignoreCase)
        {
            //必須這樣,請不要修改
            if (string.IsNullOrEmpty(s))
            {
                return new Dictionary<string, string>();
            }

            if (s.IndexOf('?') != -1)
            {
                s = s.Remove(0, s.IndexOf('?'));
            }

            Dictionary<string, string> kvs = new Dictionary<string, string>();
            Regex reg = new Regex(@"[\?&]?(?<key>[^=]+)=(?<value>[^\&]*)", RegexOptions.Compiled | RegexOptions.Multiline);
            MatchCollection ms = reg.Matches(s);
            string key;
            foreach (Match ma in ms)
            {
                key = ignoreCase ? ma.Groups["key"].Value.ToLower() : ma.Groups["key"].Value;
                if (kvs.ContainsKey(key))
                {
                    kvs[key] += "," + ma.Groups["value"].Value;
                }
                else {
                    kvs[key] = ma.Groups["value"].Value;
                }
            }

            return kvs;
        }

        /// <summary>
        /// 設置 URL中的 key
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string SetUrlKeyValue(this string url, string key, string value, Encoding encode)
        {
            if (url == null)
                url = "";
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            if (value == null)
                value = "";
            if (null == encode)
                throw new ArgumentNullException("encode");
            //if (!string.IsNullOrEmpty(url.ParseString(key, true).Trim())) {
            if (url.ParseString(true).ContainsKey(key.ToLower()))
            {
                Regex reg = new Regex(@"([\?\&])(" + key + @"=)([^\&]*)(\&?)", RegexOptions.IgnoreCase);
                //　如果 value 前几个字符是数字，有BUG
                //return reg.Replace(url, "$1$2" + HttpUtility.UrlEncode(value, encode) + "$4");

                return reg.Replace(url, (ma) => {
                    if (ma.Success)
                    {
                        //var a = string.Format("{0}{1}{2}{3}", ma.Groups[1].Value, ma.Groups[2].Value, HttpUtility.UrlEncode(value, encode), ma.Groups[4].Value);
                        return string.Format("{0}{1}{2}{3}", ma.Groups[1].Value, ma.Groups[2].Value, HttpUtility.UrlEncode(value, encode), ma.Groups[4].Value);
                    }
                    return "";

                });

            }
            else {
                return url + (url.IndexOf('?') > -1 ? "&" : "?") + key + "=" + HttpUtility.UrlEncode(value, encode);
            }
        }


        /// <summary>
        /// 修正URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string FixUrl(this string url)
        {
            return url.FixUrl("");
        }

        /// <summary>
        /// 修正URL
        /// </summary>
        /// <param name="url"></param>
        /// <param name="defaultPrefix"></param>
        /// <returns></returns>
        public static string FixUrl(this string url, string defaultPrefix)
        {
            // 必須這樣,請不要修改
            if (url == null)
                url = "";

            if (defaultPrefix == null)
                defaultPrefix = "";
            string tmp = url.Trim();
            if (!Regex.Match(tmp, "^(http|https):").Success)
            {
                tmp = string.Format("{0}/{1}", defaultPrefix, tmp);
            }
            tmp = Regex.Replace(tmp, @"(?<!(http|https):)[\\/]+", "/").Trim();
            return tmp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToAbsolute(string url)
        {
            return VirtualPathUtility.ToAbsolute(url);
        }

        #endregion

        #region mix
        /// <summary>
        /// 獲取用於 Javascript 的安全字串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string JsSafeString(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return "";//必須這樣,請不要修改

            return str.ToString().Replace("'", "\\'").Replace("\"", "&quot;");
        }

        /// <summary>
        /// 獲取潔淨的線上CKEditor編輯器內容
        /// </summary>
        /// <param name="content">源內容</param>
        /// <returns></returns>
        public static string GetCleanEditorContent(this string content)
        {
            if (String.IsNullOrEmpty(content))
                throw new ArgumentNullException("content");
            content = Regex.Replace(content.Replace("\r\n", "").Replace('\t', ' '), ">(( )+|[\\s　?]+|\\s+)", ">", RegexOptions.Multiline);
            content = Regex.Replace(content, "<div firebugversion=\"1.5.3\" id=\"_firebugConsole\" style=\"display: none;\">&nbsp;</div>$", "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "<p>&nbsp;</p>$", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            content = Regex.Replace(content, "<br />$", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return content;
        }

        /// <summary>
        /// 替換以 ../$ ../../$ 等
        /// 以 ../VMaster/$ ../../VMaster/$ VMaster/$ 等
        /// 開頭的 src, href
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="realSite"></param>
        /// <param name="placeHolder"></param>
        /// <returns></returns>
        public static string ReplaceResourceSite(this string ctx, string realSite, string placeHolder = @"\$")
        {
            if (ctx == null)
            {
                throw new ArgumentNullException("ctx");
            }

            //Regex rx = new Regex(@"(<(a|img|script|link)[^>]*(src|href)=(?<quot>[""']))(\.\./)*(VMaster/)*\$(?<sub>[^'""]*)\6", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex rx = new Regex(@"(<(a|img|script|link)[^>]*(src|href)=(?<quot>[""']))(\.\./)*(VMaster/)*" + placeHolder + @"(?<sub>[^'""]*)\6", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var outputHtml = rx.Replace(ctx, (ma) => {
                if (ma.Success)
                {
                    return ma.Groups[1] + ma.Groups["sub"].Value.FixUrl(realSite) + ma.Groups["quot"].Value;
                }
                return ma.Groups[0].Value;
            });
            return outputHtml;
        }
        #endregion

        #region 如果空字串則轉為null
        /// <summary>
        /// treat empty string to null
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EmptyThenNull(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            return input;
        }

        /// <summary>
        /// 沒值時返回null，有值時返回input.Trim()
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToNull(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }
            return input.Trim();
        }

        /// <summary>
        /// 沒值時返回string.Empty，有值時返回input.Trim()
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToEmpty(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            return input.Trim();
        }

        /// <summary>
        /// 比较字符串是否相等，而不考虑大小写
        /// source.Equals(value, StringComparison.OrdinalIgnoreCase);
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string source, string value)
        {
            return source.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

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

        #region 处理t-sql中插入的值，过滤特殊字符

        /// <summary>
        /// 处理t-sql中插入的值，防止意外字符导致错误
        /// </summary>
        /// <param name="str">需要插入的参数值</param>
        /// <returns></returns>
        public static string ToSecuritySQL(this string str)
        {
            return str.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]").Replace("^", "[^]");
        }

        #endregion

        #region 序列化与反序列化
        #region JSON序列化，反序列化 {1AC5559B-3615-4EA6-B15B-B6374B28D7A1}
        /// <summary>
        /// 对象序列化成json字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isNeedFormat">默认false</param>
        /// <param name="isCanCyclicReference">默认true,生成的json每个对象会自动加上类似 "$id":"1","$ref": "1"</param>
        /// <param name="dateTimeFormat">默认null,即使用json.net默认的序列化机制，如："\/Date(1439335800000+0800)\/"</param>
        /// <param name="typeNameHandling">默认null,序列化时指定typename处理方式。None = 0,Objects = 1,Arrays = 2,All = 3,Auto = 4.对应的枚举是Newtonsoft.Json.TypeNameHandling</param>
        /// <param name="ignoreNullValue">值为null的不进行序列化（比如某个属性的值为null，序列化后没有此属性存在)</param>
        /// <returns></returns>
        public static string ToJson(this object input, bool isNeedFormat = false, bool isCanCyclicReference = true,
                                    string dateTimeFormat = null, int? typeNameHandling = null, bool ignoreNullValue = true)
        {
            var settings = new JsonSerializerSettings();
            settings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;//兼容<=4.5版本，默认序列化成微软的datetime json格式，e.g. "\/Date(1198908717056+0800)\/"，如果要输出ISO标准时间，可以通过dateTimeFormat进行设置。

            if (ignoreNullValue)
            {
                settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            }

            if (typeNameHandling.HasValue)
            {
                settings.TypeNameHandling = typeNameHandling.ToEnum<TypeNameHandling>();
                settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            }

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

            var format = isNeedFormat ? Formatting.Indented : Formatting.None;

            var json = JsonConvert.SerializeObject(input, format, settings);
            return json;
        }

        /// <summary>
        /// json字符串反序列化为对象
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
        /// json字符串反序列化为对象
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

        #region Xml 序列化，反序列化

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj, Action<Exception> onError = null)
        {
            try
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var namespaces = new XmlSerializerNamespaces();
                    namespaces.Add("", "");
                    serializer.Serialize(stream, obj, namespaces);
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
        /// 反序列化
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
        
        #region 压缩与解压 {4800C066-A10B-40A7-B6BA-B9F91CCE8DE6}

        /// <summary>
        /// 字符串压缩(GZip)
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
        /// 字符串解压(GZip)
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

        /// <summary>
        /// 字符串压缩(LZ4)
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
        /// 字符串解压(LZ4)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string LZ4Decompress(this byte[] data)
        {
            var result = LZ4.LZ4Codec.Unwrap(data);
            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// 对象转换binary
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
        /// binary转换对象
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
    }

    /// <summary>
    /// {4800C066-A10B-40A7-B6BA-B9F91CCE8DE6}
    /// </summary>
    public static class MySQLCompressHelper
    {
        public static byte[] MySQLCompress(this string str, Ionic.Zlib.CompressionLevel level = Ionic.Zlib.CompressionLevel.Default)
        {
            return UTF8Encoding.UTF8.GetBytes(str).MySQLCompress(level);
        }

        public static byte[] MySQLCompress(this byte[] buffer, Ionic.Zlib.CompressionLevel level = Ionic.Zlib.CompressionLevel.Default)
        {
            using (var output = new MemoryStream())
            {
                output.Write(BitConverter.GetBytes((int)buffer.Length), 0, 4);
                using (var compressor = new Ionic.Zlib.ZlibStream(output, Ionic.Zlib.CompressionMode.Compress, level))
                {
                    compressor.Write(buffer, 0, buffer.Length);
                }

                return output.ToArray();
            }
        }

        public static string MySQLUncompress(this byte[] buffer)
        {
            return UTF8Encoding.UTF8.GetString(buffer.MySQLUncompressBuffer());
        }

        public static byte[] MySQLUncompressBuffer(this byte[] buffer)
        {
            using (var output = new MemoryStream())
            {
                using (var decompressor = new Ionic.Zlib.ZlibStream(output, Ionic.Zlib.CompressionMode.Decompress))
                {
                    decompressor.Write(buffer, 4, buffer.Length - 4);
                }

                return output.ToArray();
            }
        }
    }
}
