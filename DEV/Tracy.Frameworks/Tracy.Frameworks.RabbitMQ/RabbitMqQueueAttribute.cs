using System;

namespace Tracy.Frameworks.RabbitMQ
{
    /// <summary>
    /// 自定义的RabbitMq队列信息实体特性
    /// </summary>
    public class RabbitMQQueueAttribute : Attribute
    {
        public RabbitMQQueueAttribute(string queueName)
        {
            QueueName = queueName ?? string.Empty;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool IsProperties { get; set; }
    }
}
