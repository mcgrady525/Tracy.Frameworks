using System.Runtime.Serialization;

namespace Tracy.Frameworks.MongoDb.Repository
{
    [DataContract]
    public enum MongoDBs
    {
        /// <summary>
        /// TicketDB
        /// </summary>
        [EnumMember]
        MongoTicketDB,

        /// <summary>
        /// HotelDB
        /// </summary>
        [EnumMember]
        MongoHotelDB,

        /// <summary>
        /// MonitorDB
        /// </summary>
        [EnumMember]
        MongoMonitorDB,
    }
}
