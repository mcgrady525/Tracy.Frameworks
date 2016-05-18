using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// 错误日志返回说明 必填字段:Message,Detail,Source,SystemCode
    /// </summary>
    public class ErrorLogInfo : LogInfoBase
    {
        private static readonly Process process = Process.GetCurrentProcess();

        /// <summary>
        /// 错误日志返回说明 必填字段:Message,Detail,Source,SystemCode
        /// </summary>
        public ErrorLogInfo()
        {
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
            Url = ConfigurationManager.AppSettings["Log.Service.Url"] + "/ErrorLog/AddLog";
            CreateTime = DateTime.Now;
            ProcessID = process.Id;
            ProcessName = process.ProcessName;
            MachineName = System.Environment.MachineName;
            AppDomainName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            IPAddress = LogHelper.IP;
        }


        /// <summary>
        /// [必選] 标题 一般传:ex.Message
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// [必選] 消息 一般传ex.ToString();
        /// </summary>
        [JsonIgnore]
        public string Detail { get; set; }


        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("Detail")]
        public byte[] DetailBinary
        {
            get
            {
                return LogHelper.Compress(Detail);
            }
        }



        /// <summary>
        /// [可选]应用域名
        /// </summary>
        public string AppDomainName { get; set; }

        /// <summary>
        /// [可选]进程标识符
        /// </summary>
        public int ProcessID { get; set; }

        /// <summary>
        /// [可选]进程名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// [可选]线程名字
        /// </summary>
        public string ThreadName { get; set; }

        /// <summary>
        /// 业务系统的IP地址
        /// </summary>
        public string IPAddress { get; set; }

    }
}
