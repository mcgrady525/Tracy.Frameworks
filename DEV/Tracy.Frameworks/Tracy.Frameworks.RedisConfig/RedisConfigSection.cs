using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.RedisConfig
{
    public class RedisConfigSection : ConfigurationSection
    {
       [ConfigurationProperty("", IsDefaultCollection = true)]
        public HostGroupCollection HostGroups
        {
            get
            {
                return this[""] as HostGroupCollection;
            }
        }
         
    }
}
