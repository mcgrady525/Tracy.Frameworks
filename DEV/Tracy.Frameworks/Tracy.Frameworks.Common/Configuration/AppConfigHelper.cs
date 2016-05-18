using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

namespace Tracy.Frameworks.Common.Configuration
{
    /// <summary>
    /// 描述:配置文件操作辅助类
    /// 作者:鲁宁
    /// 时间:2013/8/15 14:02:23
    /// </summary>
    public class AppConfigHelper
    {
        /// <summary>
        /// 获取应用程序根目录路径
        /// </summary>
        public static readonly string BASEDIRECTORY = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// 获取配置文件中指定键的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>键的值</returns>
        public static string GetAppSetting(string key)
        {
            if (ConfigurationManager.AppSettings.Count == 0 || !ConfigurationManager.AppSettings.HasKeys()) return string.Empty;
            if (!ConfigurationManager.AppSettings.AllKeys.Any(p => p.Equals(key))) return string.Empty;

            return ConfigurationManager.AppSettings[key];
        }
    }
}
