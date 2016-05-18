using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// 性能日志返回说明 必填字段:ClassName,MethodName,Duration,Source,SystemCode
    /// </summary>
    public class PerfLogInfo : LogInfoBase
    {
        /// <summary>
        /// 性能日志返回说明 必填字段:ClassName,MethodName,Duration,Source,SystemCode
        /// </summary>
        public PerfLogInfo()
        {
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            Url = ConfigurationManager.AppSettings["Log.Service.Url"] + "/PerfLog/AddLog";
            CreateTime = DateTime.Now;
            MachineName = System.Environment.MachineName;
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
        /// 花费时间
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
