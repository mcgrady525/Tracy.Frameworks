using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2014/7/4 12:45:05
 * 描述说明：
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Mvc.Extends
{
    public static class InputExtension
    {
        /// <summary>
        /// 输出Input控件，并应用上指定的灰色注释文本
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string notice)
        {
            return htmlHelper.TextBoxFor(expression, ((IDictionary<string, object>)null), notice);
        }

        /// <summary>
        /// 输出Input控件，并应用上指定的灰色注释文本
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string notice)
        {
            return htmlHelper.TextBoxFor(expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)), notice);
        }

        /// <summary>
        /// 输出Input控件，并应用上指定的灰色注释文本
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string notice)
        {
            if (!string.IsNullOrWhiteSpace(notice))
                AddNotice(ref htmlAttributes, notice);
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        private static void AddNotice(ref IDictionary<string, object> htmlAttributes, string content)
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (htmlAttributes.Keys.Contains("mod") && !htmlAttributes["mod"].ToString().Contains("notice"))
                htmlAttributes["mod"] = (htmlAttributes["mod"].ToString() + (string.IsNullOrWhiteSpace(htmlAttributes["mod"].ToString()) ? "notice" : "|notice"));
            else
                htmlAttributes.Add("mod", "notice");
            if (htmlAttributes.Keys.Contains("mod_notice_tip"))
                htmlAttributes["mod_notice_tip"] = content;
            else
                htmlAttributes.Add("mod_notice_tip", content);
        }

        /// <summary>
        /// 输出HTML时间选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="param"></param>
        /// <param name="notice"></param>
        /// <param name="isDoubleCalendar"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static MvcHtmlString DateTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
            string param = "dateFmt:'yyyy-MM-dd',startDate:'%y-%M-%d'", string notice = "", bool isDoubleCalendar = true, string skin = "xxx")
        {
            return htmlHelper.DateTimePickerFor(expression, ((IDictionary<string, object>)null), param, notice, isDoubleCalendar, skin);
        }

        /// <summary>
        /// 输出HTML时间选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="param"></param>
        /// <param name="notice"></param>
        /// <param name="isDoubleCalendar"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static MvcHtmlString DateTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes,
            string param = "dateFmt:'yyyy-MM-dd',startDate:'%y-%M-%d'", string notice = "", bool isDoubleCalendar = true, string skin = "xxx")
        {
            return htmlHelper.DateTimePickerFor(expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)), param, notice, isDoubleCalendar, skin);
        }

        /// <summary>
        /// 输出HTML时间选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="param"></param>
        /// <param name="notice"></param>
        /// <param name="isDoubleCalendar"></param>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static MvcHtmlString DateTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes,
            string param = "dateFmt:'yyyy-MM-dd',startDate:'%y-%M-%d'", string notice = "", bool isDoubleCalendar = true, string skin = "xxx")
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (htmlAttributes.Keys.Contains("class") && !htmlAttributes["class"].ToString().Contains("Wdate"))
                htmlAttributes["class"] = (htmlAttributes["class"].ToString() + " Wdate");
            else
                htmlAttributes.Add("class", "Wdate");
            string value = string.Format("WdatePicker({{{0}doubleCalendar:{1},skin:'{2}'}})", string.IsNullOrWhiteSpace(param) ? "" : string.Concat(param.TrimEnd(','), ","), isDoubleCalendar.ToString().ToLower(), skin);
            if (htmlAttributes.Keys.Contains("onfocus"))
                htmlAttributes["onfocus"] = value;
            else
                htmlAttributes.Add("onfocus", value);

            if (!string.IsNullOrWhiteSpace(notice))
                AddNotice(ref htmlAttributes, notice);

            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        /// <summary>
        /// 输出HTML金额选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString MoneyInputFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string notice = "")
        {
            return htmlHelper.MoneyInputFor(expression, ((IDictionary<string, object>)null), notice);
        }

        /// <summary>
        /// 输出HTML金额选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString MoneyInputFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string notice = "")
        {
            return htmlHelper.MoneyInputFor(expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)), notice);
        }

        /// <summary>
        /// 输出HTML金额选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString MoneyInputFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string notice = "")
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (htmlAttributes.Keys.Contains("class") && !htmlAttributes["class"].ToString().Contains("moneyinput"))
                htmlAttributes["class"] = (htmlAttributes["class"].ToString() + " moneyinput");
            else
                htmlAttributes.Add("class", "moneyinput");

            if (!string.IsNullOrWhiteSpace(notice))
                AddNotice(ref htmlAttributes, notice);

            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        /// <summary>
        /// 输出HTML数值选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString NumberPickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string notice = "")
        {
            return htmlHelper.NumberPickerFor(expression, ((IDictionary<string, object>)null), notice);
        }

        /// <summary>
        /// 输出HTML数值选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString NumberPickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string notice = "")
        {
            return htmlHelper.NumberPickerFor(expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)), notice);
        }

        /// <summary>
        /// 输出HTML数值选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString NumberPickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string notice = "")
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (htmlAttributes.Keys.Contains("onkeyup"))
                htmlAttributes["onkeyup"] = "this.value=this.value.replace(/\\D/g,'')";
            else
                htmlAttributes.Add("onkeyup", "this.value=this.value.replace(/\\D/g,'')");
            if (htmlAttributes.Keys.Contains("onafterpaste"))
                htmlAttributes["onafterpaste"] = "this.value=this.value.replace(/\\D/g,'')";
            else
                htmlAttributes.Add("onafterpaste", "this.value=this.value.replace(/\\D/g,'')");

            if (!string.IsNullOrWhiteSpace(notice))
                AddNotice(ref htmlAttributes, notice);

            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        /// <summary>
        /// 输出HTML航空公司选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString AirlineSelectorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string notice = "")
        {
            return htmlHelper.AirlineSelectorFor(expression, ((IDictionary<string, object>)null), notice);
        }

        /// <summary>
        /// 输出HTML航空公司选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString AirlineSelectorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string notice = "")
        {
            return htmlHelper.AirlineSelectorFor(expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)), notice);
        }

        /// <summary>
        /// 输出HTML航空公司选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static MvcHtmlString AirlineSelectorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string notice = "")
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (htmlAttributes.Keys.Contains("class") && !htmlAttributes["class"].ToString().Contains("airlineSelector"))
                htmlAttributes["class"] = (htmlAttributes["class"].ToString() + " airlineSelector");
            else
                htmlAttributes.Add("class", "airlineSelector");

            if (!string.IsNullOrWhiteSpace(notice))
                AddNotice(ref htmlAttributes, notice);

            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        /// <summary>
        /// 输出HTML城市选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="notice"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static MvcHtmlString CitySelectorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string callback = "", string notice = "")
        {
            return htmlHelper.CitySelectorFor(expression, ((IDictionary<string, object>)null), callback, notice);
        }

        /// <summary>
        /// 输出HTML城市选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static MvcHtmlString CitySelectorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string callback = "", string notice = "")
        {
            return htmlHelper.CitySelectorFor(expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)), callback, notice);
        }

        /// <summary>
        /// 输出HTML城市选择器
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="notice"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static MvcHtmlString CitySelectorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string callback = "", string notice = "")
        {
            if (htmlAttributes == null)
                htmlAttributes = new Dictionary<string, object>();
            if (htmlAttributes.Keys.Contains("mod") && !htmlAttributes["mod"].ToString().Contains("cityselector"))
                htmlAttributes["mod"] = (htmlAttributes["mod"].ToString() + (string.IsNullOrWhiteSpace(htmlAttributes["mod"].ToString()) ? "cityselector" : "|cityselector"));
            else
                htmlAttributes.Add("mod", "cityselector");
            if (htmlAttributes.Keys.Contains("mod_address_callback"))
                htmlAttributes["mod_address_callback"] = callback;
            else
                htmlAttributes.Add("mod_address_callback", callback);

            if (!string.IsNullOrWhiteSpace(notice))
                AddNotice(ref htmlAttributes, notice);

            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        /// <summary>
        /// 讓Hidden也可以使用DisplayName設置的格式
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="html"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString HiddenInputFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
        {
            IDictionary<string, object> htmlAttributesTmp = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (htmlAttributesTmp.ContainsKey("type"))
            {
                htmlAttributesTmp["type"] = "hidden";
            }
            else
            {
                htmlAttributesTmp.Add("type", "hidden");
            }

            return html.TextBoxFor(expression, htmlAttributesTmp);
        }
    }
}
