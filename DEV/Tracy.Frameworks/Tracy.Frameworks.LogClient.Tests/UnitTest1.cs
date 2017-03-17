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
        public void TestMethod2()
        {
            var debugLog = new DebugLog 
            {
                SystemCode= "Ubtrip",
                Source= "Ubtrip.UI",
                Message = "Message111",
                Detail = "Detail111"
            };

            var result = debugLog.ToXml(isNeedFormat:true);

        }


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
            //var list = new List<PerformanceLog>();
            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new PerformanceLog
            //    {
            //        SystemCode = "Ubtrip",
            //        Source = "Ubtrip.UI",
            //        ClassName = "ClassName" + i,
            //        MethodName = "MethodName" + i,
            //        Duration = 10 * i,
            //        Remark = "Remark" + i
            //    });
            //}

            var list = new List<DebugLog>();
            list.Add(new DebugLog 
            {
                SystemCode = "Interface",
                Source = "Interface.Service",
                Message = "Message国足加油",
                Detail = "Detail111"
            });

            list.Add(new DebugLog 
            {
                SystemCode = "Interface",
                Source = "Interface.Service",
                Message = "Message国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油国足加油",
                Detail = "Detail222"
            });

            var result = list.ToJson();
        }

    }
}
