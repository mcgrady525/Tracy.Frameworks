using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Tracy.Frameworks.LogClient.Helper;

namespace Tracy.Frameworks.LogClient.Entity
{
    /// <summary>
    /// 错误日志(error log)
    /// </summary>
    public class ErrorLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public ErrorLog()
        {
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
        /// 标题
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 应用程序域名称
        /// </summary>
        public string AppDomainName { get; set; }

    }
}
