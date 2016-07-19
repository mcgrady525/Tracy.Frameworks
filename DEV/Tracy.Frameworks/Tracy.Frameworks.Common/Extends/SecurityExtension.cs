using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/7/14 18:14:27
 * 描述说明：加密算法
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Extends
{
    #region 单向散列加密
    /// <summary>
    /// 通用加密
    /// </summary>
    public static class CommonSecurity
    {
        /// <summary>
        /// ToSHA1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSHA1(this string input)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                string result = BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input)));
                return result.Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 采用SHA512算法对字符串加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSHA512(this string input)
        {
            using (var sha = new SHA512Managed())
            {
                string result = BitConverter.ToString(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(input)));
                return result.Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 16位，低8位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string To16bitMD5(this string input)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                string result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input)), 4, 8);
                return result.Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// 32位小写[常用]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string To32bitMD5(this string input)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                string result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input)));
                return result.Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// MD5 按自己需要截取MD5的裡的相關部分
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startIndex">起始字元位置（從0開始）</param>
        /// <param name="length">長度</param>
        /// <returns></returns>
        public static string ToMD5(this string input, int startIndex, int length)
        {
            return input.To32bitMD5().Substring(startIndex, length);
        }
    }
    #endregion
}
