using System;
using Tracy.Frameworks.Redis;
using NUnit.Framework;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Redis.Tests
{
    [NUnit.Framework.TestFixture]
    public class RedisWrapperTest
    {
        private readonly IRedisWrapper redisWrapper;
        private static readonly string redisKey = "urn:ABS:Offline.Service:RedisTestKey:d4d963d7-1081-4ebb-8923-d12865719b84";//不压缩
        private static readonly string redisKeyByGZip = "urn:ABS:Offline.Service:RedisTestKey:d4d963d7-1081-4ebb-8923-d12865719b84" + "[GZip]";//GZip压缩
        private static readonly string redisKeyByLZ4 = "urn:ABS:Offline.Service:RedisTestKey:d4d963d7-1081-4ebb-8923-d12865719b84" + "[LZ4]";//LZ4压缩

        public RedisWrapperTest()
        {
            redisWrapper = RedisWrapperFactory.GetInstance(RedisGroupNames.AbsDefault);
        }


        [NUnit.Framework.Test]
        public void Redis_Wrapper_Set_BaseType_Test()
        {
            if (redisWrapper.Exists(redisKey))
            {
                redisWrapper.Remove(redisKey);
            }

            var result = redisWrapper.Set(redisKey, "1620", TimeSpan.FromMinutes(10));
            Assert.IsTrue(result);

        }

        [NUnit.Framework.Test]
        public void Redis_Wrapper_Set_ComplexType_Test()
        {
            //存在则先删除，再set
            if (redisWrapper.Exists(redisKey))
            {
                redisWrapper.Remove(redisKey);
            }

            var model = new User
            {
                ID = 1,
                Name = "zhangsan",
                Age= 30,
                Phone= "13155556666",
                CreateTime = DateTime.Now
            };
            var result = redisWrapper.Set(redisKey, model, TimeSpan.FromHours(8));
            Assert.IsTrue(result);
        }

        [NUnit.Framework.Test]
        public void Redis_Wrapper_Get_Test()
        {
            //基础类型
            //var r= redisWrapper.Get<string>(key);
            //r.Should().NotBeEmpty();

            ////自定义类型
            //var result = redisWrapper.Get<User>(redisKey);
            //Assert.IsNotNull(result);

            //var json = redisWrapper.GetJson(key);
            //json.Should().NotBeEmpty();

            //未压缩
            //GZip压缩
            //LZ4压缩
            try
            {
                //var result = redisWrapper.Get<User>(redisKey);
                //var jsonStr = "﻿[{\"172.18.23.135\"},{\"172.18.21.115\"}]";
                //var result= jsonStr.FromJson<System.Collections.Generic.List<string>>();
                var list = new System.Collections.Generic.List<string> { "1","2","3"};
                var jsonStr = list.ToJson();
                var result = jsonStr.FromJson<System.Collections.Generic.List<string>>();
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [NUnit.Framework.Test]
        public void Redis_Wrapper_GetTimeToLive_Test()
        {
            //key不存在:null= key过期:null
            //key没有设定有效时间:null
            //key在有效期内
            var key = "urn:ABS:Offline.Service:RedisTestKey:4d5cb714-8f6c-40a4-8f11-610789781873";
            var result = redisWrapper.GetTimeToLive(key);

        }

    }

    public class User
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public DateTime? CreateTime { get; set; }

    }
}
