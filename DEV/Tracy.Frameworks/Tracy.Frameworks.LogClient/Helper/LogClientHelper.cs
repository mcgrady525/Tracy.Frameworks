using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Tracy.Frameworks.LogClient.Entity;
using Tracy.Frameworks.Common.Helpers;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.LogClient.Helper
{
    /// <summary>
    /// 日志客户端helper
    /// </summary>
    public sealed class LogClientHelper
    {
        //单例模式
        private static readonly LogClientHelper logClientHelper = new LogClientHelper();
        private LogClientHelper()
        { }

        public static LogClientHelper Instance
        {
            get
            {
                return logClientHelper;
            }
        }

        /// <summary>
        /// 写debug日志
        /// </summary>
        /// <param name="debugLog"></param>
        public static void Debug(DebugLog debugLog)
        {
            var list = new List<DebugLog> { };
            list.Add(debugLog);
            var data = list.ToJson();
            HttpHelper.SendRequestByHttpWebRequest(debugLog.Url, data);
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="errorLog"></param>
        public static void Error(ErrorLog errorLog)
        {
            var list = new List<ErrorLog>();
            list.Add(errorLog);
            var data = list.ToJson();
            HttpHelper.SendRequestByHttpWebRequest(errorLog.Url, data);
        }

        /// <summary>
        /// 写xml日志
        /// </summary>
        /// <param name="xmlLog"></param>
        public static void Xml(XmlLog xmlLog)
        {
            var list = new List<XmlLog>();
            list.Add(xmlLog);
            var data = list.ToJson();
            HttpHelper.SendRequestByHttpWebRequest(xmlLog.Url, data);
        }

        /// <summary>
        /// 写性能日志
        /// </summary>
        /// <param name="performanceLog"></param>
        public static void Performance(PerformanceLog performanceLog)
        {
            var list = new List<PerformanceLog>();
            list.Add(performanceLog);
            var data = list.ToJson();
            HttpHelper.SendRequestByHttpWebRequest(performanceLog.Url, data);
        }

        /// <summary>
        /// 写operate log操作日志
        /// </summary>
        /// <param name="operateLog"></param>
        public static void Operate(OperateLog operateLog)
        {
            var list = new List<OperateLog>();
            list.Add(operateLog);
            var data = list.ToJson();
            HttpHelper.SendRequestByHttpWebRequest(operateLog.Url, data);
        }

        /// <summary>
        /// 写业务跟踪日志
        /// </summary>
        /// <param name="traceLog"></param>
        public static void Trace(TraceLog traceLog)
        {
            var list = new List<TraceLog>();
            list.Add(traceLog);
            var data = list.ToJson();
            HttpHelper.SendRequestByHttpWebRequest(traceLog.Url, data);
        }

        /// <summary>
        /// 同时写xml日志和性能日志
        /// </summary>
        /// <param name="xmlPerformanceLog"></param>
        public static void XmlPerformance(XmlPerformanceLog xmlPerformanceLog)
        {
            var list = new List<XmlPerformanceLog>();
            list.Add(xmlPerformanceLog);
            var data = list.ToJson();
            HttpHelper.SendRequestByHttpWebRequest(xmlPerformanceLog.Url, data);
        }

    }
}
