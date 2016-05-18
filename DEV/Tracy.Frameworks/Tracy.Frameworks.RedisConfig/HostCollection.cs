using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.RedisConfig
{
    public class HostCollection : ConfigurationElementCollection
    {

        public HostCollection()
        {
            HostConfig details = (HostConfig)CreateNewElement();
            if (details.IP != "" && details.Port != "")
            {
                Add(details);
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new HostConfig();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            HostConfig config = ((HostConfig)element);
            return string.Format("{0}:{1}:{2}", config.IP, config.Port,config.ReadOnly);
        }

        public HostConfig this[int index]
        {
            get
            {
                return (HostConfig)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public HostConfig this[string name]
        {
            get
            {
                return (HostConfig)BaseGet(name);
            }
        }

        public int IndexOf(HostConfig details)
        {
            return BaseIndexOf(details);
        }

        public void Add(HostConfig details)
        {
            BaseAdd(details);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(HostConfig details)
        {
            if (BaseIndexOf(details) >= 0)
                BaseRemove(string.Format("{0}:{1}",details.IP , details.Port));
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override string ElementName
        {
            get { return "host"; }
        }
        
    }
}
