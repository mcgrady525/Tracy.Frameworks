using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Tracy.Frameworks.Translation.Converter
{
    internal class NativeMethods
    {
        /// <summary>
        /// 不同編碼字元轉換
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="dwMapFlags">轉換方向</param>
        /// <param name="lpSrcStr">輸入</param>
        /// <param name="cchSrc"></param>
        /// <param name="lpDestStr">輸出</param>
        /// <param name="cchDest">位元組長度</param>
        /// <returns></returns>
        [DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        public static extern int LCMapString(int locale, uint dwMapFlags, [MarshalAs(UnmanagedType.LPTStr)] string lpSrcStr, int cchSrc, IntPtr lpDestStr, int cchDest);
    }
}
