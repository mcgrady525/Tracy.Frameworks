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

        [NUnit.Framework.Test]
        public void StringExtension_ToJson_ByJavascriptSerializer_Test()
        {
            var user = this.GetUser();
            var jsSerializer = new JavaScriptSerializer();
            var result = jsSerializer.Serialize(user);
        }

        private User GetUser()
        {
            return new User
            {
                Id = 1,
                Name = "zhangsan",
                Age = 31,
                CreateTime = new DateTime(2016, 5, 5, 15, 27, 27)
            };
        }

    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
