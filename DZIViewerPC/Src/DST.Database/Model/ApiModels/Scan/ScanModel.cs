using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;

namespace DST.Database.Model
{
    public class ScanModel : ObservableObject
    {
        public string sampleId { get; set; }
        public string id { get; set; }
        public string productName { get; set; }
        public string pathologyCode { get; set; }
        public string laboratoryCode { get; set; }
        public string dept { get; set; }
        public string doctorName { get; set; }
        public string patientName { get; set; }
        public int? sex { get; set; }
        public int? age { get; set; }
        public string waxBlockNumber { get; set; }
        public string code { get; set; }
        public string sliceShortCode { get; set; }
        public string producer { get; set; }
        public string producerName { get; set; }
        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? productionTime { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? scanTime { get; set; }

        public int? receiveStatus { get; set; }

        public int? scanStatus { get; set; }

        public string adviceType { get; set; }

        public string adviceTypeName { get; set; }

        private bool isSelected = false;
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }
        [JsonIgnore]
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }

        /// <summary>
        /// 扫码时间，仅展示，不涉及到和中台交互
        /// </summary>
        [JsonIgnore]
        public DateTime? receiverTime { get; set; }
    }
}
