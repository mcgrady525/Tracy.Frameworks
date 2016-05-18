using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tracy.Frameworks.MongoDb.Tests.Entity
{
    /// <summary>
    /// MongoDb实体，封装业务实体
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public class BookingSearchRQMongo: BaseMongoEntity<ObjectId>//一定要指定主键，最好默认ObjectId
    {
        [DataMember]
        public BookingSearchRQ BookingSearchRQ { get; set; }
    }
}
