using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// 日志系统基类
    /// </summary>
    public abstract class LogInfoBase
    {
        /// <summary>
        /// [可選] 默認為 当前线程ID
        /// </summary>
        public int ThreadID { get; set; }

        /// <summary>
        /// [可選] 默認從config拿url
        /// </summary>
        [JsonIgnore]
        public string Url { get; set; }

        /// <summary>
        /// [可選] 默認當前時間
        /// </summary>
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// [可选]机器名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// 来源,如: Online.Site,Offline.Site,Offline.Service,Scheduler
        /// </summary>
        public string Source { get; set; }


        /// <summary>
        /// 系统编码,如:Hotel,Ticket,PBS
        /// </summary>
        public string SystemCode { get; set; }
    }
}
