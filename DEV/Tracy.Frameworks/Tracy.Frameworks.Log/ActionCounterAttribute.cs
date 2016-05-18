using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;


namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// Mvc所有Action性能统计
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ActionCounterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }


        /// <summary>
        /// Called by the ASP.NET   MVC framework after the action method executes. (request进入controller，并且执行结束时触发,)
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var className = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";
            var methodName = filterContext.ActionDescriptor.ActionName;
            var startime = (DateTime)filterContext.HttpContext.Items["HttpContext.Current.Request"];
            var duration = (DateTime.Now - startime).TotalMilliseconds;
            var remark = LogHelper.GetLogContextStr() + ",OnActionExecuted：" + Convert.ToInt64(duration);
            PerfLogInfo info = ActionCounterHelper.GetPerfLogInfo(this.SystemCode, this.Source, className, methodName, Convert.ToInt64(duration), remark);
            filterContext.HttpContext.Items["HttpContext.Current.Request.PerfLogInfo"] = info;

            #region 注释掉的
            //object response = null;
            //if (filterContext != null && filterContext.Result != null)
            //{
            //    var result = filterContext.Result;
            //    //.net framework 提供了 7种 ActionResult ,这里需要看项目的设计，我们只需要分析各自项目会运用到哪些ActionResult，并且拿到相应的Model或者Data(如果没有返回值就不需要Response就是null)
            //    if (result is ViewResultBase)
            //    {
            //        if ((result as ViewResultBase).Model != null)
            //        {
            //            response = (result as ViewResultBase).Model;
            //        }
            //    }
            //    else if (result is JsonResult)
            //    {
            //        if ((result as JsonResult).Data != null)
            //        {
            //            response = (result as JsonResult).Data;
            //        }
            //    }
            //}

            //TraceFrontendRequest(); 
            #endregion

            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes. (request尚未进入controller触发,)
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionParameters != null)
            {
                //LoggingAttribute 只会拥有一个实例，并且运用于所有的Cotrollers,所以我们必须保证每一次Request只会记录自己的数据
                //filterContext.HttpContext.Items[HttpContext.Current.Request] = filterContext.ActionParameters;
                filterContext.HttpContext.Items["HttpContext.Current.Request"] = DateTime.Now;
            }
            base.OnActionExecuting(filterContext);

        }

        /// <summary>
        /// Called by the ASP.NET   MVC framework after the action result executes. . (action的 方法执行完毕，并且结果执行完毕(数据绑定结束) 触发 )
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            PerfLogInfo info = filterContext.HttpContext.Items["HttpContext.Current.Request.PerfLogInfo"] as PerfLogInfo;
            try
            {
                var startime = (DateTime)filterContext.HttpContext.Items["HttpContext.Current.Request"];
                var duration = (DateTime.Now - startime).TotalMilliseconds;
                info.Duration = Convert.ToInt64(duration);
                var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                if (path.IndexOf("Scheduler", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    LogHelper.Send(info);
                }
                else
                {
                    PerfLogManager.Instance.Enqueue(info);
                }
            }
            catch (Exception ex)
            {
                ErrorLogManager.Instance.Enqueue(new ErrorLogInfo
                {
                    Message = ex.Message + " 統計性能時出錯.",
                    Detail = ex.ToString(),
                    Source = info.Source,
                    SystemCode = info.SystemCode
                });
            }
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// Called by the ASP.NET   MVC framework before the action result executes. . (action的 方法执行完毕，但是结果执行之前(数据绑定结束) 触发  )
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }

    public static class ActionCounterHelper
    {
        public static PerfLogInfo GetPerfLogInfo(string systemCode, string source, string className, string methodName, long duration, string remark)
        {
            return new PerfLogInfo
            {
                SystemCode = systemCode,
                Source = source,
                ClassName = className,
                MethodName = methodName,
                Duration = duration,
                Remark = remark
            };
        }
    }
}
