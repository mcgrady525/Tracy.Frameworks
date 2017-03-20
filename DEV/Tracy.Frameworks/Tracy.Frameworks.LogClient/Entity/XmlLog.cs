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
    /// xml日志
    /// </summary>
    public class XmlLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public XmlLog()
        {
            Url = ConfigHelper.GetAppSetting("Log.OpenApi.Url").TrimEnd('/') + "/api/xmllog/add";
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
        [JsonIgnore]
        public string RQ { get; set; }

        [JsonProperty("RQ")]
        public byte[] RQBinary
        {
            get
            { 
                //将RQ压缩为二进制格式
                return this.RQ.LZ4Compress();
            }
        }

        /// <summary>
        /// 返回xml
        /// </summary>
        [JsonIgnore]
        public string RS { get; set; }

        [JsonProperty("RS")]
        public byte[] RSBinary
        {
            get
            { 
                //将RS压缩为二进制格式
                return this.RS.LZ4Compress();
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
