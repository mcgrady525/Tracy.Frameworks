using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/6/11 17:12:56
 * 描述说明：
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Common.Translation
{
    public class NativeMethods
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
