using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.RabbitMQ
{
    /// <summary>
    /// 封装rabbitMQ的接口
    /// </summary>
    public interface IRabbitMQWrapper
    {
        /// <summary>
        /// 创建rabbitMQ服务器连接，connection可以共用
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        IConnection CreateConnection(RabbitMQConfig config);

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="channel"></param>
        void Publish<T>(T command, IModel channel = null);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void Subscribe<T>(Action<T> handler);

        /// <summary>
        /// 获取消息(主动拉)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        void Pull<T>(Action<T> handler);

    }
}
