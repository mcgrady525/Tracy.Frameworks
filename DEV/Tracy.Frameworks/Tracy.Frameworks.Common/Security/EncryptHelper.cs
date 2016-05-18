using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Tracy.Frameworks.Common.Configuration;

namespace Tracy.Frameworks.Common.Security
{
    /// <summary>
    /// Description:加密解密辅助类
    /// Author:McgradyLu
    /// Time:11/9/2013 1:10:21 PM
    /// </summary>
    public class EncryptHelper
    {
        public static string DESKey
        {
            get { return AppConfigHelper.GetAppSetting("DESKey"); }
        }

        #region 可逆加密
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        /// <summary>
        /// DES加密 [用户自定密钥]
        /// </summary>
        /// <param name="encryptKey"></param>
        /// <param name="encryptStr"></param>
        /// <returns></returns>
        public static string EncryptDES(string encryptKey, string encryptStr)
        {
            byte[] rgbKey = null, rgbIV = null, InputByteArray = null;
            DESCryptoServiceProvider dCSP = null;
            MemoryStream mStream = null;
            CryptoStream cStream = null;
            string result = "";

            try
            {
                rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                rgbIV = Keys;
                InputByteArray = Encoding.UTF8.GetBytes(encryptStr);
                dCSP = new DESCryptoServiceProvider();
                mStream = new MemoryStream();
                cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(InputByteArray, 0, InputByteArray.Length);
                cStream.FlushFinalBlock();
                result = Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                //LogHelper.Error("encrypt occur error!", ex);
                throw ex;
            }
            finally
            {
                rgbKey = null;
                rgbIV = null;
                InputByteArray = null;
                dCSP = null;
                mStream.Close();
                cStream.Dispose();
            }
            return result;
        }

        /// <summary>
        /// DES解密 [用户自定密钥]
        /// </summary>
        /// <param name="decryptKey"></param>
        /// <param name="decryptStr"></param>
        /// <returns></returns>
        public static string DecryptDES(string decryptKey, string decryptStr)
        {
            byte[] rgbKey = null, rgbIV = null, InputByteArray = null;
            DESCryptoServiceProvider dCSP = null;
            MemoryStream mStream = null;
            CryptoStream cStream = null;
            string result = "";

            try
            {
                rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                rgbIV = Keys;
                InputByteArray = Convert.FromBase64String(decryptStr);
                dCSP = new DESCryptoServiceProvider();
                mStream = new MemoryStream();
                cStream = new CryptoStream(mStream, dCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(InputByteArray, 0, InputByteArray.Length);
                cStream.FlushFinalBlock();
                result = Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                //LogHelper.Error("decrypt occur error!", ex);
                throw ex;
            }
            finally
            {
                rgbKey = null;
                rgbIV = null;
                InputByteArray = null;
                dCSP = null;
                mStream.Close();
                cStream.Dispose();
            }
            return result;
        }

        /// <summary>
        /// DES加密 [使用框架密钥]
        /// </summary>
        /// <param name="encryptStr"></param>
        /// <returns></returns>
        public static string EncryptDES_ByCustom(string encryptStr)
        {
            string encryptKey = DESKey;
            byte[] rgbKey = null, rgbIV = null, InputByteArray = null;
            DESCryptoServiceProvider dCSP = null;
            MemoryStream mStream = null;
            CryptoStream cStream = null;
            string result = "";

            try
            {
                rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                rgbIV = Keys;
                InputByteArray = Encoding.UTF8.GetBytes(encryptStr);
                dCSP = new DESCryptoServiceProvider();
                mStream = new MemoryStream();
                cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(InputByteArray, 0, InputByteArray.Length);
                cStream.FlushFinalBlock();
                result = Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rgbKey = null;
                rgbIV = null;
                InputByteArray = null;
                dCSP = null;
                mStream.Close();
                cStream.Dispose();
            }

            return result;

        }

        /// <summary>
        /// DES解密 [使用框架密钥]
        /// </summary>
        /// <param name="decryptStr"></param>
        /// <returns></returns>
        public static string DecryptDES_ByCustom(string decryptStr)
        {
            string decryptKey = DESKey;
            byte[] rgbKey = null, rgbIV = null, InputByteArray = null;
            DESCryptoServiceProvider dCSP = null;
            MemoryStream mStream = null;
            CryptoStream cStream = null;
            string result = "";

            try
            {
                rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                rgbIV = Keys;
                InputByteArray = Convert.FromBase64String(decryptStr);
                dCSP = new DESCryptoServiceProvider();
                mStream = new MemoryStream();
                cStream = new CryptoStream(mStream, dCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(InputByteArray, 0, InputByteArray.Length);
                cStream.FlushFinalBlock();
                result = Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rgbKey = null;
                rgbIV = null;
                InputByteArray = null;
                dCSP = null;
                mStream.Close();
                cStream.Dispose();
            }

            return result;

        }
        #endregion

        #region 不可逆加密

        /// <summary>
        /// SHA加密，不可逆 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SHA1(string source)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");
        }

        /// <summary>
        /// MD5加密，不可逆  
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5(string source)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5");
        }

		/// <summary>
		/// 获取MD5加密字符串【常用】
		/// </summary>
		/// <param name="str">源字符串</param>
		/// <returns>加密后的字符串</returns>
		public static string GetMD5String(string str) {
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] data = System.Text.Encoding.Default.GetBytes(str);
			byte[] md5data = md5.ComputeHash(data);
			md5.Clear();

			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < md5data.Length - 1; i++) {
				builder.Append(md5data[i].ToString("X2"));
			}
			return builder.ToString();
		}

        #endregion
    }
}
