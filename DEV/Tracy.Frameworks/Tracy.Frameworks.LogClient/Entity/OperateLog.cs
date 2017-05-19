using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Tracy.Frameworks.Common.Helpers;
using Tracy.Frameworks.LogClient.Helper;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.LogClient.Entity
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OperateLog : BaseLog
    {
        private static readonly Process process = Process.GetCurrentProcess();

        public OperateLog()
        {
            Url = ConfigHelper.GetAppSetting("Log.OpenApi.Url").TrimEnd('/') + "/api/operatelog/add";
            MachineName = System.Environment.MachineName;
            IPAddress = HttpHelper.GetLocalIP();
            ClientIP = HttpHelper.GetClientIP();
            ProcessID = process.Id;
            ProcessName = process.ProcessName;
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
            CreatedTime = DateTime.Now;
            AppDomainName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        /// <summary>
        /// AppDomainName
        /// </summary>
        public string AppDomainName { get; set; }

        /// <summary>
        /// OperatedTime
        /// </summary>
        public DateTime? OperatedTime { get; set; }

        /// <summary>
        /// user_id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// user_name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// operate_module
        /// </summary>
        public string OperateModule { get; set; }

        /// <summary>
        /// operate_type
        /// </summary>
        public string OperateType { get; set; }

        /// <summary>
        /// modify_before
        /// </summary>
        [JsonIgnore]
        public string ModifyBefore { get; set; }

        [JsonProperty("ModifyBefore")]
        public byte[] ModifyBeforeDetail
        {
            get
            {
                return this.ModifyBefore.LZ4Compress();
            }
        }

        /// <summary>
        /// modify_after
        /// </summary>
        [JsonIgnore]
        public string ModifyAfter { get; set; }

        [JsonProperty("ModifyAfter")]
        public byte[] ModifyAfterDetail
        {
            get
            {
                return this.ModifyAfter.LZ4Compress();
            }
        }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CorpId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CorpName { get; set; }

    }
}
