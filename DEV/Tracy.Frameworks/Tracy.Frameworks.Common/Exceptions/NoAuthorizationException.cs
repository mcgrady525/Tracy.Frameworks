using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Tracy.Frameworks.Common.Exceptions
{
    /// <summary>
    /// 服务端是否登录校验异常
    /// </summary>
    [DataContract, Serializable]
    public class NoAuthorizationException : FaultException
    {
        /// <summary>
        /// 后台校验异常標示
        /// </summary>
        private static readonly FaultCode ValidationFaultCode = new FaultCode(typeof(NoAuthorizationException).FullName);

        /// <summary>
        /// 
        /// </summary>
        public NoAuthorizationException()
            : base("No Authorization(xxxSZ)", ValidationFaultCode) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public NoAuthorizationException(string action)
            : base("No Authorization(xxxSZ)", ValidationFaultCode, action) { }
    }

    /// <summary>
    /// 非法客户端异常
    /// </summary>
    [DataContract, Serializable]
    public class ClientUnverifiedException : FaultException
    {
        /// <summary>
        /// 后台校验异常標示
        /// </summary>
        private static readonly FaultCode ValidationFaultCode = new FaultCode(typeof(ClientUnverifiedException).FullName);

        /// <summary>
        /// 
        /// </summary>
        public ClientUnverifiedException()
            : base("Client Unverified(xxxSZ)", ValidationFaultCode) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public ClientUnverifiedException(string action)
            : base("Client Unverified(xxxSZ)", ValidationFaultCode, action) { }
    }

    /// <summary>
    /// Options请求就绪
    /// </summary>
    [DataContract, Serializable]
    public class HttpOptionsOkException : FaultException
    {
        /// <summary>
        /// 后台校验异常標示
        /// </summary>
        private static readonly FaultCode ValidationFaultCode = new FaultCode(typeof(HttpOptionsOkException).FullName);

        /// <summary>
        /// 
        /// </summary>
        public HttpOptionsOkException()
            : base("HTTP OPTIONS OK(xxxSZ)", ValidationFaultCode) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public HttpOptionsOkException(string action)
            : base("HTTP OPTIONS OK(xxxSZ)", ValidationFaultCode, action) { }
    }
}
