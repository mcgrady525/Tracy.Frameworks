using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Tracy.Frameworks.MongoDb.Repository
{
    public class LocalTimeSerializer : IBsonSerializer, IBsonSerializer<DateTime>
    {
        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadDateTime();
            var bdt = new BsonDateTime(value);
            var result = bdt.ToLocalTime();
            return result;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var dt = (DateTime)value;
            var bdt = new BsonDateTime(dt);
            context.Writer.WriteDateTime(bdt.MillisecondsSinceEpoch);
        }

        DateTime IBsonSerializer<DateTime>.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadDateTime();
            var bdt = new BsonDateTime(value);
            var result = bdt.ToLocalTime();
            return result;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var dt = (DateTime)value;
            var bdt = new BsonDateTime(dt);
            context.Writer.WriteDateTime(bdt.MillisecondsSinceEpoch);
        }

        //c#6.0表达式为主体的属性(赋值)
        //public Type ValueType => typeof(DateTime);

        public Type ValueType
        {
            get
            { 
                return typeof(DateTime);
            }
        }
    }
}