using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Tracy.Frameworks.LogClient.Helper
{
    /// <summary>
    /// 日志客户端helper
    /// </summary>
    public static class LogClientHelper
    {
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

    }
}
