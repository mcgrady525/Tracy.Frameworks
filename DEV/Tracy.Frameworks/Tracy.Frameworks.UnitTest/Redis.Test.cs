using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracy.Frameworks.Redis;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.UnitTest
{
    /// <summary>
    /// redis测试
    /// </summary>
    [TestFixture]
    public class Redis
    {
        [Test]
        public void Test_String()
        {
            var redisClient = new RedisWrapper();

            //get
            var name = redisClient.Get("name");

            if (name.IsNullOrEmpty())
            {
                redisClient.GetOrAdd("name",()=> { return "kobe"; });
            }

            //add
            redisClient.Add("","");

        }


    }
}
