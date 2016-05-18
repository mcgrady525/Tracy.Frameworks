using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.Common.Exceptions.ValidationException
{
    ///// <summary>
    ///// 后台验证结果集合
    ///// </summary>
    //[Serializable]
    //[CollectionDataContract]
    //[KnownType(typeof(ValidationResult))]
    //public class ValidationResultDetailCollection : List<ValidationResult>
    //{
    //}

    /// <summary>
    /// 后台验证结果
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(ValidationError))]
    public class ValidationResult
    {
        /// <summary>
        /// 初始化后台验证结果的新实例
        /// </summary>
        /// <param name="errors">错误明细</param>
        /// <param name="entry">所验证的对象信息</param>
        /// <param name="isValid">是否验证通过</param>
        public ValidationResult(List<ValidationError> errors, ValidationEntry entry = null, bool isValid = false)
        {
            this.Entry = entry;
            this.Errors = errors;
            this.IsValid = isValid;
        }

        /// <summary>
        /// 所验证的对象信息
        /// </summary>
        [DataMember]
        public ValidationEntry Entry { get; protected set; }

        /// <summary>
        /// 是否验证通过
        /// </summary>
        [DataMember]
        public bool IsValid { get; protected set; }

        /// <summary>
        /// 错误明细
        /// </summary>
        [DataMember]
        public List<ValidationError> Errors { get; protected set; }
    }
}
