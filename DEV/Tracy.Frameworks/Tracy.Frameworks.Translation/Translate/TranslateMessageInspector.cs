using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace Tracy.Frameworks.Translation.Translate
{
    public class TranslateMessageInspector : IDispatchMessageInspector
    {
        private TranslationType TranslationType;

        public TranslateMessageInspector(TranslationType translationType)
        {
            TranslationType = translationType;
        }

        public object AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            return null;
        }

        public void BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            try
            {
                if (TranslationType != Translate.TranslationType.None)
                {
                    reply = TranslateMessage(reply, TranslationType);
                }
            }
            catch
            {
            }
        }

        private Message TranslateMessage(Message message, TranslationType translationType)
        {
            var flag = message.Headers.Any(m => m.Name == "NoTranslate" && m.Namespace == "NoTranslate");
            if (flag)
            {
                message.Headers.RemoveAll("NoTranslate", "NoTranslate");
            }
            var ms = new MemoryStream();
            var xw = XmlWriter.Create(ms);
            message.WriteMessage(xw);
            xw.Flush();
            var body = Encoding.UTF8.GetString(ms.ToArray());
            xw.Close();
            if (!flag)
            {
                body = translationType == Translate.TranslationType.SimplifiedToTraditional ? CommTranslate.BasicToTraditional(body) : CommTranslate.BasicToSimplified(body);
            }

            ms = new MemoryStream(Encoding.UTF8.GetBytes(body));
            var xdr = XmlDictionaryReader.CreateTextReader(ms, new XmlDictionaryReaderQuotas());
            var newMessage = Message.CreateMessage(xdr, int.MaxValue, message.Version);

            newMessage.Properties.CopyProperties(message.Properties);
            return newMessage;
        }
    }
}
