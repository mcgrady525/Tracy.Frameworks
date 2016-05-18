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
    /// 不要进行简繁体转换的标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NoTranslateAttribute : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(new ParameInspector());
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
    }

    public class ParameInspector : IParameterInspector
    {
        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            var header = MessageHeader.CreateHeader("NoTranslate", "NoTranslate", true);
            OperationContext.Current.OutgoingMessageHeaders.Add(header);
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            return null;
        }
    }
}
