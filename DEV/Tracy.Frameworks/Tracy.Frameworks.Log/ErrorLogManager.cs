using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// Error日志监控
    /// </summary>
    public class ErrorLogManager
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
        /// 对列初始化
        /// </summary>
        public static ConcurrentQueue<ErrorLogInfo> Queue = new ConcurrentQueue<ErrorLogInfo>();

        /// <summary>
        /// 私有构造方法,防止别人私自使用
        /// </summary>
        private ErrorLogManager()
        {
            MaxReceiveCount = Convert.ToInt64(ConfigurationManager.AppSettings["Log.ErrorLog.MaxReceiveCount"] ?? "10000");
            MaxPostCount = Convert.ToInt32(ConfigurationManager.AppSettings["Log.ErrorLog.MaxPostCount"] ?? "10");
        }

        public static readonly ErrorLogManager Instance = new ErrorLogManager();

        /// <summary>
        /// 开启任务
        /// </summary>
        internal static readonly Task WriteTask = Task.Factory.StartNew(() =>
        {
            Thread.CurrentThread.Name = "ErrorLogManager.Run";
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
            LogHelper.Monitor(Queue, WriteTask, "ErrorLog");
        }, TaskCreationOptions.LongRunning).ContinueWith(t => { var exp = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);

        /// <summary>
        /// 排队
        /// </summary>
        /// <param name="info"></param>
        public void Enqueue(ErrorLogInfo info)
        {
            //营业中，排队才有意义
            if (WriteTask.Status != TaskStatus.Running && WriteTask.Status != TaskStatus.WaitingToRun && WriteTask.Status != TaskStatus.WaitingForActivation)
            {
                return;
            }
            if (Queue.Count < MaxReceiveCount)
            {
                Queue.Enqueue(info);
            }

        }

        /// <summary>
        /// 发送对列到日志系统
        /// </summary>
        private static void Send()
        {
            var temp = new List<ErrorLogInfo>();
            while (true)
            {
                ErrorLogInfo info;
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
