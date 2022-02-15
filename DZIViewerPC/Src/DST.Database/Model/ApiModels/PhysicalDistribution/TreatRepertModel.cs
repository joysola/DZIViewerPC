using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 送检返回的清单列表
    /// </summary>
    public class TreatRepertModel
    {
        public string hospitalName { get; set; }
        public string patientName { get; set; }
        public string age { get; set; }
        public string sex { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? gatherTime { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? receiverTime { get; set; }
        public string laboratoryCode { get; set; }
        public string productName { get; set; }
        public string remark { get; set; }
    }
}
