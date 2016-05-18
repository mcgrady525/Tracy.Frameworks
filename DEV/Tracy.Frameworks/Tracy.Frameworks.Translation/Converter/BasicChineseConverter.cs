using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Tracy.Frameworks.Translation.Translate;

namespace Tracy.Frameworks.Translation.Converter
{
    public static class BasicChineseConverter
    {
        /// <summary>
        /// 執行字元對應的簡繁轉換
        /// </summary>
        /// <param name="text">輸入內容</param>
        /// <param name="direction">轉換方向</param>
        /// <returns></returns>
        public static string Convert(string text, TranslationDirection direction)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            uint mapFlag = (uint)((direction == TranslationDirection.TraditionalToSimplified) ? 0x2000000 : 0x4000000);
            int bytesCount = (text.Length * 2) + 2;
            IntPtr lpDestStr = Marshal.AllocHGlobal(bytesCount);
            if (IntPtr.Zero == lpDestStr) throw new InsufficientMemoryException("Can't alloc enough memory while xxxSZ.Translation.Converter.BasicChineseConverter.Convert call LCMapString.");
            NativeMethods.LCMapString(0x804, mapFlag, text, -1, lpDestStr, bytesCount);
            string result = Marshal.PtrToStringUni(lpDestStr);
            Marshal.FreeHGlobal(lpDestStr);
            if (direction == TranslationDirection.TraditionalToSimplified) //特殊處理字符
            {
                result = result.Replace("聖", "圣").Replace("後", "后");
            }
            else
            {
                result = result.Replace("圣", "聖").Replace("后", "後");
            }
            return result.Replace("臺", "台");
        }
    }
}
