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
    /// xml日志
    /// </summary>
    public class XmlLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public XmlLog()
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
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 应用程序域名称
        /// </summary>
        public string AppDomainName { get; set; }

        /// <summary>
        /// 请求xml
        /// </summary>
        public string RQ { get; set; }

        /// <summary>
        /// 返回xml
        /// </summary>
        public string RS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
