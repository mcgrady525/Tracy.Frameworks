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
    public class PerfLogManager
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
        /// 性能日志对列初始化
        /// </summary>
        public static ConcurrentQueue<PerfLogInfo> Queue = new ConcurrentQueue<PerfLogInfo>();


        /// <summary>
        /// 私有构造方法,防止别人私自使用
        /// </summary>
        private PerfLogManager()
        {
            MaxReceiveCount = Convert.ToInt64(ConfigurationManager.AppSettings["Log.PerfLog.MaxReceiveCount"] ?? "100000");
            MaxPostCount = Convert.ToInt32(ConfigurationManager.AppSettings["Log.PerfLog.MaxPostCount"] ?? "1000");
        }

        public static readonly PerfLogManager Instance = new PerfLogManager();

        /// <summary>
        /// 开启任务
        /// </summary>
        internal static Task WriteTask = Task.Factory.StartNew(() =>
        {
            Thread.CurrentThread.Name = "PerfLogManager.Run";
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
            LogHelper.Monitor(Queue, WriteTask, "PerfLog");
        }, TaskCreationOptions.LongRunning).ContinueWith(t => { var exp = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);



        private static object objLock = new object();
        private const string QueueFullStr = "PerfLogManager.Full";
        /// <summary>
        /// 排队
        /// </summary>
        /// <param name="info"></param>
        public void Enqueue(PerfLogInfo info)
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
                            Thread.CurrentThread.Name = "PerfLogManager.Run";
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



        /// <summary>
        /// 发送对列到日志系统
        /// </summary>
        private static void Send()
        {
            var temp = new List<PerfLogInfo>();
            while (true)
            {
                PerfLogInfo info;
                if (Queue.TryDequeue(out info))
                {
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
                Thread.Sleep(10000);

            }
        }
    }
}
