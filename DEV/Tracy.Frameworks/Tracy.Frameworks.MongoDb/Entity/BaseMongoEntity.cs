using System;
using System.Runtime.Serialization;

namespace Tracy.Frameworks.MongoDb
{
    [DataContract(IsReference = true)]
    [Serializable]
    public abstract class BaseMongoEntity<TKey> : IEntity<TKey>
    {
        [DataMember]
        public TKey Id { get; set; }
    }
}