using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Consts
{
    /// <summary>
    /// 日志类型常量
    /// </summary>
    [Serializable]
    public static class LogTypeConst
    {
        /// <summary>
        /// 日志分类: DEBUG
        /// </summary>
        public static readonly string DEBUG = "DEBUG";
        /// <summary>
        /// 日志分类:INFO
        /// </summary>
        public static readonly string INFO = "INFO";
        /// <summary>
        /// 日志分类:ERROR
        /// </summary>
        public static readonly string ERROR = "ERROR";
        /// <summary>
        /// 日志分类:WARN
        /// </summary>
        public static readonly string WARN = "WARN";


        /// <summary>
        /// 日志優先順序:高
        /// </summary>
        public static readonly int HIGH = 10;
        /// <summary>
        /// 日誌優先順序:中
        /// </summary>
        public static readonly int NORMAL = 20;
        /// <summary>
        /// 日誌優先順序:低
        /// </summary>
        public static readonly int LOW = 30;
    }
}
