using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracy.Frameworks.Common.Helpers;

namespace Tracy.Frameworks.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestBatchInsertDemo();            
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
