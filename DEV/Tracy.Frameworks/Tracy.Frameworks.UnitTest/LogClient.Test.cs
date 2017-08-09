using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracy.Frameworks.LogClient.Entity;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.UnitTest
{
    /// <summary>
    /// Tracy.Frameworks.LogClient的单元测试
    /// </summary>
    [TestFixture]
    public class LogClient
    {
        /// <summary>
        /// 测试调试日志
        /// </summary>
        [Test]
        public void Test_DebugLog_Insert()
        {
            var list = new List<DebugLog>();
            var nums = Enumerable.Range(1, 100).ToList();
            nums.ForEach(i =>
            {
                list.Add(new DebugLog
                {
                    SystemCode = "SystemCode" + i,
                    Source = "Source" + i,
                    Message = "Message" + i,
                    Detail = "Detail" + i
                });
            });

            var result = list.ToJson();

        }

        /// <summary>
        /// 测试操作日志
        /// </summary>
        [Test]
        public void Test_OperateLog_Insert()
        {
            var list = new List<OperateLog>();
            var nums = Enumerable.Range(101, 200).ToList();
            nums.ForEach(i =>
            {
                list.Add(new OperateLog
                {
                    SystemCode = "SystemCode" + i,
                    Source = "Source" + i,
                    OperatedTime = DateTime.Now,
                    UserId = "UserId" + i,
                    UserName = "UserName" + i,
                    OperateModule = "OperateModule" + i,
                    OperateType = "OperateType" + i,
                    ModifyBefore = "ModifyBefore" + i,
                    ModifyAfter = "ModifyAfter" + i,
                    CorpId = i,
                    CorpName = "CorpName" + i,
                    Remark = "Remark"+ i
                });
            });

            var result = list.ToJson();

        }

        [Test]
        public void Test_HttpHelper_Post()
        {
            var debugLog = new DebugLog 
            {
                SystemCode = "Ubtrip",
                Source = "HttpHelper",
                Message = "Test using httpHelper post!",
                Detail = "Test using httpHelper post!"
            };
            Tracy.Frameworks.LogClient.Helper.LogClientHelper.Debug(debugLog);
        }


    }
}
