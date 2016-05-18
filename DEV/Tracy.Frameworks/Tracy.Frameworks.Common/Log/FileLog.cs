using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Tracy.Frameworks.Common.Const;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：5/24/2014 1:37:48 PM
 * 描述说明：记录文本日志
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Log
{
    public class FileLog: ILog
    {
        private static object logger_lock = 1;

        /// <summary>
        /// 记录一般信息
        /// </summary>
        /// <param name="msg"></param>
        public void Info(string msg)
        {
            lock (logger_lock)
            {
                WriteLog(LogFormat.INFO,msg,null);
            }
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="msg"></param>
        public void Debug(string msg)
        {
            lock (logger_lock)
            {
                WriteLog(LogFormat.DEBUG, msg, null);
            }
        }

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Warn(string msg, Exception ex)
        {
            lock (logger_lock)
            {
                WriteLog(LogFormat.WARN, msg, ex);
            }
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        public void Error(string msg, Exception ex)
        {
            lock (logger_lock)
            {
                WriteLog(LogFormat.ERROR,msg,ex);
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logType">日志类型</param>
        /// <param name="msg">附加消息</param>
        /// <param name="ex">异常对象</param>
        private static void WriteLog(string logType, string msg, Exception ex)
        {
            try
            {
                string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log"); //不要使用HttpContext.Current.Server.MapPath，因为它只适用于web程序
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                string path = Path.Combine(tempPath, DateTime.Now.ToString("yyyy-MM") + ".txt");
                string format = "[{0}] [{1}] {2} - {3} {4}";
                StackTrace stackTrace = new StackTrace(true);
                MethodBase method = stackTrace.GetFrame(2).GetMethod();

                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Flush();
                        streamWriter.BaseStream.Seek(0L, SeekOrigin.End);
                        streamWriter.WriteLine(String.Format(format, new object[]{
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                            logType,
                            method.DeclaringType.FullName,
                            msg,
                            ex
                        }));
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
