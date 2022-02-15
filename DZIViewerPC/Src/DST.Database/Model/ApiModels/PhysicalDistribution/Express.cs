using DST.Common.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 物流实体
    /// </summary>
    public class Express
    {
        public string id { get; set; }

        public string createUser { get; set; }

        public string createDept { get; set; }

        //public Nullable<DateTime> createTime { get; set; }

        public string updateUser { get; set; }

        public Nullable<DateTime> updateTime { get; set; }

        public Nullable<int> status { get; set; }

        public Nullable<int> isDeleted { get; set; }

        public string mailNo { get; set; }

        public string hospitalId { get; set; }

        public string hospitalName { get; set; }

        public Nullable<int> amount { get; set; }

        public Nullable<int> actualAmount { get; set; }

        public string salesUserId { get; set; }

        public string salesUserName { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> deliveryDate { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> receiveDate { get; set; }

        public string expressMessage { get; set; }

        public string remark { get; set; }

        public Nullable<int> sort { get; set; }
        public Nullable<int> anomalyStatus { get; set; }

        public Nullable<int> sampleStatus { get; set; }

        public Nullable<int> expressStatus { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> arriveTime { get; set; }
    }

}
