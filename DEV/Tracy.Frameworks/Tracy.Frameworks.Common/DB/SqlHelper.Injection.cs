using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.DB
{
    /// <summary>
    /// SQL Server防注入类
    /// </summary>
    public sealed partial class SqlHelper
    {
        /// <summary>
        /// 关键字过滤
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        /// <returns>返回True就是找到了可能sql注入的关键字</returns>
        public static bool IsFilterKeyWords(string originalString)
        {
            //参考：technet.microsoft.com/zh-cn/library/ms161953.aspx
            if (originalString.IndexOf(";") != -1 || originalString.IndexOf("'") != -1 || originalString.IndexOf("--") != -1 || originalString.IndexOf("/*") != -1 || originalString.IndexOf("*/") != -1 || originalString.IndexOf("xp_cmdshell") != -1)
                return true;
            else
                return false;
        }
    }
}
