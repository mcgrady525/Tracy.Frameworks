using System;
using System.Security.Cryptography;
using System.Text;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Common.Tests
{
    /// <summary>
    /// 1，保存用户密码
    /// 2，保存登录的token
    /// 3，一般的依据一些规则生成的key
    /// </summary>
    [NUnit.Framework.TestFixture]
    public class SecurityExtensionTest
    {
        [NUnit.Framework.Test]
        public void SavePwd_Test()
        {
            System.Diagnostics.Debug.WriteLine("start...");

            System.Diagnostics.Debug.WriteLine("end");

        }

        [NUnit.Framework.Test]
        public void TestMD5()
        {
            var input = "admin";

            var result1 = input.To16bitMD5();
            var result2 = input.To32bitMD5();
            var result4 = GetMD5String(input);

        }

        /// <summary>
        /// MD5加密字符串
        /// </summary>
        public static string GetMD5String(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.Default.GetBytes(str);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < md5data.Length - 1; i++)
            {
                builder.Append(md5data[i].ToString("X2"));
            }
            return builder.ToString();
        }

    }
}
