using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Web.Mvc;
using System.Linq.Expressions;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/7/4 11:54:05
 * 描述说明：Label擴展
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Mvc.Extends
{
    public static class LabelExtension
    {
        /// <summary>
        /// 返回一个HTML label标签，标签内容来源于输入的参数中表达式所指定的属性及前后缀
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="shell"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, LabelInShell shell)
        {
            return html.LabelFor(expression, null, shell);
        }

        /// <summary>
        /// 返回一个HTML label标签，标签内容来源于输入的参数中表达式所指定的属性及前后缀
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="labelText"></param>
        /// <param name="shell"></param>
        /// <returns></returns>
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, LabelInShell shell)
        {
            if (html == null)
                throw new ArgumentNullException("html");
            return LabelHelper(html, ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData), ExpressionHelper.GetExpressionText(expression), shell, labelText);
        }

        /// <summary>
        /// 内部处理
        /// </summary>
        /// <param name="html"></param>
        /// <param name="metadata"></param>
        /// <param name="htmlFieldName"></param>
        /// <param name="shell"></param>
        /// <param name="labelText"></param>
        /// <returns></returns>
        internal static MvcHtmlString LabelHelper(HtmlHelper html, ModelMetadata metadata, string htmlFieldName, LabelInShell shell, string labelText = null)
        {
            string str = labelText ?? (metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new char[] { '.' }).Last<string>()));
            if (string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Empty;
            }
            string prefix = string.Empty;
            string postfix = string.Empty;
            FixStrExplain(shell, ref prefix, ref postfix);
            str = string.Format("{0}{1}{2}", prefix, str, postfix);
            TagBuilder tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.InnerHtml = str;
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
        }
        private static void FixStrExplain(LabelInShell shell, ref string prefix, ref string postfix)
        {
            if ((shell & LabelInShell.StarBefore) > 0)
                prefix += "<span class='red'>*</span>";
            if ((shell & LabelInShell.StarAfter) > 0)
                postfix += "<span class='red'>*</span>";
            if ((shell & LabelInShell.ColonAfter) > 0)
                postfix += ":";
        }

    }

    /// <summary>
    /// 标签显示文字的常用前后缀包围类型
    /// </summary>
    [DataContract]
    [Flags]
    public enum LabelInShell
    {
        /// <summary>
        /// 后缀冒号
        /// </summary>
        [Description("后缀冒号")]
        [EnumMember]
        ColonAfter = 1,

        /// <summary>
        /// 前缀红色星号
        /// </summary>
        [Description("前缀红色星号")]
        [EnumMember]
        StarBefore = 2,

        /// <summary>
        /// 后缀红色星号
        /// </summary>
        [Description("后缀红色星号")]
        [EnumMember]
        StarAfter = 4
    }
}
