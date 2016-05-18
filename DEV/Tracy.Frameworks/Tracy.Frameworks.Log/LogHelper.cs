using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// 日志系统公共方法类
    /// </summary>
    public static class LogHelper
    {
        public const string LogSection = "LogSection";
        private const string GZipStr = "gzip";
        private const string ZLibStr = "zlib";

        public static string LogCompressType { get { return ConfigurationManager.AppSettings["Log.Compress.Type"] ?? GZipStr; } }

        public static byte[] Compress(string input)
        {
            if (LogCompressType == ZLibStr)
            {
                return input.MySQLCompress();
            }
            else
            {
                return input.GZipCompress();
            }
        }


        public static string UnCompress(byte[] input)
        {
            if (LogCompressType == ZLibStr)
            {
                return input.MySQLUncompress();
            }
            else
            {
                return input.GZipDecompress();
            }
        }

        /// <summary>
        /// Send提交数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Send<T>(List<T> data) where T : LogInfoBase
        {
            try
            {
                if (data == null || !data.Any())
                {
                    return false;
                }

                var url = data.First().Url;
                if (string.IsNullOrEmpty(url))
                {
                    return false;
                }

                var json = JsonConvert.SerializeObject(data);
                var postData = Encoding.UTF8.GetBytes(json);

                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/json; charset=utf-8");
                    // 将字符串转换成字节数组
                    var responseData = client.UploadData(url, "POST", postData);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Send提交数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Send<T>(T data) where T : LogInfoBase
        {
            return Send(new List<T> { data });
        }


        internal static void Monitor<T>(ConcurrentQueue<T> queue, Task writeTask, string queueType) where T : LogInfoBase
        {
            string systemCode, source;

            while (true)
            {
                var que = queue.FirstOrDefault();
                if (que != null)
                {
                    systemCode = que.SystemCode;
                    source = que.Source;
                    break;
                }
                Thread.Sleep(5000);
            }

            while (true)
            {
                var info = new QueueMonitorInfo()
                {
                    SystemCode = systemCode,
                    Source = source,
                    QueueType = queueType,
                    TaskStatus = writeTask.Status,
                    QueueLength = queue.Count,
                };
                LogHelper.Send(info);

                Thread.Sleep(5000);
            }

        }


        #region 分段代码性能日志，将各段性能执行时间数，附加到性能日志的Remark中
        public static string GetLogContextStr()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return null;
            }

            var dic = System.Web.HttpContext.Current.Items[LogSection] as Dictionary<string, long>;
            if (dic == null || dic.Count == 0)
            {
                return null;
            }
            else
            {
                var jsonStr = JsonConvert.SerializeObject(dic, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.None
                });
                return jsonStr;
            }
        }

        public static void AttachLogToContext(string keyName, long times)
        {
            if (times <= 0 || System.Web.HttpContext.Current == null)
            {
                return;
            }
            var logDict = System.Web.HttpContext.Current.Items[LogSection] as Dictionary<string, long>;
            if (logDict != null)
            {
                try
                {
                    if (!logDict.ContainsKey(keyName))
                    {
                        logDict.Add(keyName, times);
                    }
                    else
                    {
                        int count = logDict.Keys.Where(p => p.Contains(keyName)).Count();
                        logDict.Add(keyName + "_" + count, times);
                    }
                }
                catch (Exception)
                {
                    logDict.Add(keyName + "_" + logDict.Count, times);
                }
            }
        }

        /// <summary>
        /// 从xxxApi接口返回后，需将LogSectionDic属性初始化到RestApi的上下文中
        /// </summary>
        /// <param name="dict"></param>
        public static void CombineLogToContext(Dictionary<string, long> dict)
        {
            try
            {
                if (System.Web.HttpContext.Current != null && dict != null && dict.Count > 0)
                {
                    System.Web.HttpContext.Current.Items[LogSection] = new Dictionary<string, long>();
                    foreach (var item in dict)
                    {
                        AttachLogToContext(item.Key, item.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogManager.Instance.Enqueue(new ErrorLogInfo()
                {
                    Message = ex.Message,
                    Detail = ex.ToString(),
                    SystemCode = "xxxSZ.Frameworks",
                    Source = "xxxSZ.Frameworks.Log",
                });
            }
        }

        /// <summary>
        /// 将从Wcf端Header返回的Context附加到RestApi端
        /// </summary>
        /// <typeparam name="T">ChannelFactory<xxxSZ.ServiceBus.HotelRestService.IHotelRestService>中的泛型参</typeparam>
        /// <param name="client">factory.CreateChannel实例</param>
        /// <param name="action">执行RestApi端调用wcf的方法</param>
        public static void OperationLogContext<T>(T client, Action action)
        {
            //使用通道建立Scope
            using (OperationContextScope scope = new OperationContextScope(client as IContextChannel))
            {
                //执行RestApi端调用wcf的方法
                action();
                if (OperationContext.Current != null && OperationContext.Current.IncomingMessageHeaders != null)
                {
                    var logSection = OperationContext.Current.IncomingMessageHeaders.FirstOrDefault(n => n.Name == LogHelper.LogSection);
                    if (logSection != null)
                    {
                        var logSectionStr = logSection.Namespace;
                        if (!string.IsNullOrWhiteSpace(logSectionStr))
                        {
                            LogHelper.CombineLogToContext(logSectionStr.FromJson<Dictionary<string, long>>());
                        }
                    }
                }
            }
        }

        public static Dictionary<string, long> GetLogContext()
        {
            if (System.Web.HttpContext.Current == null)
            {
                return new Dictionary<string, long>();
            }
            var LogDict = System.Web.HttpContext.Current.Items[LogHelper.LogSection] as Dictionary<string, long>;
            return LogDict;
        }
        #endregion


        private static string _IP;
        /// <summary>
        /// 獲取IP地址
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


    }

    /// <summary>
    /// 别的系统请不要调用
    /// {4800C066-A10B-40A7-B6BA-B9F91CCE8DE6}
    /// </summary>
    internal static class OnlyForLogExtend
    {


        #region JSON序列化，反序列化 {1AC5559B-3615-4EA6-B15B-B6374B28D7A1}
        /// <summary>
        /// 使用json序列化为字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isNeedFormat">默认false</param>
        /// <param name="isCanCyclicReference">默认true,生成的json每个对象会自动加上类似 "$id":"1","$ref": "1"</param>
        /// <param name="dateTimeFormat">默认null,即使用json.net默认的序列化机制，如："\/Date(1439335800000+0800)\/"</param>
        /// <returns></returns>
        public static string ToJson(this object input, bool isNeedFormat = false, bool isCanCyclicReference = true, string dateTimeFormat = null)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            };

            if (isCanCyclicReference)
            {
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            }

            if (!string.IsNullOrWhiteSpace(dateTimeFormat))
            {
                var jsonConverter = new List<JsonConverter>()
                {
                    new Newtonsoft.Json.Converters.IsoDateTimeConverter(){ DateTimeFormat = dateTimeFormat }//如： "yyyy-MM-dd HH:mm:ss"
                };
                settings.Converters = jsonConverter;
            }

            var format = isNeedFormat ? Formatting.Indented : Formatting.None;

            var json = JsonConvert.SerializeObject(input, format, settings);
            return json;
        }

        /// <summary>
        /// 从序列化字符串里反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="dateTimeFormat">默认null,即使用json.net默认的序列化机制</param>
        /// <returns></returns>
        public static T FromJson<T>(this string input, string dateTimeFormat = null)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };

            if (!string.IsNullOrWhiteSpace(dateTimeFormat))
            {
                var jsonConverter = new List<JsonConverter>()
                {
                    new Newtonsoft.Json.Converters.IsoDateTimeConverter(){ DateTimeFormat = dateTimeFormat }//如： "yyyy-MM-dd HH:mm:ss"
                };
                settings.Converters = jsonConverter;
            }

            return JsonConvert.DeserializeObject<T>(input, settings);
        }
        #endregion


        public static byte[] MySQLCompress(this string str, Ionic.Zlib.CompressionLevel level = Ionic.Zlib.CompressionLevel.Default)
        {
            return UTF8Encoding.UTF8.GetBytes(str).MySQLCompress(level);
        }

        public static byte[] MySQLCompress(this byte[] buffer, Ionic.Zlib.CompressionLevel level = Ionic.Zlib.CompressionLevel.Default)
        {
            using (var output = new MemoryStream())
            {
                output.Write(BitConverter.GetBytes((int)buffer.Length), 0, 4);
                using (var compressor = new Ionic.Zlib.ZlibStream(output, Ionic.Zlib.CompressionMode.Compress, level))
                {
                    compressor.Write(buffer, 0, buffer.Length);
                }

                return output.ToArray();
            }
        }

        public static string MySQLUncompress(this byte[] buffer)
        {
            return UTF8Encoding.UTF8.GetString(buffer.MySQLUncompressBuffer());
        }

        public static byte[] MySQLUncompressBuffer(this byte[] buffer)
        {
            using (var output = new MemoryStream())
            {
                using (var decompressor = new Ionic.Zlib.ZlibStream(output, Ionic.Zlib.CompressionMode.Decompress))
                {
                    decompressor.Write(buffer, 4, buffer.Length - 4);
                }

                return output.ToArray();
            }
        }

        #region C#字符串压缩与解压缩 {4800C066-A10B-40A7-B6BA-B9F91CCE8DE6}

        /// <summary>
        /// GZipStream压缩字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] GZipCompress(this string str)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);

            using (MemoryStream msTemp = new MemoryStream())
            {
                using (GZipStream gz = new GZipStream(msTemp, CompressionMode.Compress, true))
                {
                    gz.Write(buffer, 0, buffer.Length);
                    gz.Close();

                    return msTemp.GetBuffer();
                }
            }
        }

        /// <summary>
        /// GZipStream解压字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GZipDecompress(this byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                byte[] buffer = new byte[0x1000];
                int length = 0;

                using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
                {
                    using (MemoryStream msTemp = new MemoryStream())
                    {
                        while ((length = gz.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            msTemp.Write(buffer, 0, length);
                        }

                        return System.Text.Encoding.UTF8.GetString(msTemp.ToArray());
                    }
                }
            }
        }
        #endregion


    }
}
