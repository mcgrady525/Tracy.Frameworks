using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/6/5 12:46:38
 * 描述说明：参数验证扩展类
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Extends
{
    public static class ArgumentExtension
    {
        /// <summary>
        /// 字符串类型参数不为空
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        public static void RequireNotNullOrEmpty(this string value, string argumentName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The value can't be null or empty", argumentName);
            }
        }

        /// <summary>
        /// object类型参数不空null
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        public static void RequireNotNull(this object value, string argumentName)
        {
            if (value == null)
            {
                throw new ArgumentException("The value can't be null", argumentName);
            }
        }
    }
}
