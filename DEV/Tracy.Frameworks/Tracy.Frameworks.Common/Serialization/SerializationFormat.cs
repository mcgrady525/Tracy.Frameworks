using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.Common.Serialization
{
    /// <summary>
    /// 指定Rest服务消息格式的枚举
    /// </summary>
    [DataContract]
    public enum SerializationFormat : short
    {
        /// <summary>
        /// XML 格式
        /// </summary>
        [EnumMember]
        Xml = 0,

        /// <summary>
        /// JavaScript 对象表示法 (JSON) 格式
        /// </summary>
        [EnumMember]
        Json = 1
    }
}
