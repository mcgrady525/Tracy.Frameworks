using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Consts
{
    /// <summary>
    /// 各业务系统的rabbitMQ队列命名
    /// </summary>
    [Serializable]
    public static class RabbitMQQueueConst
    {
        /// <summary>
        /// Log系统的debug log的rabbitMQ队列名称
        /// </summary>
        public const string LogDebugLog = "Log.Queue.DebugLog";

        /// <summary>
        /// Log系统的error log的rabbitMQ队列名称
        /// </summary>
        public const string LogErrorLog = "Log.Queue.ErrorLog";

        /// <summary>
        /// Log系统的xml log的rabbitMQ队列名称
        /// </summary>
        public const string LogXmlLog = "Log.Queue.XmlLog";

        /// <summary>
        /// Log系统的performance log的rabbitMQ队列名称
        /// </summary>
        public const string LogPerformanceLog = "Log.Queue.PerformanceLog";

        /// <summary>
        /// Log系统的trace log的rabbitMQ队列名称
        /// </summary>
        public const string LogTraceLog = "Log.Queue.TraceLog";

    }
}
