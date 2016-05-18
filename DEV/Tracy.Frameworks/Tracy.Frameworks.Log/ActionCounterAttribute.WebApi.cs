using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Filters;

namespace Tracy.Frameworks.Log.WebApi
{
    /// <summary>
    /// Web API所有Action性能统计
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

        // 计时
        private const string StopwatchKey = "HttpContext.Current.Request";

        /// <summary>
        /// action执行前调用
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Request.Properties[StopwatchKey] = DateTime.Now;
            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// action执行后调用
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var startTime = (DateTime)actionExecutedContext.Request.Properties[StopwatchKey];
                var duration = (DateTime.Now - startTime).TotalMilliseconds;
                var className = actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller";
                var methodName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                var duration1 = Convert.ToInt64(duration);
                var remark = LogHelper.GetLogContextStr() + ",OnActionExecuted：" + Convert.ToInt64(duration);
                PerfLogInfo info = ActionCounterHelper.GetPerfLogInfo(this.SystemCode, this.Source, className, methodName, duration1, remark);

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
                    Source = this.Source,
                    SystemCode = this.SystemCode
                });
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
