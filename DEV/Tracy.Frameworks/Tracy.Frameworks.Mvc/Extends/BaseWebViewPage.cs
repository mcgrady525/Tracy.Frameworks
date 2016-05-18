using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Diagnostics.CodeAnalysis;

/*********************************************************
 * 开发人员：鲁宁
 * 创建时间：2015/1/29 14:18:18
 * 描述说明：
 * 
 * 更改历史：
 * 
 * *******************************************************/
namespace Tracy.Frameworks.Mvc.Extends
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseWebViewPage : System.Web.Mvc.WebViewPage
    {
        ///// <summary>
        ///// 
        ///// </summary>
        //public EmployeeViewModel Employee
        //{
        //    get
        //    {
        //        OfflineBaseController controller = (OfflineBaseController)this.ViewContext.Controller;
        //        return controller.Employee;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public string QuickLinks
        //{
        //    get
        //    {
        //        //OfflineBaseController controller = (OfflineBaseController)this.ViewContext.Controller;
        //        //if (!string.IsNullOrEmpty(controller.QuickLinks))
        //        //    return controller.QuickLinks;
        //        //else
        //        //    return "{}";
        //        return "{}";
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public string RightsManagementPath
        //{
        //    get
        //    {
        //        var sites = ConfigurationHelper.GetSection<Sites>().All.Cast<Site>();
        //        var siteUrlMaps = sites.ToDictionary(s => s.Resource.ToLower(), s => s.Url);
        //        string key = "_rightsmanagement_";
        //        if (siteUrlMaps.ContainsKey(key))
        //        {
        //            return siteUrlMaps[key];
        //        }
        //        return "";
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //public bool IsLogin
        //{
        //    get
        //    {
        //        OfflineBaseController controller = (OfflineBaseController)this.ViewContext.Controller;
        //        return controller.IsLogin;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public string LogoutLink
        //{
        //    get
        //    {
        //        return ConfigurationHelper.GetSection<Paths>().UserQuit.Path;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public string UserInfoLink
        //{
        //    get
        //    {
        //        return ConfigurationHelper.GetSection<Paths>().UserInfo.Path;
        //    }
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        //public string QuickMenu
        //{
        //    get
        //    {
        //        return ConfigurationHelper.GetSection<Paths>().QuickMenu.Path;
        //    }

        //}

    }



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class BaseWebViewPage<TModel> : BaseWebViewPage
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseWebViewPage()
            : base()
        { }
        private ViewDataDictionary<TModel> viewData;
        /// <summary>
        /// 
        /// </summary>
        public new AjaxHelper<TModel> Ajax
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public new HtmlHelper<TModel> Html
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public new TModel Model
        {
            get
            {
                return ViewData.Model;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "This is the mechanism by which the ViewPage gets its ViewDataDictionary object.")]
        public new ViewDataDictionary<TModel> ViewData
        {
            get
            {
                if (viewData == null)
                {
                    SetViewData(new ViewDataDictionary<TModel>());
                }
                return viewData;
            }
            set
            {
                SetViewData(value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public override void InitHelpers()
        {
            base.InitHelpers();

            Ajax = new AjaxHelper<TModel>(ViewContext, this);
            Html = new HtmlHelper<TModel>(ViewContext, this);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vData"></param>
        protected override void SetViewData(ViewDataDictionary vData)
        {
            if (vData == null)
                throw new ArgumentNullException("vData");
            this.viewData = new ViewDataDictionary<TModel>(vData);

            base.SetViewData(this.viewData);
        }
    }
}
