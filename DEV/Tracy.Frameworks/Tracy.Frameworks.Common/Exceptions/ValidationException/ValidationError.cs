using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.Common.Exceptions.ValidationException
{
    /// <summary>
    /// 后台验证错误
    /// </summary>
    [Serializable]
    [DataContract]
    public class ValidationError
    {
        /// <summary>
        /// 初始化后台验证错误的新实例
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        public ValidationError(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 初始化后台验证错误的新实例
        /// </summary>
        /// <param name="propertyName">所验证的属性名称</param>
        /// <param name="errorMessage">错误信息</param>
        public ValidationError(string propertyName, string errorMessage = "")
        {
            this.PropertyName = propertyName;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// 所验证的属性名称
        /// </summary>
        [DataMember]
        public string PropertyName { get; protected set; }
    }
}
