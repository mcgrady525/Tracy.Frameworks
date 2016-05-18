using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tracy.Frameworks.MongoDb.Tests.Entity
{
    [DataContract(IsReference = true)]
    [Serializable]
    public class Entity1
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public DateTime CreateTime { get; set; }
    }
}
