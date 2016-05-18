using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Tracy.Frameworks.Common.Extends;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/6/11 17:11:25
 * 描述说明：中文转换辅助类
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Translation
{
    public class BasicChineseConverter
    {
        /// <summary>
        /// 语言转换
        /// </summary>
        /// <param name="text">输入文本</param>
        /// <param name="direction">转换方向</param>
        /// <returns></returns>
        public static string Convert(string text, TranslationDirection direction)
        {
            text.RequireNotNullOrEmpty("text");

            uint mapFlag = (uint)((direction == TranslationDirection.TraditionalToSimplified) ? 0x2000000 : 0x4000000);
            int bytesCount = (text.Length * 2) + 2;
            IntPtr lpDestStr = Marshal.AllocHGlobal(bytesCount);
            if (IntPtr.Zero == lpDestStr) 
                throw new InsufficientMemoryException("Can't alloc enough memory while xxxSZ.Translation.Converter.BasicChineseConverter.Convert call LCMapString.");
            NativeMethods.LCMapString(0x804, mapFlag, text, -1, lpDestStr, bytesCount);
            string result = Marshal.PtrToStringUni(lpDestStr);
            Marshal.FreeHGlobal(lpDestStr);
            return result;
        }
    }
}
