using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Tracy.Frameworks.Common.Helpers;
using Tracy.Frameworks.LogClient.Helper;
using Newtonsoft.Json;
using Tracy.Frameworks.Common.Extends;

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
            Url = ConfigHelper.GetAppSetting("Log.OpenApi.Url") + "/api/errorlog/add";
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
        [JsonIgnore]
        public string Detail { get; set; }

        [JsonProperty("Detail")]
        public byte[] DetailBinary
        {
            get
            {
                //将Detail压缩为二进制
                return this.Detail.LZ4Compress();
            }
        }

        /// <summary>
        /// 应用程序域名称
        /// </summary>
        public string AppDomainName { get; set; }

    }
}
