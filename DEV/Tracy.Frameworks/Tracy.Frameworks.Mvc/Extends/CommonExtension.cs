using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2015/1/29 10:50:33
 * 描述说明：
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Mvc.Extends
{
    public static class CommonExtension
    {
        /// <summary>
        /// 渲染脚本
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }
    }
}
