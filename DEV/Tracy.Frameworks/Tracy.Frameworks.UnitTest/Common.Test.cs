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
using Tracy.Frameworks.UnitTest.Helper;

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

        [Test]
        public void Test_Serializable()
        {
            //测试Serializable对序列化的影响，将对象序列化成字符串
            //BinarySerializer，json.net，XmlSerializer和LZ4
            //结论：特性[Serializable]只对使用BinaryFormatter序列化(序列化成流)的时候有影响，其它的序列化不影响。

            var input = new TestSerializableClass
            {
                Id = 1,
                Name = "aaa",
                CreatedTime = DateTime.Now
            };

            //BinarySerializer
            var result1 = string.Empty;
            try
            {
                result1 = BinarySerializerHelper.Serialize(input);
            }
            catch (Exception ex)
            {

            }

            //json.net
            var result2 = string.Empty;
            try
            {
                result2 = input.ToJson();
            }
            catch (Exception ex)
            {

            }

            //XmlSerializer
            var result3 = string.Empty;
            try
            {
                result3 = input.ToXml();
            }
            catch (Exception ex)
            {

            }

            //LZ4
            byte[] result4 = null;
            try
            {
                result4 = input.ToJson().LZ4Compress();
            }
            catch (Exception ex)
            {

            }

        }

        [Test]
        public void Test_Extends_AddLastSecond()
        {
            var r1 = DateTime.MinValue;
            var r2 = DateTime.MaxValue;
            var r3 = default(DateTime);
            var r4 = r1 == r3;//true

            var r5 = DateTime.Now.AddLastSecond();

        }

        /// <summary>
        /// 测试去重
        /// </summary>
        [Test]
        public void Test_Extends_DistinctBy()
        {
            var list = new List<TestDistinct>()
            {
                new TestDistinct
                {
                    Id= 1,
                    BunkCode= "A",
                    BunkPrice= 101
                },
                new TestDistinct
                {
                    Id= 2,
                    BunkCode= "B",
                    BunkPrice= 102
                },
                new TestDistinct
                {
                    Id= 3,
                    BunkCode= "C",
                    BunkPrice= 103
                },
                new TestDistinct
                {
                    Id= 4,
                    BunkCode= "D",
                    BunkPrice= 104
                },
                new TestDistinct
                {
                    Id= 5,
                    BunkCode= "A",
                    BunkPrice= 101
                }
            };

            var result = list.DistinctBy(p => p.Id).ToList();

            list = list.DistinctBy(p => new { p.BunkCode, p.BunkPrice }).ToList();



        }


    }

    public class TestDistinct
    {
        public int Id { get; set; }

        public string BunkCode { get; set; }

        public double BunkPrice { get; set; }

    }

    [Serializable]
    public class TestSerializableClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedTime { get; set; }

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
