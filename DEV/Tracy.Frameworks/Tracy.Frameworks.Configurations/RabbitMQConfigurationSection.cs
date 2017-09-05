using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Tracy.Frameworks.Common.Extends;

namespace Tracy.Frameworks.Configurations
{
    /// <summary>
    /// RabbitMQ的配置节
    /// </summary>
    public sealed class RabbitMQConfigurationSection : ConfigurationSection
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

        /// <summary>
        /// Port，默认为5672
        /// </summary>
        [ConfigurationProperty("port", DefaultValue = 5672)]
        public int Port
        {
            get
            {
                return this["port"].ToString().ToInt();
            }
            set
            {
                this["port"] = value;
            }
        }

        /// <summary>
        /// UserName
        /// </summary>
        [ConfigurationProperty("userName", IsRequired = true)]
        public string UserName
        {
            get
            {
                return this["userName"].ToString();
            }
            set
            {
                this["userName"] = value;
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get
            {
                return this["password"].ToString();
            }
            set
            {
                this["password"] = value;
            }
        }

        /// <summary>
        /// VHost，默认'/'
        /// </summary>
        [ConfigurationProperty("vhost", DefaultValue= "/")]
        public string VHost
        {
            get
            {
                return this["vhost"].ToString();
            }
            set
            {
                this["vhost"] = value;
            }
        }

    }
}
