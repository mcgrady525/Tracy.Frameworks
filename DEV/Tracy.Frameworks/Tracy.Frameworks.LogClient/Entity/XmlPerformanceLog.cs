using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Tracy.Frameworks.Common.Extends;
using Tracy.Frameworks.Common.Helpers;

namespace Tracy.Frameworks.LogClient.Entity
{
    /// <summary>
    /// xml和性能日志
    /// </summary>
    public class XmlPerformanceLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public XmlPerformanceLog()
        {
            Url = ConfigHelper.GetAppSetting("Log.OpenApi.Url").TrimEnd('/') + "/api/xmlperformancelog/add";
            MachineName = System.Environment.MachineName;
            IPAddress = HttpHelper.GetLocalIP();
            ClientIP = HttpHelper.GetClientIP();
            ProcessID = process.Id;
            ProcessName = process.ProcessName;
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
            CreatedTime = DateTime.Now;
            XmlLog.AppDomainName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        public XmlLog XmlLog { get; set; }

        public PerformanceLog PerformanceLog { get; set; }

    }
}
