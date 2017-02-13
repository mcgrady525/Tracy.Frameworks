using System;
using NUnit.Framework;
using Tracy.Frameworks.LogClient.Entity;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.LogClient.Tests
{
    [NUnit.Framework.TestFixture]
    public class UnitTest1
    {
        [NUnit.Framework.Test]
        public void TestMethod1()
        {
            var debugLog = new DebugLog 
            {
                SystemCode= "Log",
                Source= "Log.Service",
                Message= "Message1",
                Detail = "Detail1"
            };
            var result = debugLog.ToJson();
        }
    }
}
