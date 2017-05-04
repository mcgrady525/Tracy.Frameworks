using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tracy.Frameworks.Common.Extends;
using Tracy.Frameworks.Common.Helpers;
using System.Diagnostics;
using EmitMapper;
using EmitMapper.MappingConfiguration;

namespace Tracy.Frameworks.UnitTest
{
    /// <summary>
    /// Tracy.Frameworks.Common的单元测试类
    /// </summary>
    [TestFixture]
    public class CommonTest
    {
        [Test]
        public void Test_Extends_ToDataTable()
        {
            var list = new List<UserFrom>();
            list.Add(new UserFrom
            {
                Name = "mcgradylu",
                Age = 20
            });
            list.Add(new UserFrom
            {
                Name = "kobe",
                Age = 22
            });

            var dt = list.ToDataTable();
        }

        [Test]
        public void Test_Extends_Dto()
        {
            //默认
            var userFrom = new UserFrom { Name = "zhangsan", Age = 20 };
            //var result = userFrom.ToDto<UserFrom, UserTo>();

            //属性名字不一样的
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<UserFrom, UserTo1>(new DefaultMapConfig()
                .MatchMembers((x, y) =>
                {
                    if (x == "Age" && y == "UserAge")
                    {
                        return true;
                    }
                    return x == y;
                }));
            var result1 = mapper.Map(userFrom);
        }

    }

    

    public class UserFrom
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class UserTo
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class UserTo1
    {
        public string Name { get; set; }

        public int UserAge { get; set; }
    }
}
