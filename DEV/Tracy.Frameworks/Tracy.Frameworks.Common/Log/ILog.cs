using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：5/24/2014 1:32:35 PM
 * 描述说明：日志公共接口
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Log
{
	public interface ILog
	{
        /// <summary>
        /// 记录一般信息
        /// </summary>
        /// <param name="msg"></param>
        void Info(string msg);

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="msg"></param>
        void Debug(string msg);

        /// <summary>
        /// 记录警告信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        void Warn(string msg, Exception ex);

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="ex"></param>
        void Error(string msg, Exception ex);
	}
}
