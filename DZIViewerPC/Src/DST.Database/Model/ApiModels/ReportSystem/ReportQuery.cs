using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class ReportQuery : ObservableObject
	{
        public List<string> productIdList { get; set; } = new List<string>();

        public string pathologyCodeOrPatientName { get; set; } = "";

        public string laboratoryCode { get; set; } = "";

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? gatherTimeStart { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? gatherTimeEnd { get; set; }


        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? reportTimeStart { get; set; }

        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? reportTimeEnd { get; set; }

        public int? confirmStatus { get; set; }

        private string productId = "";

        /// <summary>
        /// 前端只是单个ID
        /// </summary>
        [JsonIgnore]
        public string ProductId
        {
            get { return this.productId; }
            set
            {
                this.productId = value == null ? "" : value;
                this.RaisePropertyChanged("ProductId");
                this.productIdList.Clear();
                if (!string.IsNullOrEmpty(this.productId))
                {
                    this.productIdList.Add(this.productId);
                }
            }
        }
    }
}
