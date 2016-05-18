using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Tracy.Frameworks.RedisConfig
{
    public class HostGroupCollection : ConfigurationElementCollection
    {
        public HostGroupCollection()
        {
          
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
            return new HostGroup();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((HostGroup)element).Name;
        }

        public HostGroup this[int index]
        {
            get
            {
                return (HostGroup)BaseGet(index);
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

        new public HostGroup this[string name]
        {
            get
            {
                return (HostGroup)BaseGet(name);
            }
        }

        public int IndexOf(HostGroup details)
        {
            return BaseIndexOf(details);
        }

        public void Add(HostGroup details)
        {
            BaseAdd(details);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(HostGroup details)
        {
            if (BaseIndexOf(details) >= 0)
                BaseRemove(details.Name);
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
            get { return "HostGroup"; }
        }
    }
}
