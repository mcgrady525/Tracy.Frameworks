using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;

namespace Tracy.Frameworks.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateFaultHandler : IErrorHandler {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error) {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
        public void ProvideFault(Exception error , System.ServiceModel.Channels.MessageVersion version , ref System.ServiceModel.Channels.Message fault) {
            if ( error is FaultException<ValidationFault> || error is ArgumentValidationException ) {
                var action = OperationContext.Current.IncomingMessageHeaders.Action;
                var operations = OperationContext.Current.EndpointDispatcher.DispatchRuntime.Operations;
                var operation = operations.Where ( d => d.Action == action ).FirstOrDefault ( );
                if ( operation != null ) {
                    var method = TypeDescriptor.GetProperties ( operation.Invoker )["Method"].GetValue ( operation.Invoker );
                    var returnType = ( Type )TypeDescriptor.GetProperties ( method )["ReturnType"].GetValue ( method );
                    IList<ValidationDetail> details = null;
                    if ( error is FaultException<ValidationFault> ) {
                        details = ( error as FaultException<ValidationFault> ).Detail.Details;
                    } else {
                        var tmp = ( error as ArgumentValidationException ).ValidationResults;
                        details = tmp.Select ( r => new ValidationDetail ( r.Message , r.Key , r.Tag ) ).ToList ( );
                    }


                    // 先判断是否有静态方法 Create 
                    var create = returnType.GetMethod ( "Create" , BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy , null , new Type[] { typeof ( IList<ValidationDetail> ) } , null );
                    if ( create != null ) {

                        if ( create.IsGenericMethod )
                            create = create.MakeGenericMethod ( returnType );
                        var value = create.Invoke ( null , new object[] { details } );

                        fault = operation.Formatter.SerializeReply ( version , new object[] { } , value );
                    } else if ( returnType.GetConstructor ( new Type[] { typeof ( IList<ValidationDetail> ) } ) != null ) {
                        var value = Activator.CreateInstance ( returnType , details );
                        fault = operation.Formatter.SerializeReply ( version , new object[] { } , value );
                    }
                }
            }
        }


    }
}