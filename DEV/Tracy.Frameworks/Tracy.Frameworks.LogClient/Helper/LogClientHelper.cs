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

        #region 其它
        /// <summary>
        /// ip
        /// </summary>
        private static string _IP;

        /// <summary>
        /// ip
        /// </summary>
        /// <returns></returns>
        public static string IP
        {
            get
            {
                if (_IP == null)
                {
                    try
                    {
                        IPHostEntry host;
                        var sb = new StringBuilder();
                        host = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (var ip in host.AddressList)
                        {
                            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                sb.Append(",");
                                sb.Append(ip.ToString());
                            }
                        }
                        _IP = sb.ToString().Trim(new char[] { ',' });
                    }
                    catch
                    {
                        _IP = "无";
                    }
                }
                return _IP;
            }
        }
        #endregion

    }
}
