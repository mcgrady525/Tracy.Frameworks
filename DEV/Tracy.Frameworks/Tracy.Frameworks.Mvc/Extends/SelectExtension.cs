using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Tracy.Frameworks.Common.Extends;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/7/4 11:43:20
 * 描述说明：列表控件擴展
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Mvc.Extends
{
    public static class SelectExtension
    {
        #region RadioButtonList

        /// <summary>
        /// 返回一组HTML input(radio)标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.RadioButtonListFor(expression, typeof(TProperty), null, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一组HTML input(radio)标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.RadioButtonListFor(expression, typeof(TProperty), htmlAttributes, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一组HTML input(radio)标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.RadioButtonListFor(expression, enumType, null, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一组HTML input(radio)标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, IDictionary<string, object> htmlAttributes, object defaultValue = null, string allOptionStr = "")
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = enumType.GetGenericArguments()[0];
            }
            object defaultVal = null;
            MethodInfo mi;
            SelectProcess(expression, enumType, defaultValue, ref defaultVal, out mi);

            var checkedhtmlAttributes = new Dictionary<string, object>();
            checkedhtmlAttributes.Add("Checked", "checked");
            if (htmlAttributes != null && htmlAttributes.Count > 0)
            {
                if (htmlAttributes.Keys.Contains("Checked"))
                    htmlAttributes.Remove("Checked");
                foreach (var attr in htmlAttributes)
                {
                    checkedhtmlAttributes.Add(attr.Key, attr.Value);
                }
            }

            bool flag = true;
            List<string> strList = new List<string>();
            foreach (object item in Enum.GetValues(enumType))
            {
                var value = mi.Invoke(null, new object[] { item });
                if (flag && defaultVal != null && Enum.Equals(item, defaultVal))
                {
                    strList.Add(string.Concat("<label>", htmlHelper.RadioButtonFor(expression, value, checkedhtmlAttributes), item.GetDescription(), "</label>"));
                    flag = false;
                }
                else
                {
                    strList.Add(string.Concat("<label>", htmlHelper.RadioButtonFor(expression, value, htmlAttributes), item.GetDescription(), "</label>"));
                }
            }

            if (!string.IsNullOrEmpty(allOptionStr))
                strList.Insert(0, string.Concat(htmlHelper.RadioButtonFor(expression, "", (flag ? checkedhtmlAttributes : htmlAttributes)), allOptionStr));

            return new MvcHtmlString(string.Join("&nbsp;", strList));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="radioOptions"></param>
        /// <param name="htmlAttributes"></param>

        /// <returns></returns>
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> radioOptions, IDictionary<string, object> htmlAttributes)
        {
            if (radioOptions == null)
            {
                throw new ArgumentNullException("radioOptions");
            }
            var htmlFullFieldName = helper.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
            ModelState state;
            var modelValue = "";
            if (helper.ViewData.ModelState.TryGetValue(htmlFullFieldName, out state) && (state.Value != null))
            {
                modelValue = (string)state.Value.ConvertTo(typeof(string), null);
            }
            else
            {
                modelValue = Convert.ToString(helper.ViewData.Eval(htmlFullFieldName));
            }
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(modelValue))
            {
                foreach (var i in radioOptions)
                {
                    i.Selected = modelValue == i.Value;
                }
            }
            foreach (var i in radioOptions)
            {

                if (i.Selected)
                {
                    if (!htmlAttributes.ContainsKey("checked"))
                    {
                        htmlAttributes.Add("checked", "checked");
                    }
                }

                sb.Append(helper.RadioButtonFor(expression, i.Value, htmlAttributes));
                if (htmlAttributes.ContainsKey("checked"))
                {
                    htmlAttributes.Remove("checked");
                }
                sb.Append(i.Text);
            }
            return new MvcHtmlString(sb.ToString());
        }

        #endregion

        #region DropdownList

        /// <summary>
        /// 返回一个HTML select标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.DropDownListFor(expression, typeof(TProperty), null, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一个HTML select标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.DropDownListFor(expression, typeof(TProperty), htmlAttributes, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一个HTML select标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.DropDownListFor(expression, enumType, null, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一个HTML select标签，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <param name="lstExcludeValue">除了默认值，在这里的都将被删除</param>
        /// <param name="lstOnlyIncludeValue">除了默认值，不在这里的都将被删除</param>
        /// <returns></returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            Type enumType,
            IDictionary<string, object> htmlAttributes,
            object defaultValue = null,
            string allOptionStr = "",
            List<string> lstExcludeValue = null,
            List<string> lstOnlyIncludeValue = null
            )
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = enumType.GetGenericArguments()[0];
            }
            object defaultVal = null;
            MethodInfo mi;
            SelectProcess(expression, enumType, defaultValue, ref defaultVal, out mi);

            var items = Enum.GetValues(enumType).Cast<Enum>()
                .Select(p => new SelectListItem
                {
                    Selected = (defaultVal != null && Enum.Equals(p, defaultVal)),
                    Text = p.GetDescription(),
                    Value = mi.Invoke(null, new object[] { p }).ToString()
                }).ToList();

            if (lstExcludeValue.HasValue())
            {
                items.RemoveAll(p => lstExcludeValue.Contains(p.Value));
            }
            if (lstOnlyIncludeValue.HasValue())
            {
                items.RemoveAll(p => !lstOnlyIncludeValue.Contains(p.Value));
            }

            if (!string.IsNullOrEmpty(allOptionStr))
                items.Insert(0, new SelectListItem { Selected = (!items.Any(p => p.Selected == true)), Text = allOptionStr, Value = "" });

            var mvcStr = htmlHelper.DropDownListFor(expression,
                items,
                htmlAttributes);

            return mvcStr;
        }

        #endregion

        #region ListBoxFor

        /// <summary>
        /// 返回一个HTML select标签(multiple)，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString ListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.ListBoxFor(expression, typeof(TProperty), null, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一个HTML select标签(multiple)，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString ListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.ListBoxFor(expression, typeof(TProperty), htmlAttributes, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一个HTML select标签(multiple)，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString ListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, object defaultValue = null, string allOptionStr = "")
        {
            return htmlHelper.ListBoxFor(expression, enumType, null, defaultValue, allOptionStr);
        }

        /// <summary>
        /// 返回一个HTML select标签(multiple)，标签列表内容来源于输入的枚举类型的各项的Description或DisplayName特性以及各项的数值值
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="enumType"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="defaultValue"></param>
        /// <param name="allOptionStr"></param>
        /// <returns></returns>
        public static MvcHtmlString ListBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Type enumType, IDictionary<string, object> htmlAttributes, object defaultValue = null, string allOptionStr = "")
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                enumType = enumType.GetGenericArguments()[0];
            }
            object defaultVal = null;
            MethodInfo mi;
            SelectProcess(expression, enumType, defaultValue, ref defaultVal, out mi);

            var items = Enum.GetValues(enumType).Cast<Enum>()
                .Select(p => new SelectListItem
                {
                    Selected = (defaultVal != null && Enum.Equals(p, defaultVal)),
                    Text = p.GetDescription(),
                    Value = mi.Invoke(null, new object[] { p }).ToString()
                }).ToList();

            if (!string.IsNullOrEmpty(allOptionStr))
                items.Insert(0, new SelectListItem { Selected = (!items.Any(p => p.Selected == true)), Text = allOptionStr, Value = "" });

            var mvcStr = htmlHelper.ListBoxFor(expression,
                items,
                htmlAttributes);
            return mvcStr;
        }

        #endregion

        private static void SelectProcess<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression, Type enumType, object defaultValue,
            ref object defaultVal, out MethodInfo mi)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!enumType.IsEnum)
                throw new ArithmeticException("'enumType' must be a enum or a nullable enum.");

            string method = string.Concat("To", Enum.GetUnderlyingType(enumType).Name);
            mi = typeof(Convert).GetMethod(method, new Type[] { enumType });
            if (defaultValue != null)
            {
                try
                {
                    defaultVal = Enum.Parse(enumType, defaultValue.ToString());
                }
                catch { }
            }
        }
    }
}
