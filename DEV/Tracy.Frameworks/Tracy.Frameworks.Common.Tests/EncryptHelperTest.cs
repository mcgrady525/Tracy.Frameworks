using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tracy.Frameworks.Common.Helpers;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Common.Tests
{
    [TestFixture]
    public class EncryptHelperTest
    {
        private static readonly string clearTextPwd = "123456";

        [Test]
        public void MD5With32bit_Test()
        {
            //MD5With32bit= MD5
            var result = EncryptHelper.MD5With32bit(clearTextPwd);
            var result1 = EncryptHelper.MD5(clearTextPwd);
        }

        [Test]
        public void DESEncrypt_Test()
        { 
            //加密
            var result1 = EncryptHelper.DESEncrypt(clearTextPwd);

            //解密
            var result2 = EncryptHelper.DESDecrypt(result1);

            Assert.AreEqual(result2, clearTextPwd);
        }

    }
}
