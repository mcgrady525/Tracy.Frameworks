using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// xml日志返回说明 主要字段:Source,SystemCode
    /// </summary>
    public class XmlLogInfo : LogInfoBase
    {

        public XmlLogInfo()
        {
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadName = Thread.CurrentThread.Name;
            Url = ConfigurationManager.AppSettings["Log.Service.Url"] + "/XmlLog/AddLog";
            CreateTime = DateTime.Now;
            MachineName = System.Environment.MachineName;
            AppDomainName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            IPAddress = LogHelper.IP;
        }

        /// <summary>
        /// 请求Xml
        /// </summary>
        public string RQ { get; set; }

        /// <summary>
        /// 返回Xml
        /// </summary>
        public string RS { get; set; }

        /// <summary>
        /// 一般不用傳，如果傳了則用這個，否則用RS.GZIP()
        /// </summary>
        public byte[] RSBinary { get; set; }

        /// <summary>
        /// 是否需要壓縮RS 
        /// 隊列發送前，如果此屬性為true && RSBinary==null,則RSBinary=RS.GZipCompress();RS=null
        /// </summary>
        [JsonIgnore]
        public bool IsNeedCompressRS { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// [可选] 默認為當前 线程名
        /// </summary>
        public string ThreadName { get; set; }


        /// <summary>
        /// [可选]应用域名
        /// </summary>
        public string AppDomainName { get; set; }

        /// <summary>
        /// 业务系统的IP地址
        /// </summary>
        public string IPAddress { get; set; }

    }
}
