using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tracy.Frameworks.MongoDb.Tests.Entity
{
    /// <summary>
    /// 业务实体，预订查询请求
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public class BookingSearchRQ
    {
        [DataMember]
        public bool? IsReturnPagedData { get; set; }

        [DataMember]
        public string OutBoundFromAirport { get; set; }

        [DataMember]
        public string OutBoundToAirport { get; set; }

        [DataMember]
        public string InBoundFromAirport { get; set; }

        [DataMember]
        public string InBoundToAirport { get; set; }
        
        [DataMember]
        public JourneyType JourneyType { get; set; }

        /// <summary>
        /// 出發日期 沒有時間部份
        /// </summary>
        [DataMember]
        public DateTime OutBoundDate { get; set; }

        /// <summary>
        /// 出發時間，格式："00:00-12:12",如果有多個，則"00:00-05:59|06:00-07:59"
        /// </summary>
        [DataMember]
        public string OutBoundTimeRange { get; set; }

        /// <summary>
        /// 回程日期
        /// 如果是單程，這里就不用錄入
        /// </summary>
        [DataMember]
        public DateTime? InBoundDate { get; set; }
    }

    /// <summary>
    /// OneWay RoundTrip OpenJaw
    /// </summary>
    [DataContract]
    public enum JourneyType : byte
    {
        /// <summary>
        /// 單程
        /// </summary>
        [Display(Name = "OneWay")]
        [EnumMember]
        OneWay = 1,

        /// <summary>
        /// 來回程(回程的出發地城市(或機場)為來程的目的地城市(或機場))
        /// </summary>
        [Display(Name = "RoundTrip")]
        [EnumMember]
        RoundTrip = 2,

        /// <summary>
        /// 來回程(回程的出發地城市不是來程的目的地城市),
        /// 查找OpenJour時用戶必須填多個出發地城市和目的地城市
        /// 查找時要求來回程必須在一個Routing中
        /// </summary>
        [Display(Name = "OpenJaw")]
        [EnumMember]
        OpenJaw = 3,

        //[Display(Name = "MultipleLegs")]
        //[EnumMember]
        //MultipleLegs = 4
    }
}
