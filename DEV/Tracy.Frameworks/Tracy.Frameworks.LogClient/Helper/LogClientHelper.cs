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
            HttpHelper.SendRequestByHttpWebRequest(debugLog.Url, debugLog.ToJson());
        }

        /// <summary>
        /// 写error日志
        /// </summary>
        /// <param name="errorLog"></param>
        public static void Error(ErrorLog errorLog)
        {
            HttpHelper.SendRequestByHttpWebRequest(errorLog.Url, errorLog.ToJson());
        }

        /// <summary>
        /// 写性能日志
        /// </summary>
        /// <param name="performanceLog"></param>
        public static void Performance(PerformanceLog performanceLog)
        {
            HttpHelper.SendRequestByHttpWebRequest(performanceLog.Url, performanceLog.ToJson());
        }

        /// <summary>
        /// 写xml日志
        /// </summary>
        /// <param name="xmlLog"></param>
        public static void Xml(XmlLog xmlLog)
        {
            HttpHelper.SendRequestByHttpWebRequest(xmlLog.Url, xmlLog.ToJson());
        }

        /// <summary>
        /// 写业务跟踪日志
        /// </summary>
        /// <param name="traceLog"></param>
        public static void Trace(TraceLog traceLog)
        {
            HttpHelper.SendRequestByHttpWebRequest(traceLog.Url, traceLog.ToJson());
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
