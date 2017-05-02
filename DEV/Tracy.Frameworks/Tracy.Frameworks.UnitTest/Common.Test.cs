using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Tracy.Frameworks.Common.Extends;
using Tracy.Frameworks.Common.Helpers;
using System.Diagnostics;

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
        public void Test_Stopwatch()
        { 
            //1，var stopWatch= new Stopwatch();
            //2，var stopWatch= Stopwatch.StartNew();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            //Do1();

            stopWatch.Stop();

            stopWatch = Stopwatch.StartNew();
            stopWatch.Start();

            //Do1();

            stopWatch.Stop();



        }

    }

    public class TestBatchInsert
    {
        /// <summary>
        /// 值，用guid表示
        /// </summary>
        public string Val { get; set; }
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
}
