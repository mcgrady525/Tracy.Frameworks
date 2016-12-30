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
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string SendRequestByHttpWebRequest(string url)
        {
            var result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "zh-cn");
            
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
