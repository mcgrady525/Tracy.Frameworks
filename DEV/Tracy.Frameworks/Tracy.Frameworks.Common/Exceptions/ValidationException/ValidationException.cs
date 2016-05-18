using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;
using Tracy.Frameworks.Common.Extends;
using System.Linq.Expressions;
using Tracy.Frameworks.Common.Serialization;

namespace Tracy.Frameworks.Common.Exceptions.ValidationException
{
    ///// <summary>
    ///// 后台校验异常(非泛型)
    ///// </summary>
    //[Serializable]
    //[DataContract]
    //public class ValidationExceptionLite : System.ServiceModel.FaultException
    //{
    //    /// <summary>
    //    /// 初始化后台校验异常(非泛型)的新實例
    //    /// </summary>
    //    /// <param name="exception"></param>
    //    public ValidationExceptionLite(ValidationException exception)
    //        : base(string.Format("{0}\r\n{1}\r\n{2}", exception.Message, StandardSerializer.GetString(exception.Detail), exception.ToString()),
    //        new FaultCode("xxxSZ.Frameworks.Exceptions.ValidationException.ValidationExceptionLite"),
    //        exception.Action) { this.Source = exception.Source; }

    //    /// <summary>
    //    /// 初始化后台校验异常(非泛型)的新實例
    //    /// </summary>
    //    /// <param name="details"></param>
    //    /// <param name="message"></param>
    //    /// <param name="reason"></param>
    //    /// <param name="action"></param>
    //    /// <param name="source"></param>
    //    public ValidationExceptionLite(List<ValidationResult> details, string message, string reason, string action, string source = "")
    //        : base(string.Format("{0}\r\n{1}\r\n{2}", message, StandardSerializer.GetString(details), reason),
    //        new FaultCode("xxxSZ.Frameworks.Exceptions.ValidationException.ValidationExceptionLite"),
    //        action) { if (!string.IsNullOrWhiteSpace(source)) this.Source = source; }

    //    ///// <summary>
    //    ///// 初始化后台校验异常(非泛型)的新實例
    //    ///// </summary>
    //    //public ValidationExceptionLite() : base() { }

    //    ///// <summary>
    //    ///// 初始化后台校验异常(非泛型)的新實例
    //    ///// </summary>
    //    ///// <param name="reason"></param>
    //    ///// <param name="code"></param>
    //    ///// <param name="action"></param>
    //    //public ValidationExceptionLite(string reason, FaultCode code, string action) : base(reason, code, action) { }
    //}

    /// <summary>
    /// 后台校验异常
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(ValidationResult))]
    [KnownType(typeof(ValidationError))]
    [KnownType(typeof(ValidationEntry))]
    [KnownType(typeof(ValidationEntityState))]
    public class ValidationException : System.ServiceModel.FaultException<List<ValidationResult>>
    {
        /// <summary>
        /// 拋出后台校验异常
        /// </summary>
        /// <typeparam name="TEnumReturnCode">返回代码枚舉類型</typeparam>
        /// <param name="returnCode">返回代码</param>
        /// <param name="propertyName">发生错误的具体属性</param>
        /// <param name="entity">发生错误的对象</param>
        /// <param name="state">对象状态</param>
        public static void Throw<TEnumReturnCode>(TEnumReturnCode returnCode, object entity = null, string propertyName = "", ValidationEntityState state = 0)
            where TEnumReturnCode : struct
        {
            throw new ValidationException(
                entity,
                string.Format(returnCode.GetDescription(), (entity == null ? propertyName : entity.GetType().GetProperty(propertyName).GetDescription())),
                state,
                propertyName
                );
        }

        /// <summary>
        /// 拋出后台校验异常
        /// </summary>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="propertyName">发生错误的具体属性</param>
        /// <param name="entity">发生错误的对象</param>
        /// <param name="state">对象状态</param>
        public static void Throw(string errorMessage, object entity = null, string propertyName = "", ValidationEntityState state = 0)
        {
            throw new ValidationException(entity, errorMessage, state, propertyName);
        }

        /// <summary>
        /// 拋出后台校验异常
        /// </summary>
        /// <typeparam name="TEnumReturnCode">返回代码枚舉類型</typeparam>
        /// <typeparam name="TFunc">发生错误的具体属性的類型</typeparam>
        /// <param name="returnCode">返回代码</param>
        /// <param name="propertyExpression">指定发生错误的具体属性的表達式</param>
        /// <param name="entity">发生错误的对象</param>
        /// <param name="state">对象状态</param>
        public static void Throw<TEnumReturnCode, TFunc>(TEnumReturnCode returnCode, object entity, Expression<Func<TFunc>> propertyExpression, ValidationEntityState state = 0)
            where TEnumReturnCode : struct
        {
            var propertyName = PropertyExtension.ExtractPropertyName(propertyExpression);
            throw new ValidationException(
                entity,
                string.Format(returnCode.GetDescription(), (entity == null ? propertyName : entity.GetType().GetProperty(propertyName).GetDescription())),
                state,
                propertyName
                );
        }

        /// <summary>
        /// 拋出后台校验异常
        /// </summary>
        /// <typeparam name="TFunc">发生错误的具体属性的類型</typeparam>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="propertyExpression">指定发生错误的具体属性的表達式</param>
        /// <param name="entity">发生错误的对象</param>
        /// <param name="state">对象状态</param>
        public static void Throw<TFunc>(string errorMessage, object entity, Expression<Func<TFunc>> propertyExpression, ValidationEntityState state = 0)
        {
            var propertyName = PropertyExtension.ExtractPropertyName(propertyExpression);
            throw new ValidationException(entity, errorMessage, state, propertyName);
        }

        /// <summary>
        /// 后台校验异常標示
        /// </summary>
        private static readonly FaultCode ValidationFaultCode = new FaultCode(typeof(ValidationException).FullName);

        /// <summary>
        /// 初始化后台校验异常的新实例
        /// </summary>
        /// <param name="entity">发生错误的对象</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="state">对象状态</param>
        /// <param name="propertyName">发生错误的具体属性</param>
        public ValidationException(object entity, string errorMessage, ValidationEntityState state = 0, string propertyName = "")
            : base(new List<ValidationResult> {
                    new ValidationResult(
                        new List<ValidationError>
                        {
                            new ValidationError(propertyName, errorMessage)
                        },
                        new ValidationEntry(entity, state)
                        )}, errorMessage, ValidationFaultCode) { if (entity != null) this.Source = entity.GetType().FullName; }

        /// <summary>
        /// 初始化后台校验异常的新实例
        /// </summary>
        /// <param name="detail">验证结果详情</param>
        public ValidationException(List<ValidationResult> detail) : base(detail, StandardSerializer.GetString(detail), ValidationFaultCode) { }

        /// <summary>
        /// 初始化后台校验异常的新实例
        /// </summary>
        /// <param name="detail">验证结果详情</param>
        /// <param name="reason">附加原因描述</param>
        public ValidationException(List<ValidationResult> detail, string reason) : base(detail, reason, ValidationFaultCode) { }

        /// <summary>
        /// 初始化后台校验异常的新实例
        /// </summary>
        /// <param name="detail">验证结果详情</param>
        /// <param name="reason">附加原因描述</param>
        /// <param name="action">活动信息</param>
        public ValidationException(List<ValidationResult> detail, string reason, string action) : base(detail, reason, ValidationFaultCode, action) { }
    }
}
