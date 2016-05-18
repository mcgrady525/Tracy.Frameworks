using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Tracy.Frameworks.Log
{
    /// <summary>
    /// Wcf所有方法性能统计
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class WcfServiceCounterAttribute : Attribute, IServiceBehavior, IEndpointBehavior
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 系统编码
        /// </summary>
        public string SystemCode { get; set; }

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }
        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in cDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new WcfServiceInterpector(SystemCode, Source));
                }
            }
        }
        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }
        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }
        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new WcfServiceInterpector(SystemCode, Source));
        }
        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
