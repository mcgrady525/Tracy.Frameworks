using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tracy.Frameworks.UnitTest.Helper
{
    public class BinarySerializerHelper
    {
        /// <summary>
        /// 将对象序列化成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Serialize<T>(T input)
        {
            using (var stream= new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, input);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

    }
}
