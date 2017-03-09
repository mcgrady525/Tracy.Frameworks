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
    /// 性能日志(performance log)
    /// </summary>
    public class PerformanceLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public PerformanceLog()
        {
            Url = ConfigHelper.GetAppSetting("Log.OpenApi.Url") + "/api/performancelog/add";
            MachineName = System.Environment.MachineName;
            IPAddress = LogClientHelper.IP;
            ProcessID = process.Id;
            ProcessName = process.ProcessName;
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
            CreatedTime = DateTime.Now;
        }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 耗时，单位：ms
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
