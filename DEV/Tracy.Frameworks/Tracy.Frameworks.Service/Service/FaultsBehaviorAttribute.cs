using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Tracy.Frameworks.Service
{
    /// <summary>
    /// WCF异常处理行为特性
    /// </summary>
    public class FaultsBehaviorAttribute : Attribute, IServiceBehavior, IEndpointBehavior, IContractBehavior
    {
        //改变客户端行为
        private void ApplyClientBehavior(ClientRuntime runtime)
        {
            foreach (IClientMessageInspector inspector in runtime.MessageInspectors)
                if (inspector is FaultsMessageInspector)
                    return;
            runtime.MessageInspectors.Add(new FaultsMessageInspector());
        }

        //改变服务端行为
        private void ApplyDispatchBehavior(ChannelDispatcher dispatcher)
        {
            foreach (IErrorHandler errorHandler in dispatcher.ErrorHandlers)
                if (errorHandler is FaultsHandler)
                    return;
            dispatcher.ErrorHandlers.Add(new FaultsHandler());
        }

        #region IContractBehavior Implemented

        /// <summary>
        /// 处理WCF绑定参数
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            return;
        }

        /// <summary>
        /// 应用客户端行为
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            if (!contractDescription.Name.Equals("IMetadataExchange"))
            {
                this.ApplyClientBehavior(clientRuntime);
            }
        }

        /// <summary>
        /// 应用服务端行为
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        /// <param name="dispatchRuntime"></param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            if (!contractDescription.Name.Equals("IMetadataExchange"))
            {
                this.ApplyDispatchBehavior(dispatchRuntime.ChannelDispatcher);
            }
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="contractDescription"></param>
        /// <param name="endpoint"></param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }

        #endregion

        #region IEndpointBehavior Implemented

        /// <summary>
        /// 处理WCF绑定参数
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            return;
        }

        /// <summary>
        /// 应用客户端行为
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointclientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime endpointclientRuntime)
        {
            if (!endpoint.Binding.Name.StartsWith("mex"))
                this.ApplyClientBehavior(endpointclientRuntime);
        }

        /// <summary>
        /// 应用服务端行为
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            if(!endpoint.Binding.Name.StartsWith("mex"))
                this.ApplyDispatchBehavior(endpointDispatcher.ChannelDispatcher);
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        #endregion

        #region IServiceBehavior Implemented

        /// <summary>
        /// 处理WCF绑定参数
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            return;
        }

        /// <summary>
        /// 应用Service端行为
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                if (!dispatcher.BindingName.StartsWith("mex"))
                {
                    //dispatcher.ChannelInitializers.Add(new ServiceChannelInitializer());
                    this.ApplyDispatchBehavior(dispatcher);
                }
            }
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        #endregion
    }
}
