using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Tracy.Frameworks.Redis
{
    public class JsonNetSerializer : ISerializer
    {
        static JsonNetSerializer()
        {
            settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            //settings.NullValueHandling = NullValueHandling.Ignore;
            //settings.MissingMemberHandling = MissingMemberHandling.Ignore;
        }

        private static JsonSerializerSettings settings;
        public static JsonSerializerSettings Settings
        {
            get
            {
                return settings;
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Serialize<T>(T data)
        {
            var jsonStr = JsonConvert.SerializeObject(data, Formatting.None, settings);
            using (MemoryStream sb = new MemoryStream())
            {
                using (var writer = new System.IO.StreamWriter(sb, Encoding.UTF8))
                {
                    writer.Write(jsonStr);
                }
                return sb.ToArray();
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="data">object</param>
        /// <returns></returns>
        public byte[] Serialize(object data)
        {
            var jsonStr = JsonConvert.SerializeObject(data, Formatting.None, settings);
            using (MemoryStream sb = new MemoryStream())
            {
                using (var writer = new System.IO.StreamWriter(sb, Encoding.UTF8))
                {
                    writer.Write(jsonStr);
                }
                return sb.ToArray();
            }
        }

        /// <summary>
        /// 反序列化为类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T Deserialize<T>(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                {
                    var jsonStr = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(jsonStr, settings);
                }
            }
        }

        /// <summary>
        /// 反序列化为object
        /// </summary>
        /// <param name="t"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public object Deserialize(Type t, byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, Encoding.UTF8))
                {
                    var jsonStr = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject(jsonStr, t, settings);
                }
            }
        }
    }
}
