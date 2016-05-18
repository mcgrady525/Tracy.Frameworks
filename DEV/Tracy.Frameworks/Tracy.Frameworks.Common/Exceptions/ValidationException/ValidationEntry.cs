using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.Common.Exceptions.ValidationException
{
    /// <summary>
    /// 后台验证所验证的对象
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(ValidationEntityState))]
    public class ValidationEntry : IEquatable<ValidationEntry>
    {
        /// <summary>
        /// 初始化后台验证所验证的对象的新实例
        /// </summary>
        /// <param name="entity">对象</param>
        /// <param name="state">对象状态</param>
        public ValidationEntry(object entity, ValidationEntityState state = 0)
        {
            this.Entity = entity;
            this.State = state;
        }

        /// <summary>
        /// 对象
        /// </summary>
        [IgnoreDataMember]
        public object Entity { get; protected set; }

        /// <summary>
        /// 对象状态
        /// </summary>
        [DataMember]
        public ValidationEntityState State { get; set; }

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="other">另一个ValidationEntry</param>
        /// <returns>返回两个ValidationEntry是否相等</returns>
        public bool Equals(ValidationEntry other)
        {
            return this.Entity.GetHashCode() == other.Entity.GetHashCode() && this.State == other.State;
        }
    }
}
