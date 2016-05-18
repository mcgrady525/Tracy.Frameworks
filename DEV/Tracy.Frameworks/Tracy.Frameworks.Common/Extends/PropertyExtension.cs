using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace Tracy.Frameworks.Common.Extends
{
    /// <summary>
    /// 属性扩展类
    /// </summary>
    public static class PropertyExtension
    {
        /// <summary>
        /// 獲取屬性值，主要用於頁面行內代碼中綁定數據時獲取複雜對象中的屬性值時使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName">屬性名</param>
        /// <returns></returns>
        public static object GetPropertyValue<T>(this T entity, string propertyName)
            where T : class
        {
            return entity.GetType().GetProperty(propertyName).GetValue(entity, null);
        }

        /// <summary>
        /// 獲取屬性值格式化字符串，主要用於頁面行內代碼中綁定數據時獲取複雜對象中的屬性值時使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName">屬性名</param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string GetPropertyValue<T>(this T entity, string propertyName, string format)
            where T : class
        {
            object propertyValue = entity.GetPropertyValue(propertyName);
            if ((propertyValue == null) || (propertyValue == DBNull.Value))
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(format))
            {
                return propertyValue.ToString();
            }
            return string.Format(format, propertyValue);
        }

        /// <summary>
        /// 从属性表达式中提取属性名
        /// </summary>
        /// <typeparam name="T">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，形如(p => p.PropertyName)</param>
        /// <returns>返回属性名称</returns>
        /// <exception cref="ArgumentNullException">当表达式 <paramref name="propertyExpression"/> 为null时抛出此异常.</exception>
        /// <exception cref="ArgumentException">当表达式:<br/>
        ///     不是与 <see cref="MemberExpression"/> 兼容的类型<br/>
        ///     <see cref="MemberExpression"/> 不返回一个属性<br/>
        ///     或属性的Get访问器为静态时<br/>
        ///     抛出此异常.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures")]
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (null == propertyExpression) throw new ArgumentNullException("propertyExpression");

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (null == memberExpression)
                throw new ArgumentException("Not member access expression.", "propertyExpression");

            var property = memberExpression.Member as PropertyInfo;
            if (null == property)
                throw new ArgumentException("Expression not property.", "propertyExpression");

            var getMethod = property.GetGetMethod(true);
            if (getMethod.IsStatic)
                throw new ArgumentException("Static expression.", "propertyExpression");

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// 获取屬性的 Description 或 DisplayName 特性值
        /// </summary>
        /// <param name="prop">屬性</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string GetDescription(this PropertyInfo prop)
        {
            if (prop == null)
            {
                return string.Empty;
            }
            try
            {
                DescriptionAttribute da = prop.GetCustomAttributes(true).OfType<DescriptionAttribute>().FirstOrDefault();
                if (da != null && !string.IsNullOrEmpty(da.Description))
                    return da.Description;
                else
                {
                    DisplayNameAttribute dna = prop.GetCustomAttributes(true).OfType<DisplayNameAttribute>().FirstOrDefault();
                    if (dna != null && !string.IsNullOrEmpty(dna.DisplayName))
                        return dna.DisplayName;
                }
            }
            catch
            {
            }
            return prop.Name;
        }
    }
}
