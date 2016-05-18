using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Tracy.Frameworks.Translation.Translate
{
    /// <summary>
    /// /// <summary>
    /// 簡繁體轉換特性，如某個OperationContract不需要轉換，請填寫NoTranslateAttribute特性
    /// Demo:
    /// [Translate(TranslationType = TranslationType.SimplifiedToTraditional)]
    /// public class Service1 : IService1
    /// {
    ///     [NoneTranslate]
    ///     string GetData(int value);
    /// }
    /// </summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TranslateAttribute : Attribute, IServiceBehavior
    {
        public TranslationType TranslationType { get; set; }

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher cDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (var endpointDispatcher in cDispatcher.Endpoints)
                {
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new TranslateMessageInspector(TranslationType));
                }
            }
        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}
