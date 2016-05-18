using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Tracy.Frameworks.Log
{
    /// <summary>   
    /// 消息拦截器【实现客户端和服务端的消息拦截】   
    /// </summary>   
    internal class WcfServiceInterpector : IClientMessageInspector, IDispatchMessageInspector
    {

        public WcfServiceInterpector(string systemCode, string source)
        {
            SystemCode = systemCode;
            Source = source;
        }

        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 在收到回复消息之后将它传递回客户端应用程序之前，启用消息的检查或修改。
        /// </summary>
        /// <param name="reply">要转换为类型并交回给客户端应用程序的消息</param>
        /// <param name="correlationState">关联状态数据</param>
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }

        /// <summary>
        /// 在将请求消息发送到服务之前，启用消息的检查或修改
        /// </summary>
        /// <param name="request">要发送给服务的消息</param>
        /// <param name="channel">客户端对象通道</param>
        /// <returns>作为 System.ServiceModel.Dispatcher.IClientMessageInspector.AfterReceiveReply(System.ServiceModel.Channels.Message@,System.Object)方法的 correlationState 参数返回的对象。如果不使用相关状态，则为 null。最佳做法是将它设置为 System.Guid，以确保没有两个相同的correlationState 对象。</returns>
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }

        /// <summary>
        /// 在已接收入站消息后将消息调度到应发送到的操作之前调用
        /// </summary>
        /// <param name="request">请求消息</param>
        /// <param name="channel">传入通道</param>
        /// <param name="instanceContext"></param>
        /// <returns>用于关联状态的对象。该对象将在 System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)</returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 在操作已返回后发送回复消息之前调用。
        /// </summary>
        /// <param name="reply">回复消息。如果操作是单向的，则此值为 null。</param>
        /// <param name="correlationState">从 System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            PerfLogInfo info = new PerfLogInfo();

            info.SystemCode = SystemCode;
            info.Source = Source;
            var startime = (DateTime)correlationState;
            var duration = (DateTime.Now - startime).TotalMilliseconds;

            info.ClassName = OperationContext.Current == null ? "" : OperationContext.Current.EndpointDispatcher.ContractName;
            info.MethodName = OperationContext.Current == null ? "" : ParseOperationName(OperationContext.Current.IncomingMessageHeaders.Action);

            info.Duration = Convert.ToInt64(duration);


            info.Remark = LogHelper.GetLogContextStr();
            LogHelper.AttachLogToContext("W." + info.MethodName, info.Duration);
            var logcontextStr = LogHelper.GetLogContext().ToJson();
            MessageHeader header = MessageHeader.CreateHeader(LogHelper.LogSection, logcontextStr, logcontextStr);
            try
            {
                //不存在jsonp时，才加。
                if (
                    (OperationContext.Current != null && OperationContext.Current.EndpointDispatcher.EndpointAddress.ToString().IndexOf("jsonp", StringComparison.OrdinalIgnoreCase) == -1)
                    ||
                    (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request.Path.IndexOf("jsonp", StringComparison.OrdinalIgnoreCase) == -1)
                    )
                {
                    reply.Headers.Add(header);
                }

            }
            catch
            {
            }
            var path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            if (path.IndexOf("Scheduler", StringComparison.CurrentCultureIgnoreCase) > 0)
            {
                LogHelper.Send(info);
            }
            else
            {
                PerfLogManager.Instance.Enqueue(info);
            }
        }



        /// <summary>
        ///  截取字符
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private string ParseOperationName(string action)
        {
            if (string.IsNullOrEmpty(action))
            {
                return action;
            }
            var index = action.LastIndexOf('/');
            if (index >= 0)
            {
                action = action.Substring(index + 1);
            }
            return action;
        }
    }
}
