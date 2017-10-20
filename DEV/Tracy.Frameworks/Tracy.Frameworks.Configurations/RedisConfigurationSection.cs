using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Configurations
{
    /// <summary>
    /// Redis的配置节
    /// </summary>
    public sealed class RedisConfigurationSection: ConfigurationSection
    {
        #region 配置节设置，设定文档中有不能识别的元素，属性时，不报错
        /// <summary>
        /// 遇到未知屬性時，不報錯
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            //return base.OnDeserializeUnrecognizedAttribute(name, value);
            return true;
        }

        /// <summary>
        /// 遇到未知元素時，不報錯
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
        {
            //return base.OnDeserializeUnrecognizedElement(elementName, reader);
            return true;
        }

        #endregion

        /// <summary>
        /// HostName
        /// </summary>
        [ConfigurationProperty("hostName", IsRequired = true)]
        public string HostName
        {
            get
            {
                return this["hostName"].ToString();
            }
            set
            {
                this["hostName"] = value;
            }
        }
    }
}
