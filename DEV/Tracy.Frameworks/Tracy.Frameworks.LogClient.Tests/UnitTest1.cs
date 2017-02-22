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
            //var debugLog = new DebugLog 
            //{
            //    SystemCode= "Log",
            //    Source= "Log.Service",
            //    Message= "Message1",
            //    Detail = "Detail1"
            //};
            //var result = debugLog.ToJson();
            //var list = new List<DebugLog>();
            //for (int i = 0; i < 10; i++)
            //{
            //    list.Add(new DebugLog 
            //    {
            //       SystemCode= "Log", 
            //       Source= "Log.Service",
            //       Message = "Message"+ i,
            //       Detail = "Detail"+ i
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
            var list = new List<XmlLog>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new XmlLog
                {
                    SystemCode = "Log",
                    Source = "Log.Task",
                    ClassName = "ClassName" + i,
                    MethodName = "MethodName" + i,
                    RQ = "RQ" + i,
                    RS = "RS" + i,
                    Remark = "Remark" + i
                });
            }
            var result = list.ToJson();
        }

    }
}
