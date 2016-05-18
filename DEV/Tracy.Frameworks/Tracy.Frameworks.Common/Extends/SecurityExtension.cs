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
    /// <summary>
    /// 字符串加密用委托
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns>加密后输出</returns>
    public delegate string EncryptHandler(string input);

    #region 密码加密

    /// <summary>
    /// 
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="input">密码明文</param>
        /// <param name="method">插件加密算法</param>
        /// <returns></returns>
        public static string PasswordEncrypt(this string input, EncryptHandler method)
        {
            string pwd0 = method(input); //采用插件加密算法得到一次散列的原始码
            string salt = CommonSecurity.ToSHA512(pwd0); //采用SHA512对原始码散列得到盐值
            string pwd1 = EncryptWithSalt(pwd0, salt, method); //采用带盐值的插件加密算法对原始码再散列
            return pwd1;
        }

        /// <summary>
        /// 采用带盐值的插件加密算法进行加密
        /// </summary>
        /// <param name="input">密码原始值</param>
        /// <param name="salt">盐值</param>
        /// <param name="method">插件加密算法</param>
        /// <returns></returns>
        public static string EncryptWithSalt(this string input, string salt, EncryptHandler method)
        {
            string salted = salt.Trim(); //宣告變數,儲存Salted值 
            if (salted.Length <= 5)      //如果沒給Salt值或过短,那給預設 
            {
                salted = "ctr3536ip63sz4364";
            }
            return method(string.Concat(salted.Substring(0, 5), input, salted.Substring(5)));
        }
    }

    #endregion

    #region MD5加密

    /// <summary>
    ///
    /// </summary>
    public static class MD5Helper
    {
        /// <summary>
        /// MD5 16位元加密 加密後密碼為小寫
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5(this string input)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(input)), 4, 8);
                t2 = t2.Replace("-", "");
                t2 = t2.ToLower();
                return t2;
            }
        }

        /// <summary>
        /// MD5 32位元加密 加密後密碼為小寫
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5For32(this string input)
        {
            using (var md5Hasher = MD5.Create())
            {
                byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }

    #endregion

    #region 通用加密

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
                return result.Replace("-", "");
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
                return result.Replace("-", "");
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
                return result.Replace("-", "");
            }
        }

        /// <summary>
        /// 32位
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string To32bitMD5(this string input)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                string result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input)));
                return result.Replace("-", "");
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

    #region DES加密、解密
    /// <summary>
    /// DES加密解密
    /// </summary>
    public static class DES
    {
        /// <summary>
        /// 獲取金鑰
        /// </summary>
        private static string Key
        {
            get { return @"P@+#wG+Z"; }
        }

        /// <summary>
        /// 獲取向量
        /// </summary>
        private static string IV
        {
            get { return @"L%n67}G\Mk@k%:~Y"; }
        }


        /// <summary>
        /// DES加密
        /// 使用此類裡硬編碼的Key及IV
        /// </summary>
        /// <param name="input">明文字串</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(this string input)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV);

            return DESEncrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input">明文字串</param>
        /// <param name="key">金鑰</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(this string input, string key)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(key.ToMD5(0, 8));
            byte[] rgbIV = Encoding.UTF8.GetBytes(key.ToMD5(0, 16));

            return DESEncrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">金鑰</param>
        /// <param name="iv">向量</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(this string input, string key, string iv)
        {
            byte[] buffer = Convert.FromBase64String(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV);
            return DESEncrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rgbKey">金鑰</param>
        /// <param name="rgbIV">向量</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(this string input, byte[] rgbKey, byte[] rgbIV)
        {
            byte[] buffer = Convert.FromBase64String(input);
            return DESEncrypt(buffer, rgbKey, rgbIV);
        }

        private static string DESEncrypt(byte[] buffer, byte[] rgbKey, byte[] rgbIV)
        {
            string encrypt = null;
            using (var des = new DESCryptoServiceProvider())
            {
                try
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                        {
                            cStream.Write(buffer, 0, buffer.Length);
                            cStream.FlushFinalBlock();
                            encrypt = Convert.ToBase64String(mStream.ToArray());
                        }
                    }
                }
                catch { }
                des.Clear();
            }
            return encrypt;
        }

        /// <summary>
        /// DES解密
        /// 使用此類裡硬編碼的Key及IV
        /// </summary>
        /// <param name="input">密文字串</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(this string input)
        {
            byte[] buffer = Convert.FromBase64String(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV);

            return DESDecrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input">密文字串</param>
        /// <param name="key">金鑰</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(this string input, string key)
        {
            byte[] buffer = Convert.FromBase64String(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(key.ToMD5(0, 8));
            byte[] rgbIV = Encoding.UTF8.GetBytes(key.ToMD5(0, 16));

            return DESDecrypt(buffer, rgbKey, rgbIV);
        }


        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key">金鑰</param>
        /// <param name="iv">向量</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(this string input, string key, string iv)
        {
            byte[] buffer = Convert.FromBase64String(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV);
            return DESDecrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rgbKey">金鑰</param>
        /// <param name="rgbIV">向量</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(this string input, byte[] rgbKey, byte[] rgbIV)
        {
            byte[] buffer = Convert.FromBase64String(input);
            return DESDecrypt(buffer, rgbKey, rgbIV);
        }

        private static string DESDecrypt(byte[] buffer, byte[] rgbKey, byte[] rgbIV)
        {
            string decrypt = null;
            using (var des = new DESCryptoServiceProvider())
            {
                try
                {
                    using (MemoryStream mStream = new MemoryStream())
                    {
                        using (CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                        {
                            cStream.Write(buffer, 0, buffer.Length);
                            cStream.FlushFinalBlock();
                            decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                        }
                    }
                }
                catch { }
                des.Clear();
            }

            return decrypt;
        }
    }

    #endregion

    /// <summary>
    /// 加密幫助
    /// </summary>
    public static class AesHelper
    {
        #region
        /// <summary>
        /// 獲取金鑰
        /// </summary>
        private static string Key
        {
            get
            {
                return @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M";
            }
        }

        /// <summary>
        /// 獲取向量
        /// </summary>
        private static string IV
        {
            get
            {
                return @"L+\~f4,Ir)b$=pkf";
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="input">明文字串</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(this string input)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV);

            return AESEncrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="input">明文字串</param>
        /// <param name="key">金鑰</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(this string input, string key)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(key.Md5For32());
            byte[] rgbIV = Encoding.UTF8.GetBytes(key.Md5());

            return AESEncrypt(buffer, rgbKey, rgbIV);
        }

        internal static string AESEncrypt(byte[] buffer, byte[] rgbKey, byte[] rgbIV)
        {
            string encrypt = null;

            using (var mStream = new MemoryStream())
            {
                using (var aes = Rijndael.Create())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(buffer, 0, buffer.Length);
                        cStream.FlushFinalBlock();
                    }
                    aes.Clear();
                }
                encrypt = Convert.ToBase64String(mStream.ToArray());
            }

            return encrypt;
        }


        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="input">密文字串</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(this string input)
        {
            byte[] buffer = Convert.FromBase64String(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(Key);
            byte[] rgbIV = Encoding.UTF8.GetBytes(IV);

            return AESDecrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="input">密文字串</param>
        /// <param name="key">金鑰</param>
        /// <returns>明文</returns>
        public static string AESDecrypt(this string input, string key)
        {
            byte[] buffer = Convert.FromBase64String(input);
            byte[] rgbKey = Encoding.UTF8.GetBytes(key.Md5For32());
            byte[] rgbIV = Encoding.UTF8.GetBytes(key.Md5());

            return AESDecrypt(buffer, rgbKey, rgbIV);
        }

        /// <summary>
        /// AES 解密, 如果解密失敗,返回 defaultValue
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string AESDecrypt(this string input, string key, string defaultValue)
        {
            try
            {
                return input.AESDecrypt(key);
            }
            catch
            {
                return defaultValue;
            }
        }

        internal static string AESDecrypt(byte[] buffer, byte[] rgbKey, byte[] rgbIV)
        {
            string decrypt = null;
            using (var mStream = new MemoryStream())
            {
                using (var aes = Rijndael.Create())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(buffer, 0, buffer.Length);
                        cStream.FlushFinalBlock();
                    }
                    aes.Clear();
                }
                decrypt = Encoding.UTF8.GetString(mStream.ToArray());
            }

            return decrypt;
        }

        #endregion

    }

    #region REST 加密

    /// <summary>
    /// 
    /// </summary>
    public static class RESTCPHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetToken(this string input)
        {
            string result = string.Empty;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var ticks = DateTime.Now.ToFileTimeUtc().ToString();
                result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input))).Replace("-", "");
                result = result
                    .Replace("1", ticks.Substring(3, 1))
                    .Replace("9", ticks.Substring(4, 1))
                    .Replace("7", ticks.Substring(6, 1))
                    .Replace("4", ticks.Substring(2, 1))
                    .Replace("6", ticks.Substring(7, 1))
                    .Replace("3", ticks.Substring(12, 1));
                result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(result))).Replace("-", "");
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetDeviceINFO(this string input)
        {
            string result = input;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(input)));
                result = ModEncrypt(result.Replace("-", ""));
                result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(result)));
                result = ModEncrypt(result.Replace("-", ""));
                result = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(result)));
                result = ModEncrypt(result.Replace("-", ""));
            }
            return result;
        }

        /// <summary>
        /// 对数字/字母取对称值加密
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static string ModEncrypt(string encrypt)
        {
            if (string.IsNullOrEmpty(encrypt))
            {
                return string.Empty;
            }

            string result = string.Empty;
            foreach (char ch in encrypt)
            {
                if (ch >= '0' && ch <= '9')
                {
                    result += (char)('9' - ch + '0');
                }
                else if (ch >= 'a' && ch <= 'z')
                {
                    result += (char)('z' - ch + 'a');
                }
                else if (ch >= 'A' && ch <= 'Z')
                {
                    result += (char)('Z' - ch + 'A');
                }
                else
                {
                    result += ch;
                }
            }

            return result;
        }

        /// <summary>
        /// 獲取金鑰
        /// </summary>
        private static string Key
        {
            get
            {
                return @"U*(T>U??;&{W*+eGSF89-3.4,#R}AGT|";
            }
        }

        /// <summary>
        /// 獲取向量
        /// </summary>
        private static string IV
        {
            get
            {
                return @"JOoT*&~-2Ier3^WE";
            }
        }
    }

    #endregion
}
