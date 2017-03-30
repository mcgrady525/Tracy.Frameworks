using System;

namespace Tracy.Frameworks.RabbitMQ
{
    /// <summary>
    /// rabbitMQ服务器连接配置
    /// </summary>
    public class RabbitMQConfig
    {
        /// <summary>
        /// localhost或者192.168.1.102或者my.rabbitmq.com
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 可以理解为命名空间，比如：Log.VHost.DEV，默认为'/'
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 心跳时间，单位：秒
        /// </summary>
        public ushort HeartBeat { get; set; }

        /// <summary>
        /// 自动重连
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; }

        /// <summary>
        /// 重连时间
        /// </summary>
        public TimeSpan NetworkRecoveryInterval { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
