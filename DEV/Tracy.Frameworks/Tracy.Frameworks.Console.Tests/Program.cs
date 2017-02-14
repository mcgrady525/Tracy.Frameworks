using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Console.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("111");

            //消费debug log
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                ConsumerDebugLogMessage();
            });

            //消费error log
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                ConsumerErrorLogMessage();
            });

            System.Console.WriteLine("是否为后台线程："+ System.Threading.Thread.CurrentThread.IsBackground.ToString());
            System.Console.WriteLine("222");
            System.Console.ReadKey();
        }

        private static void ConsumerDebugLogMessage()
        {
            System.Console.WriteLine("aaa");

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                System.Console.WriteLine("sss");
            }
            
            System.Console.WriteLine("bbb");
        }

        private static void ConsumerErrorLogMessage()
        {
            System.Console.WriteLine("ccc");

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                System.Console.WriteLine("ttt");
            }

            System.Console.WriteLine("ddd");
        }

    }
}
