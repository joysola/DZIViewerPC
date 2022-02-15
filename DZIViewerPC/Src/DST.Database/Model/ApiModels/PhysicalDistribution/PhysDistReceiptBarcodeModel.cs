using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class PhysDistReceiptBarcodeModel
    {
        public string patientName { get; set; }

        public int? age { get; set; }

        public string hospitalName { get; set; }

        public string provinceName { get; set; }
        public string cityName { get; set; }
        public string areaName { get; set; }
        public string code { get; set; }
        public string laboratoryCode { get; set; }

        public string productName { get; set; }

        public string productType { get; set; }

        public string hospitalId { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty("sex")]
        public string Sex { get; set; }
    }
}
