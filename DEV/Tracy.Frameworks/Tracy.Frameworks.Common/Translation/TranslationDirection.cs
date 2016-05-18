using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/6/11 17:10:25
 * 描述说明：语言转换方向枚举
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Translation
{
    public enum TranslationDirection: byte
    {
        /// <summary>
        /// 簡體中文 -> 繁體中文
        /// </summary>
        SimplifiedToTraditional,

        /// <summary>
        /// 繁體中文 -> 簡體中文
        /// </summary>
        TraditionalToSimplified
    }
}
