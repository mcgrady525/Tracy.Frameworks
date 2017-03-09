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
    /// 业务跟踪日志(trace log)
    /// </summary>
    public class TraceLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public TraceLog()
        {
            Url = ConfigHelper.GetAppSetting("Log.OpenApi.Url") + "api/tracelog/add";
            MachineName = System.Environment.MachineName;
            IPAddress = LogClientHelper.IP;
            ProcessID = process.Id;
            ProcessName = process.ProcessName;
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
            CreatedTime = DateTime.Now;
            AppDomainName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        /// <summary>
        /// 应用程序域名称
        /// </summary>
        public string AppDomainName { get; set; }

    }
}
