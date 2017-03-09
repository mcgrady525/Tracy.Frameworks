using System;
using NUnit.Framework;
using Tracy.Frameworks.LogClient.Entity;
using Tracy.Frameworks.Common.Extends;
using System.Collections.Generic;

namespace Tracy.Frameworks.LogClient.Tests
{
    [NUnit.Framework.TestFixture]
    public class UnitTest1
    {
        [NUnit.Framework.Test]
        public void TestMethod1()
        {
            //var list = new List<DebugLog>();
            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new DebugLog
            //    {
            //        SystemCode = "Interface",
            //        Source = "Interface.Service",
            //        Message = "Message" + i,
            //        Detail = "Detail" + i
            //    });
            //}
            //var list = new List<ErrorLog>();
            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new ErrorLog 
            //    {
            //        SystemCode= "Log",
            //        Source= "Log.Task",
            //        Message = "Message" + i,
            //        Detail = "Detail"+i
            //    });
            //}
            var list = new List<PerformanceLog>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new PerformanceLog
                {
                    SystemCode = "Ubtrip",
                    Source = "Ubtrip.UI",
                    ClassName = "ClassName" + i,
                    MethodName = "MethodName" + i,
                    Duration = 10 * i,
                    Remark = "Remark" + i
                });
            }
            var result = list.ToJson();
        }

    }
}
