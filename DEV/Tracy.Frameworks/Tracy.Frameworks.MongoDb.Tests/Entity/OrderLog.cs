using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.MongoDb.Tests.Entity
{
    [DataContract(IsReference = true)]
    [Serializable]
    public class OrderLog: BaseMongoEntity<string>
    {
        [DataMember]
        public int OrderId { get; set; }

        [DataMember]
        public decimal OrderAmount { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdatedDate { get; set; }

        [DataMember]
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime OrderDate { get; set; }
    }
}
