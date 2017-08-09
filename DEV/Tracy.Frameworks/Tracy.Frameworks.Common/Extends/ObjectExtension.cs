using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// object扩展，包括克隆操作
    /// </summary>
    public static class ObjectExtension
    {
        #region 克隆

        #region 深克隆

        /// <summary>
        /// 深克隆
        /// 先序列化再反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj) where T : class
        {
            return obj != null ? obj.ToJson().FromJson<T>() : null;
        }
        
        #endregion

        #region 浅克隆

        //可以使用Object.MemberwiseClone()方法

        #endregion

        /// <summary>
        /// return obj == null;
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// return obj != null;
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        #endregion
    }
}
