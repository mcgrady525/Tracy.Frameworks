using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracy.Frameworks.Common.Helpers;

namespace Tracy.Frameworks.Common.Tests
{
    [TestFixture]
    public class HttpHelperTest
    {
        [Test]
        public void SendRequestByHttpWebRequest_Test()
        {
            var url = "http://www.baidu.com";
            var data = "{\"user\":\"test\"," +
                  "\"password\":\"bla\"}";
            var result= HttpHelper.SendRequestByHttpWebRequest(url, data);
        }

        [Test]
        public void Test_GetIP()
        {
            var result = HttpHelper.GetLocalIP();
            var result1 = HttpHelper.GetClientIP();
        }

    }
}
