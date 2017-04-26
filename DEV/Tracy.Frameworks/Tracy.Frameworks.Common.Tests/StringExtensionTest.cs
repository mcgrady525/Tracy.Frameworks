using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Tracy.Frameworks.Common.Extends;
using Tracy.Frameworks.Common.Helpers;

namespace Tracy.Frameworks.Common.Tests
{
    [NUnit.Framework.TestFixture]
    public class StringExtensionTest
    {
        [NUnit.Framework.Test]
        public void StringExtension_ToJson_Test()
        {
            var user = this.GetUser();
            var result = user.ToJson(dateTimeFormat: "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// xml序列化
        /// </summary>
        [Test]
        public void StringExtension_ToXml_Test()
        {
            var user = GetUser();
            var result = user.ToXml();
        }

        /// <summary>
        /// xml反序列化
        /// </summary>
        [Test]
        public void StringExtension_FromXml_Test()
        {
            var user = GetUser();
            var result = user.ToXml(isOmitXmlDeclaration: true, isOmitDefaultNamespaces: true, isNeedFormat: true);

            var user1 = result.FromXml<User>();

        }

        [NUnit.Framework.Test]
        public void StringExtension_ToJson_ByJavascriptSerializer_Test()
        {
            var user = this.GetUser();
            var jsSerializer = new JavaScriptSerializer();
            var result = jsSerializer.Serialize(user);
        }

        [NUnit.Framework.Test]
        public void StringExtension_Clone_Test()
        {
            //test深拷贝和浅拷贝
            var user = GetUser();//user.Name= "zhangsan"
            var deepUser = user.DeepClone();
            var shallowUser = user.ShallowClone();

            //
            deepUser.Name = "lisi";
            deepUser.Department.Name = "departaaa";

            //
            shallowUser.Name = "mcgrady";
            shallowUser.Department.Name = "departbbb";

        }

        [NUnit.Framework.Test]
        public void StringExtension_ToBool_Test()
        {
            var result1 = "Y".ToBoolNew();
            var result2 = "1".ToBoolNew();
            var result3 = "true".ToBoolNew();
            var result4 = "True".ToBoolNew();
            var result5 = "0".ToBool();
        }

        [NUnit.Framework.Test]
        public void TestMethod1()
        {
            var input = "aaa";

            var f = input.ToFloat();

            var d = input.ToDouble();

            var result1 = "".Trim();

            var result2 = DateTime.MinValue;

            var result3 = 0 % 2;

            var result4 = "********".ToDateTime(DateTime.MinValue);


        }

        [NUnit.Framework.Test]
        public void TestMethod2()
        {
            //生成一个随机数
            //1，GUID
            //2，时间戳
            //3，随机数

            //1
            var r1 = Guid.NewGuid().ToString();
            var r2 = Guid.NewGuid().ToString("N");

            //2
            var r4 = DateTime.Now.Ticks;

            //3
            Random r = new Random();
            var r3 = r.Next();


        }

        [NUnit.Framework.Test]
        public void TestMethod3()
        {
            //测试byte[]
            //"aaa"
            //""或string.Empty
            var b1 = string.Empty.LZ4Compress();
            if (b1 != null && b1.Length > 0)
            {
                var r1 = b1.LZ4Decompress();
            }
        }

        [NUnit.Framework.Test]
        public void TestMethod4()
        {
            //测试CacheHelper
            var key1 = "key1";
            var key2 = "key2";

            var user1 = CacheHelper.Get(key1) as List<User>;
            if (user1 == null)
            {
                user1 = new List<User> { GetUser() };
                CacheHelper.Set(key1, user1, DateTime.UtcNow.AddSeconds(10));
            }

            //测试过期时间
            user1 = CacheHelper.Get(key1) as List<User>;

            var user2 = CacheHelper.Get(key2) as List<User>;
            if (user2 == null)
            {
                user2 = new List<User> { GetUser() };
                CacheHelper.Set(key2, user2, DateTime.UtcNow.AddSeconds(60));
            }

            //clear all
            CacheHelper.RemoveAll();

            var user11 = CacheHelper.Get(key1) as List<User>;
            var user22 = CacheHelper.Get(key2) as List<User>;
        }

        [NUnit.Framework.Test]
        public void TestMethod5()
        {
            //测试扩展方法EqualsIgnoreCase
            var b1 = "aaa".EqualsIgnoreCase("aaa");
            var b2 = "aaa".EqualsIgnoreCase("AAA");
            var b3 = "aaa".EqualsIgnoreCase("aAa");
            var b4 = "aaa".EqualsIgnoreCase("abc");


        }




        private User GetUser()
        {
            return new User
            {
                Id = 1,
                Name = "zhangsan",
                Age = 31,
                CreateTime = new DateTime(2016, 5, 5, 15, 27, 27),
                Department = new Department
                {
                    Id = 0,
                    Name = "department111"
                }
            };
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreateTime { get; set; }

        public Department Department { get; set; }

        /// <summary>
        /// 浅克隆
        /// </summary>
        /// <returns></returns>
        public User ShallowClone()
        {
            return this.MemberwiseClone() as User;
        }
    }

    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
