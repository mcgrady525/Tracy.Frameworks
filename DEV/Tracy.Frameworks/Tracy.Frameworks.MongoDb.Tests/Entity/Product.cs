using MongoDB.Bson;
using System;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.MongoDb.Tests.Entity
{
    [DataContract(IsReference = true)]
    [Serializable]
    public class Product: BaseMongoEntity<ObjectId>
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Price { get; set; }
    }
}
