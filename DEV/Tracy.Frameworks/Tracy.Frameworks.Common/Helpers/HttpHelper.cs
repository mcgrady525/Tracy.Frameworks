using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Tracy.Frameworks.Common.Helpers
{
    /// <summary>
    /// http请求封装
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 使用HttpWebRequest，适用于.net2.0~4.0
        /// 默认使用POST方式发送数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="jsonData">json格式的数据</param>
        /// <returns></returns>
        public static string SendRequestByHttpWebRequest(string url, string jsonData)
        {
            var result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            //设置request属性
            request.Method = "POST";
            request.ContentType = "application/json";

            //写入data
            using (var streamWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
            {
                streamWriter.Write(jsonData);
                streamWriter.Flush();
                streamWriter.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }

            return result;
        }

        /// <summary>
        /// 使用HttpClient，适用于.net4.5+
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendRequestByHttpClient(string url)
        {
            throw new NotImplementedException();
        }

    }
}
