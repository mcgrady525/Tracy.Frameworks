using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.RedisConfig
{
    public class HostGroup : ConfigurationElement
    {
        [ConfigurationProperty("Hosts")]
        [ConfigurationCollection(typeof(HostConfig), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public HostCollection Hosts
        {
            get
            {
                return this["Hosts"] as HostCollection;
            }

            set
            {
                this["Hosts"] = value;
            }
        }
        [ConfigurationProperty("weight")]
        public int Weight
        {
            get
            {
                return (int)this["weight"];
            }
            set
            {
                this["weight"] = value;
            }
        }
        [ConfigurationProperty("name", IsKey = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
            set
            {
                this["name"] = value;
            }
        }
        [ConfigurationProperty("writepoolsize", DefaultValue = 100)]
        public int WritePoolSize
        {
            get
            {
                return (int)this["writepoolsize"];
            }
            set
            {
                this["writepoolsize"] = value;
            }
        }
        [ConfigurationProperty("readpoolsize",DefaultValue=100)]
        public int ReadPoolSize
        {
            get
            {
                return (int)this["readpoolsize"];
            }
            set
            {
                this["readpoolsize"] = value;
            }
        }

        [ConfigurationProperty("isSentinel")]
        public bool IsSentinel
        {
            get
            {
                if (this["isSentinel"] != null)
                {
                    return bool.Parse(this["isSentinel"].ToString());
                }
                else
                {
                    return false;
                }
            }
            set
            {
                this["isSentinel"] = value;
            }
        }

        [ConfigurationProperty("masterName")]
        public string MasterName
        {
            get
            {
                return this["masterName"] as string;
            }
            set
            {
                this["masterName"] = value;
            }
        }


        #region Utilities

        #endregion
    }
}
