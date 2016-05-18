using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Translation.Translate
{
    /// <summary>
    /// 轉換類型
    /// </summary>
    public enum TranslationType
    {
        // 摘要: 
        //     簡體中文 -> 繁體中文
        SimplifiedToTraditional = 0,
        //
        // 摘要: 
        //     繁體中文 -> 簡體中文
        TraditionalToSimplified = 1,

        /// <summary>
        /// 不进行转换
        /// </summary>
        None = 2
    }
}
