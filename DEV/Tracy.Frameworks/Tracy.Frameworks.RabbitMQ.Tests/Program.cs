using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.RabbitMQ.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var rabbitMQProxy = new RabbitMQWrapper(new RabbitMQConfig 
            {
                Host= "localhost",
                VirtualHost= "/",
                HeartBeat= 60,
                AutomaticRecoveryEnabled= true,
                UserName= "admin",
                Password= "P@ssw0rd.123"
            });

            var input = Input();

            while (input != "q")
            {
                var log = new MessageModel
                {
                    CreateDateTime = DateTime.Now,
                    Msg = input
                };
                rabbitMQProxy.Publish(log);

                input = Input();
            }

            rabbitMQProxy.Dispose();
        }

        private static string Input()
        {
            Console.WriteLine("请输入信息：");
            var input = Console.ReadLine();
            return input;
        }
    }

    [RabbitMQQueue("SkyChen.QueueName", ExchangeName = "SkyChen.ExchangeName", IsProperties = true)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
