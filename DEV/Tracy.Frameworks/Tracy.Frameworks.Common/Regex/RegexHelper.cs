using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：6/5/2014 10:33:06 PM
 * 描述说明：正则表达式辅助类
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common
{
    public class RegexHelper
    {
        /// <summary>  
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true  
        /// </summary>  
        /// <param name="input">输入字符串</param>  
        /// <param name="pattern">模式字符串</param>          
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>  
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true  
        /// </summary>  
        /// <param name="input">输入的字符串</param>  
        /// <param name="pattern">模式字符串</param>  
        /// <param name="options">筛选条件</param>  
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
    }
}
