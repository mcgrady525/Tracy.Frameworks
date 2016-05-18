using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracy.Frameworks.Log
{
    public class QueueMonitorInfo : LogInfoBase
    {

        /// <summary>
        /// 性能日志返回说明 必填字段:ClassName,MethodName,Duration,Source,SystemCode
        /// </summary>
        public QueueMonitorInfo()
        {
            Url = ConfigurationManager.AppSettings["Log.Service.Url"] + "/Home/AddMonitor";
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 对列类型
        /// </summary>
        public string QueueType { get; set; }

        /// <summary>
        /// 对列长度
        /// </summary>
        public int QueueLength { get; set; }

        /// <summary>
        /// 队列状态
        /// </summary>
        public TaskStatus TaskStatus { get; set; }

    }
}
