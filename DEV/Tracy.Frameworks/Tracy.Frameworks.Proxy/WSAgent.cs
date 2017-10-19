using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Tracy.Frameworks.LogClient.Entity;
using Tracy.Frameworks.LogClient.Helper;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Proxy
{
    /// <summary>
    /// 调用接口代理
    /// 1，写xml日志
    /// 2，写性能日志
    /// </summary>
    public class WSAgent
    {
        public string SystemCode { get; set; }

        public string ClassName { get; set; }

        public string MethodName { get; set; }

        public string MethodCName { get; set; }

        public string RequestXml { get; set; }

        public string ResponseXml { get; set; }

        /// <summary>
        /// 返回结果是否不需要序列化为xml，默认为false即需要序列化
        /// </summary>
        public bool IsNotNeedResultToXml { get; set; }

        /// <summary>
        /// 是否写xml日志和性能日志到日志系统，默认为true开启
        /// </summary>
        public bool IsWriteToLogSystem { get; set; }

        /// <summary>
        /// xml日志
        /// </summary>
        private XmlLog _XmlLog { get; set; }

        /// <summary>
        /// 性能日志
        /// </summary>
        private PerformanceLog _PerfLog { get; set; }

        public WSAgent()
        {
            _XmlLog = new XmlLog { Source = "SSharing.Proxy" };
            _PerfLog = new PerformanceLog { Source = "SSharing.Proxy" };
        }

        public TResponse Request<TResponse>(Func<TResponse> func)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var result = func();
                stopWatch.Stop();
                _PerfLog.Duration = stopWatch.ElapsedMilliseconds;
                if (!IsNotNeedResultToXml)
                {
                    ResponseXml = result.ToXml(isNeedFormat: true);
                }
                else
                {
                    ResponseXml = result.ToString();
                }
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                //写xml日志和性能日志
                //发一次http请求，同时写xml日志和性能日志(fix发两次请求时由于网络因素导致的第1次请求成功，第2次请求失败)
                WriteXmlPerformanceLog();
            }
        }

        /// <summary>
        /// 同时写xml日志和性能日志
        /// </summary>
        private void WriteXmlPerformanceLog()
        {
            if (IsWriteToLogSystem)
            {
                _XmlLog.SystemCode = SystemCode;
                _XmlLog.ClassName = ClassName;
                _XmlLog.MethodName = MethodName;
                _XmlLog.MethodCName = MethodCName;
                _XmlLog.RQ = RequestXml;
                _XmlLog.RS = ResponseXml;
                _XmlLog.Remark = "SSharing.Proxy";

                _PerfLog.SystemCode = SystemCode;
                _PerfLog.ClassName = ClassName;
                _PerfLog.MethodName = MethodName;
                _PerfLog.MethodCName = MethodCName;
                _PerfLog.Remark = "SSharing.Proxy";

                var xmlPerformanceLog = new XmlPerformanceLog()
                {
                    XmlLog = new XmlLog(),
                    PerformanceLog = new PerformanceLog()
                };
                xmlPerformanceLog.XmlLog = _XmlLog;
                xmlPerformanceLog.PerformanceLog = _PerfLog;
                LogClientHelper.XmlPerformance(xmlPerformanceLog);
            }
        }
    }
}
