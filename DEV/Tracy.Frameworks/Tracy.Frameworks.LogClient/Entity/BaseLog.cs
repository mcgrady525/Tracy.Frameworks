using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Tracy.Frameworks.LogClient.Entity
{
    /// <summary>
    /// 日志实体基类
    /// </summary>
    public abstract class BaseLog
    {
        /// <summary>
        /// 系统编码，各业务系统编码，如Ubtrip,Net等
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 来源，如SSharing.Ubtrip.UI，SSharing.Expense.UI，SSharing.Ubtrip.WinService等
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 机器名称
        /// </summary>
        public string MachineName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 进程id
        /// </summary>
        public int ProcessID { get; set; }

        /// <summary>
        /// 进程名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 线程id
        /// </summary>
        public int ThreadID { get; set; }

        /// <summary>
        /// 线程名称
        /// </summary>
        public string ThreadName { get; set; }

        /// <summary>
        /// 创建时间，格式：yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        public DateTime CreatedTime { get; set; }

    }
}
