using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// 性能日志监控
    /// </summary>
    public class XmlLogManager
    {
        /// <summary>
        /// 一次最多提交的条数
        /// </summary>
        public static int MaxPostCount { get; set; }

        /// <summary>
        /// 监控对列最大的接收数量
        /// </summary>
        public static long MaxReceiveCount { get; set; }


        /// <summary>
        /// Xml日志对列初始化
        /// </summary>
        public static ConcurrentQueue<XmlLogInfo> Queue = new ConcurrentQueue<XmlLogInfo>();

        private static object objLock = new object();
        private const string QueueFullStr = "XmlLogManager.Full";

        /// <summary>
        /// 私有构造方法,防止别人私自使用
        /// </summary>
        private XmlLogManager()
        {
            MaxReceiveCount = Convert.ToInt64(ConfigurationManager.AppSettings["Log.XmlLog.MaxReceiveCount"] ?? "1000");
            MaxPostCount = Convert.ToInt32(ConfigurationManager.AppSettings["Log.XmlLog.MaxPostCount"] ?? "100");
        }

        public static readonly XmlLogManager Instance = new XmlLogManager();

        /// <summary>
        /// 开启任务
        /// </summary>
        internal static Task WriteTask = Task.Factory.StartNew(() =>
        {
            Thread.CurrentThread.Name = "XmlLogManager.Run";
            while (true)
            {
                Send();
            }
        }, TaskCreationOptions.LongRunning).ContinueWith(t => { var exp = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

        /// <summary>
        /// 开启监控任务
        /// </summary>
        private static readonly Task QueueMonitorTask = Task.Factory.StartNew(() =>
        {
            LogHelper.Monitor(Queue, WriteTask, "XmlLog");
        }, TaskCreationOptions.LongRunning).ContinueWith(t => { var exp = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

        /// <summary>
        /// 排队
        /// </summary>
        /// <param name="info"></param>
        public void Enqueue(XmlLogInfo info)
        {
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            if (path.IndexOf("Scheduler", StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                ResetRS(info);
                LogHelper.Send(info);
            }
            else
            {
                //营业中，排队才有意义
                if (WriteTask.Status != TaskStatus.Running && WriteTask.Status != TaskStatus.WaitingToRun && WriteTask.Status != TaskStatus.WaitingForActivation)
                {
                    lock (objLock)
                    {
                        if (WriteTask.Status != TaskStatus.Running && WriteTask.Status != TaskStatus.WaitingToRun && WriteTask.Status != TaskStatus.WaitingForActivation)
                        {
                            WriteTask = Task.Factory.StartNew(() =>
                            {
                                Thread.CurrentThread.Name = "XmlLogManager.Run";
                                while (true)
                                {
                                    Send();
                                }
                            }, TaskCreationOptions.LongRunning).ContinueWith(t => { var exp = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
                        }
                    }
                }
                if (Queue.Count < MaxReceiveCount)
                {
                    Queue.Enqueue(info);
                }
                else
                {
                    if (HttpRuntime.Cache[QueueFullStr] == null)
                    {
                        ErrorLogManager.Instance.Enqueue(new ErrorLogInfo
                        {
                            Message = QueueFullStr,
                            Detail = QueueFullStr,
                            Source = info.Source,
                            SystemCode = info.SystemCode
                        });
                        HttpRuntime.Cache.Insert(QueueFullStr, DateTime.Now, null, DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration);
                    }
                }
            }
        }

        /// <summary>
        /// 发送对列到XML日志系统
        /// </summary>
        private static void Send()
        {
            var temp = new List<XmlLogInfo>();
            while (true)
            {
                XmlLogInfo info;
                if (Queue.TryDequeue(out info))
                {

                    ResetRS(info);

                    temp.Add(info);
                    if (temp.Count >= MaxPostCount)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            if (temp.Count > 0)
            {
                LogHelper.Send(temp);
            }
            else
            {
                Thread.Sleep(5000); //5ms，因為XML日誌一般都很大放太多在內存里不合適
            }
        }

        private static void ResetRS(XmlLogInfo info)
        {
            if (info.IsNeedCompressRS && info.RSBinary == null)
            {
                info.RSBinary = LogHelper.Compress(info.RS);
                info.RS = null;
            }
        }
    }
}
