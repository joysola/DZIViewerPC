using DST.Common.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class PhysDistInfoModel
    {
        public string id { get; set; }
        public string createUser { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? createTime { get; set; }
        public string updateUser { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? updateTime { get; set; }
        public int? status { get; set; }
        public int? isDeleted { get; set; }
        public string mailNo { get; set; }
        public string hospitalId { get; set; }
        public string hospitalName { get; set; }
        public int? amount { get; set; }
        public int? actualAmount { get; set; }
        public string salesUserId { get; set; }
        public string salesUserName { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? deliveryDate { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? receiveDate { get; set; }
        public string expressMessage { get; set; }
        public string remark { get; set; }
        public int? sort { get; set; }
        public int? anomalyStatus { get; set; }
        public int? sampleStatus { get; set; }
        public int? expressStatus { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? arriveTime { get; set; }

        public List<Sample> expressSampleVOList { get; set; } = new List<Sample>();
    }
}
