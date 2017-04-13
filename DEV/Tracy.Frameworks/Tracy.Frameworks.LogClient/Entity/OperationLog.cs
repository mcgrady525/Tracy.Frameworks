using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Tracy.Frameworks.Common.Helpers;
using Tracy.Frameworks.LogClient.Helper;

namespace Tracy.Frameworks.LogClient.Entity
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperationLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public OperationLog()
        {
            Url = ConfigHelper.GetAppSetting("Log.OpenApi.Url").TrimEnd('/') + "/api/operationlog/add";
            MachineName = System.Environment.MachineName;
            IPAddress = LogClientHelper.IP;
            ProcessID = process.Id;
            ProcessName = process.ProcessName;
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
            CreatedTime = DateTime.Now;
            //AppDomainName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }



    }
}
