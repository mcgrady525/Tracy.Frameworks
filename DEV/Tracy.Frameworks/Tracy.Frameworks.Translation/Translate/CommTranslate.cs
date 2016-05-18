using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tracy.Frameworks.Translation.Converter;

namespace Tracy.Frameworks.Translation.Translate
{
    /// <summary>
    /// 简繁转换包装类
    /// </summary>
    public static class CommTranslate
    {
        /// <summary>
        /// 将输入内容字对字转换为简体中文，不进行术语替换，也不进行分词
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string BasicToSimplified(string input)
        {
            if (null == input)
            {
                throw new ArgumentNullException("input");
            }
            return BasicChineseConverter.Convert(input, TranslationDirection.TraditionalToSimplified);
        }

        /// <summary>
        /// 将输入内容字对字转换为繁体中文，不进行术语替换，也不进行分词
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string BasicToTraditional(string input)
        {
            if (null == input)
            {
                throw new ArgumentNullException("input");
            }
            return BasicChineseConverter.Convert(input, TranslationDirection.SimplifiedToTraditional);
        }
    }
}
