using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;

namespace Tracy.Frameworks.Service
{
    /// <summary>
    /// WCF客户端消息内窥处理
    /// </summary>
    internal sealed class FaultsMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// 接收到应答后处理
        /// </summary>
        /// <param name="reply">WCF应答消息</param>
        /// <param name="correlationState">相关状态</param>
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            //当客户端获取的应答是服务端异常时,尝试反序列化异常并rethrow
            //以免客户端无法解析异常应答而再次引发客户端异常而丢失服务端异常信息
            if (reply.IsFault) 
            {
                //创建应答消息副本
                MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
                Message copy = buffer.CreateMessage();
                reply = buffer.CreateMessage(); 

                object faultDetail = ReadFaultDetail(copy);
                Exception exception = faultDetail as Exception;
                if (exception != null)
                    throw exception;
            }
        }

        /// <summary>
        /// 发送请求前处理
        /// </summary>
        /// <param name="request">WCF请求消息</param>
        /// <param name="channel">WCF信道</param>
        /// <returns></returns>
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            return null;
        }

        private static object ReadFaultDetail(Message reply)
        {
            const string detailElementName = "Detail";

            using (var reader = reply.GetReaderAtBodyContents())
            {
                // 查找<detail>
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == detailElementName)
                        break;
                }

                if (reader.NodeType != XmlNodeType.Element || reader.LocalName != detailElementName || (!reader.Read())) return null;

                var serializer = new NetDataContractSerializer();
                try
                {
                    return serializer.ReadObject(reader);
                }
                catch (FileNotFoundException)
                {
                    //当序列化器载入程序集出错时
                    return null;
                }
            }
        }
    }
}
