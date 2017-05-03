using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracy.Frameworks.Common.Helpers;

namespace Tracy.Frameworks.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestBatchInsertDemo();            
            TestStopwatch();
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
}
