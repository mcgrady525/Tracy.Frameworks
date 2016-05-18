using System;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Tracy.Frameworks.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateFaultBehaviorAttribute : Attribute , IServiceBehavior , IEndpointBehavior {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription , System.ServiceModel.ServiceHostBase serviceHostBase , System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints , System.ServiceModel.Channels.BindingParameterCollection bindingParameters) {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription , System.ServiceModel.ServiceHostBase serviceHostBase) {
            IErrorHandler handler = new ValidateFaultHandler ( );
            foreach ( ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers ) {
                //dispatcher.ChannelInitializers.Add(new ServiceChannelInitializer());
                dispatcher.ErrorHandlers.Add ( handler );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription , System.ServiceModel.ServiceHostBase serviceHostBase) {
            foreach ( ServiceEndpoint se in serviceDescription.Endpoints ) {
                if ( se.Contract.Name.Equals ( "IMetadataExchange" ) && se.Contract.Namespace.Equals ( "http://schemas.microsoft.com/2006/04/mex" ) )
                    continue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint , System.ServiceModel.Channels.BindingParameterCollection bindingParameters) {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint , System.ServiceModel.Dispatcher.ClientRuntime clientRuntime) {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint , System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher) {
            IErrorHandler handler = new ValidateFaultHandler ( );
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add ( handler );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint) {
            if ( !endpoint.Binding.Name.Equals ( "webHttpBinding" ) )
                return;
        }
    }
}
