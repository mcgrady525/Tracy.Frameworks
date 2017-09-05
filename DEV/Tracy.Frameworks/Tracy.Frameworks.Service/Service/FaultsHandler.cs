using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Data.Entity.Validation;
using System.IO;
using System.Configuration;
using Tracy.Frameworks.Common.Exceptions;
using Tracy.Frameworks.Common.Exceptions.ValidationException;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Service
{
    /// <summary>
    /// 自定义WCF异常处理,此方法一定不能拋出異常，否則IIS Application Pool會Crash！！！！！5次后將會處于停止狀態。
    /// </summary>
    internal sealed class FaultsHandler : IErrorHandler
    {
        /// <summary>
        /// 捕获所有异常
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {
            try
            {
                if (error is HttpOptionsOkException)
                    return true;
                string Source = "", SystemCode = "";
                var projectAddressArray = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.Split('\\');
                if (projectAddressArray.Length >= 3)
                {
                    Source = projectAddressArray[projectAddressArray.Length - 2];
                    SystemCode = projectAddressArray[projectAddressArray.Length - 3];
                }
                if (SystemCode.IndexOf("Services", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    SystemCode = Source;
                    Source = "Offline.Service";
                }
                if (Source.IndexOf("Offline.Service.Site", StringComparison.CurrentCultureIgnoreCase) >= 0 || Source == "Services")
                    Source = "Offline.Service";
                var detailStr = string.Empty;
                if (error is DbEntityValidationException)
                {
                    FaultException ex = GetFaultException(error, "");
                    detailStr = ex.Reason.ToString();
                }
                else
                {
                    detailStr = error.ToString();
                }
                //ErrorLogManager.Instance.Enqueue(new ErrorLogInfo() { Message = error.Message, Detail = detailStr, SystemCode = SystemCode, Source = Source });
            }
            catch { return true; }
            return true; //True时表示异常不中止WCF会话
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="error"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            //System.ServiceModel.CommunicationException不会执行到这个方法
            if (OperationContext.Current == null)
            {
                return;
            }
            var action = OperationContext.Current.IncomingMessageHeaders.Action;

            FaultException ex;
            //提升非FaultException为FaultException, 或將泛型FaultException處理為非泛型FaultException
            if (error is DbEntityValidationException) //非FaultException EF校驗異常DbEntityValidationException
            {
                ex = GetFaultException(error, action);
            }
            else if (error is FaultException) //FaultException
            {
                return;
            }
            else //非特殊類型的非FaultException異常
            {
                ex = new FaultException(string.Format("{0}\r\n{1}", error.Message, error), null, action) { Source = error.Source };
            }

            //var ex = new WebFaultException<string>(error.ToString(), System.Net.HttpStatusCode.InternalServerError);
            var messageFault = ex.CreateMessageFault();
            //MessageFault messageFault = MessageFault.CreateFault(new FaultCode("Sender"), new FaultReason(error.ToString()),error, new NetDataContractSerializer());
            fault = Message.CreateMessage(version, messageFault, null); //第三個參數action必須是null
        }

        private static FaultException GetFaultException(Exception error, string action)
        {
            FaultException ex;
            var dbEntityValidationException = (error as DbEntityValidationException);
            if (null != dbEntityValidationException.EntityValidationErrors && dbEntityValidationException.EntityValidationErrors.Any())
            {
                var first = dbEntityValidationException.EntityValidationErrors.FirstOrDefault();
                var detail = (from err in (error as DbEntityValidationException).EntityValidationErrors //List<ValidationResult>
                              select
                              new ValidationResult(
                              (from validationError in err.ValidationErrors
                               select
                               new ValidationError(validationError.PropertyName, validationError.ErrorMessage)
                              ).ToList(),
                              new ValidationEntry(err.Entry.Entity.ToString(), (ValidationEntityState)Enum.Parse(typeof(ValidationEntityState), ((int)err.Entry.State).ToString(CultureInfo.InvariantCulture))),
                              err.IsValid)).ToList();
                ex = new ValidationException( //將ValidationException拋出，前端需要判斷ex.CreateMessageFault().HasDetail和通過ex.CreateMessageFault().GetDetail<List<ValidationResult>>()獲取Detail
                    detail,
                    string.Format("{0}\r\n{1}\r\n{2}", dbEntityValidationException.Message, detail.ToJson(), dbEntityValidationException),
                    action)
                {
                    Source = (first != null ? first.Entry.Entity.GetType().FullName : string.Empty)
                };
            }
            else
            {
                ex = new FaultException(string.Format("{0}\r\n{1}", error.Message, error), null, action) { Source = error.Source };
            }
            return ex;
        }
    }
}
