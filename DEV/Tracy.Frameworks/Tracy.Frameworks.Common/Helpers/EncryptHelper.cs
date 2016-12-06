using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Tracy.Frameworks.Common.Helpers
{
    /// <summary>
    /// 加解密helper
    /// </summary>
    public class EncryptHelper
    {
        #region 单向加密

        #region MD5

        /// <summary>
        /// MD5加密，16位小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5With16bit(string input)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                string result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input)), 4, 8);
                return result.Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// MD5加密，32位小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5With32bit(string input)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                string result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input)));
                return result.Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// MD5加密，ubtrip
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5(string input)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            var b = Encoding.UTF8.GetBytes(input);
            b = md5.ComputeHash(b);
            string result = null;
            for (int i = 0; i <= b.Length - 1; i++)
            {
                result += b[i].ToString("x").PadLeft(2, '0'); ;
            }
            md5 = null;
            return result;
        }

        #endregion

        #region SHA

        /// <summary>
        /// ToSHA1
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SHA1(string input)
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
        public static string SHA512(string input)
        {
            using (var sha = new SHA512Managed())
            {
                string result = BitConverter.ToString(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(input)));
                return result.Replace("-", "").ToLower();
            }
        }

        #endregion

        #endregion

        #region 对称加密

        #region DES

        //DES+CBC
        private const string DefaultKey = "s6m5v4a1";
        private const string DefaultIV = "7y2e3h89";

        /// <summary>
        /// 生成随机的Key和初始化向量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public static void DESGenerateKeyIV(out string key, out string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.GenerateKey();
            des.GenerateIV();
            key = Encoding.UTF8.GetString(des.Key);
            iv = Encoding.UTF8.GetString(des.IV);
        }

        /// <summary>
        /// 采用DES算法进行加密，使用默认的key, iv及密码模式(CBC)
        /// </summary>
        /// <param name="input">将要加密的文本</param>
        /// <returns>加密结果的Base64编码串</returns>
        public static string DESEncrypt(string input)
        {
            return DESEncrypt(input, DefaultKey, DefaultIV, CipherMode.CBC);
        }

        /// <summary>
        /// 采用DES算法进行解密，使用默认的key, iv及密码模式(CBC)
        /// </summary>
        /// <param name="input">加密结果的Base64编码串</param>
        /// <returns>解密之后的原始文本</returns>
        public static string DESDecrypt(string input)
        {
            return DESDecrypt(input, DefaultKey, DefaultIV, CipherMode.CBC);
        }

        /// <summary>
        /// 采用DES算法进行加密，并指定key, iv及密码模式
        /// </summary>
        /// <param name="input">将要加密的文本</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <param name="cipherMode">指定用于加密的块密码模式, .Net默认情况下指定为CBC, 如果加密模式为ECB则iv无效，指定与key相同的值即可</param>
        /// <returns>加密结果的Base64编码串</returns>
        public static string DESEncrypt(string input, string key, string iv, CipherMode cipherMode)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key不能为空且至少为8位");
            if (iv == null || iv.Length < 8) throw new ArgumentException("iv不能为空且至少为8位");

            byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] rgbIV = Encoding.UTF8.GetBytes(iv.Substring(0, 8));
            byte[] inputByteArray = Encoding.UTF8.GetBytes(input);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = cipherMode;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string result = Convert.ToBase64String(ms.ToArray());

            ms.Close();
            cs.Close();
            return result;
        }

        /// <summary>
        /// 采用DES算法进行解密，并指定key, iv及密码模式
        /// </summary>
        /// <param name="input">加密结果的Base64编码串</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始化向量</param>
        /// <param name="cipherMode">指定用于加密的块密码模式, .Net默认情况下指定为CBC，如果加密模式为ECB则iv无效，指定与key相同的值即可</param>
        /// <returns>解密之后的原始文本</returns>
        public static string DESDecrypt(string input, string key, string iv, CipherMode cipherMode)
        {
            if (key == null || key.Length < 8) throw new ArgumentException("key不能为空且至少为8位");
            if (iv == null || iv.Length < 8) throw new ArgumentException("iv不能为空且至少为8位");

            byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] rgbIV = Encoding.UTF8.GetBytes(iv.Substring(0, 8));
            byte[] inputByteArray = Convert.FromBase64String(input);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Mode = cipherMode;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string result = Encoding.UTF8.GetString(ms.ToArray());

            ms.Close();
            cs.Close();
            return result;
        }

        #endregion

        #endregion

    }
}
