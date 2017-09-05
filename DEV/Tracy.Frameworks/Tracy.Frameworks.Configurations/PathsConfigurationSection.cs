using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.Configurations
{
    /// <summary>
    /// Paths配置节
    /// </summary>
    public sealed class PathsConfigurationSection : ConfigurationSection
    {
        #region 配置節設置，設定檔中有不能識別的元素、屬性時，使其不報錯
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
        /// LogServiceSite
        /// </summary>
        [ConfigurationProperty("LogServiceSite")]
        public LogServiceSite LogServiceSite
        {
            get
            {
                return (LogServiceSite)this["LogServiceSite"];
            }
            set
            {
                this["LogServiceSite"] = value;
            }
        }

        /// <summary>
        /// LogOpenApiSite
        /// </summary>
        [ConfigurationProperty("LogOpenApiSite")]
        public LogOpenApiSite LogOpenApiSite
        {
            get
            {
                return (LogOpenApiSite)this["LogOpenApiSite"];
            }
            set
            {
                this["LogOpenApiSite"] = value;
            }
        }

        /// <summary>
        /// LogSite
        /// </summary>
        [ConfigurationProperty("LogSite")]
        public LogSite LogSite
        {
            get
            {
                return (LogSite)this["LogSite"];
            }
            set
            {
                this["LogSite"] = value;
            }
        }

    }

    public abstract class PagePathBase : ConfigurationElement
    {

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
        ///
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return this["path"].ToString();
            }
            set
            {
                this["path"] = value;
            }
        }
    }

    public sealed class LogServiceSite : PagePathBase { }
    public sealed class LogOpenApiSite : PagePathBase { }
    public sealed class LogSite : PagePathBase { }
}
