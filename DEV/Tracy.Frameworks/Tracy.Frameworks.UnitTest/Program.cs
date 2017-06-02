using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracy.Frameworks.Common.Helpers;
using EmitMapper;
using Nelibur.ObjectMapper;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestBatchInsertDemo();            
            //TestStopwatch();
            //TestDtoMapper();
            //TestDeepClonePerf();
            //TestDistinct();
            TestOrderBy();
        }

        /// <summary>
        /// 测试排序
        /// </summary>
        private static void TestOrderBy()
        {
            var list = new List<TestDistinctClass> 
            {
                new TestDistinctClass
                {
                    Id= 1,
                    BunkCode= "A",
                    BunkPrice= 105
                },
                new TestDistinctClass
                {
                    Id= 2,
                    BunkCode= "B",
                    BunkPrice= 104
                },
                new TestDistinctClass
                {
                    Id= 3,
                    BunkCode= "C",
                    BunkPrice= 103
                },
                new TestDistinctClass
                {
                    Id= 4,
                    BunkCode= "D",
                    BunkPrice= 102
                },
                new TestDistinctClass
                {
                    Id= 5,
                    BunkCode= "A",
                    BunkPrice= 101
                }
            };

            //方法1：使用Linq
            var result1 = list.OrderByDescending(p => p.Id).ToList();
            var result2 = list.OrderBy(p => p.BunkPrice).ToList();

        }

        /// <summary>
        /// 测试去重
        /// </summary>
        private static void TestDistinct()
        {
            var list = new List<TestDistinctClass> 
            {
                new TestDistinctClass
                {
                    Id= 1,
                    BunkCode= "A",
                    BunkPrice= 101
                },
                new TestDistinctClass
                {
                    Id= 2,
                    BunkCode= "B",
                    BunkPrice= 102
                },
                new TestDistinctClass
                {
                    Id= 3,
                    BunkCode= "C",
                    BunkPrice= 103
                },
                new TestDistinctClass
                {
                    Id= 4,
                    BunkCode= "D",
                    BunkPrice= 104
                },
                new TestDistinctClass
                {
                    Id= 5,
                    BunkCode= "A",
                    BunkPrice= 101
                }
            };

            //方法1：使用默认的distinct方法
            //只能针对基元类型列表，对于自定义类型组合字段条件需要自定义相等比较器实现IEqualityComparer接口，比较麻烦
            var result1 = list.Distinct().ToList();

            //方法2：使用GroupBy
            var result2 = list.GroupBy(p => new { p.BunkCode, p.BunkPrice })
                .Select(p => p.First())
                .ToList();

            //方法3：使用自己扩展的DistinctBy方法
            //利用HashSet的key不能重复的特性
            var result3 = list.DistinctBy(p => new { p.BunkCode, p.BunkPrice })
                .ToList();
        }

        /// <summary>
        /// 测试深克隆的性能
        /// </summary>
        private static void TestDeepClonePerf()
        {
            var iteration = 10 * 10000;
            CodeTimerHelper.Initialize();

            var input = new DtoFrom
            {
                Name = "aaa",
                Age = 100,
                CreatedTime = DateTime.Now
            };

            DtoFrom result = null;
            CodeTimerHelper.Time("测试深克隆的性能", iteration, () =>
            {
                result = input.DeepClone();
            });
            Console.ReadKey();
        }

        private static void TestDtoMapper()
        {
            //比较TinyMapper和EmitMapper的性能
            var input = new DtoFrom
            {
                Name = "zhangsan",
                Age = 20,
                CreatedTime = DateTime.Now
            };
            DtoTo result = null;
            var iteration = 10 * 10000;
            CodeTimerHelper.Initialize();

            //1，EmitMapper
            CodeTimerHelper.Time("EmitMapper性能测试(10 * 10000)", iteration, () =>
            {
                var mapper = ObjectMapperManager.DefaultInstance.GetMapper<DtoFrom, DtoTo>();
                result = mapper.Map(input);
            });

            //2，TinyMapper
            CodeTimerHelper.Time("TinyMapper性能测试(10 * 10000)", iteration, () =>
            {
                TinyMapper.Bind<DtoFrom, DtoTo>();
                result = TinyMapper.Map<DtoTo>(input);
            });

            Console.ReadKey();
        }

        /// <summary>
        /// 测试Stopwatch
        /// </summary>
        private static void TestStopwatch()
        {
            //1，var stopWatch= new Stopwatch();
            //2，var stopWatch= Stopwatch.StartNew();//这种方式，不再需要调用Start()方法。

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Do1();

            stopWatch.Stop();
            Console.WriteLine(string.Format("使用new Stopwatch方法，用时：{0}", stopWatch.ElapsedMilliseconds.ToString("N0")));

            var stopWatch1 = Stopwatch.StartNew();
            //stopWatch.Start();

            Do2();

            stopWatch1.Stop();
            Console.WriteLine(string.Format("使用Stopwatch.StartNew方法，用时：{0}", stopWatch1.ElapsedMilliseconds.ToString("N0")));

            Console.ReadKey();
        }

        private static void Do2()
        {
            Thread.Sleep(500);
        }

        private static void Do1()
        {
            Thread.Sleep(1000);
        }

        /// <summary>
        /// dapper批量插入测试demo
        /// </summary>
        private static void TestBatchInsertDemo()
        {
            //比较dapper的insert into values和SqlBulkCopy的性能
            //测试情形1：insert into单条插入100000条
            //测试情形2：insert into多条插入100000条，100*1000
            //测试情形3：SqlBulkCopy批量插入，100*1000
            var connStr = "Data Source=.;Initial Catalog=TestDB;Persist Security Info=True;User ID=sa;Password=password.123";

            //1
            var item = new TestBatchInsert { Val = Guid.NewGuid().ToString() };
            CodeTimerHelper.Initialize();
            CodeTimerHelper.Time("dapper单条插入", 10000, () =>
            {
                new SqlHelper(connStr).Execute(@"INSERT INTO dbo.TestBatchInsert VALUES (@Val);", item);
            });

            List<TestBatchInsert> list = new List<TestBatchInsert>();
            var nums = Enumerable.Range(0, 100).ToList();
            nums.ForEach(i =>
            {
                list.Add(new TestBatchInsert
                {
                    Val = Guid.NewGuid().ToString()
                });
            });
            //2
            CodeTimerHelper.Time("dapper批量插入", 100, () =>
            {
                new SqlHelper(connStr).Execute(@"INSERT INTO dbo.TestBatchInsert VALUES (@Val);", list);
            });

            //3
            List<TestBatchInsert> list2 = new List<TestBatchInsert>();
            nums.ForEach(i =>
            {
                list2.Add(new TestBatchInsert
                {
                    Val = Guid.NewGuid().ToString()
                });
            });
            CodeTimerHelper.Time("SqlBulkCopy批量插入", 100, () =>
            {
                new SqlHelper(connStr).BulkCopy(list2, "TestBatchInsert");
            });

            Console.ReadKey();
        }

    }

    /// <summary>
    /// 测试类型
    /// </summary>
    public class TestDistinctClass
    {
        public int Id { get; set; }

        public string BunkCode { get; set; }

        public double BunkPrice { get; set; }
    }

    public class DtoFrom
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreatedTime { get; set; }
    }

    public class DtoTo
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreatedTime { get; set; }
    }

    public class TestBatchInsert
    {
        /// <summary>
        /// 值，用guid表示
        /// </summary>
        public string Val { get; set; }
    }
}
