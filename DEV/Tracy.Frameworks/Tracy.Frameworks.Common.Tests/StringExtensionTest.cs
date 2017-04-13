using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Tracy.Frameworks.Common.Extends;

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
            var result = user.ToXml(isOmitXmlDeclaration:true, isOmitDefaultNamespaces:true, isNeedFormat:true);

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
                    Id= 0,
                    Name= "department111"
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
