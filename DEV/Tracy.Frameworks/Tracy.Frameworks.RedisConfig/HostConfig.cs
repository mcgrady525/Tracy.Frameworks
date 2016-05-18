using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.RedisConfig
{
    public class HostConfig : ConfigurationElement
    {
        [ConfigurationProperty("ip", IsRequired = true, IsKey = true)]
        public string IP
        {
            get
            {
                return (string)this["ip"];
            }
            set
            {
                this["ip"] = value;
            }
        }
        [ConfigurationProperty("port", IsRequired = true, IsKey = true)]
        public string Port
        {
            get
            {
                return (string)this["port"];
            }
            set
            {
                this["port"] = value;
            }
        }
       
        [ConfigurationProperty("readonly", IsRequired = true,IsKey=true)]
        public bool ReadOnly
        {
            get
            {
                return (bool)this["readonly"];
            }
            set
            {
                this["readonly"] = value;
            }
        }
    }
}
