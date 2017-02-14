using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Consts
{
    /// <summary>
    /// 各业务系统的rabbitMQ交换机命名
    /// </summary>
    [Serializable]
    public static class RabbitMQExchangeConst
    {
        /// <summary>
        /// Log系统的debug log的rabbitMQ交换机名称
        /// </summary>
        public const string LogDebugLog = "Log.Exchange.DebugLog";

        /// <summary>
        /// Log系统的error log的rabbitMQ交换机名称
        /// </summary>
        public const string LogErrorLog = "Log.Exchange.ErrorLog";

        /// <summary>
        /// Log系统的xml log的rabbitMQ交换机名称
        /// </summary>
        public const string LogXmlLog = "Log.Exchange.XmlLog";

        /// <summary>
        /// Log系统的performance log的rabbitMQ交换机名称
        /// </summary>
        public const string LogPerformanceLog = "Log.Exchange.PerformanceLog";

        /// <summary>
        /// Log系统的trace log的rabbitMQ交换机名称
        /// </summary>
        public const string LogTraceLog = "Log.Exchange.TraceLog";

    }
}
