using DST.Common.Converter;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 送检类型
    /// </summary>
    public class Inspection : ObservableObject
    {
        public string id { get; set; }
        public string patientName { get; set; }
        public string laboratoryCode { get; set; }
        public string code { get; set; }
        public string hospitalName { get; set; }
        public string doctorName { get; set; }
        public string salesUserName { get; set; }
        public string productName { get; set; }
        public string productId { get; set; }
        public string productType { get; set; }
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> gatherTime { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public Nullable<DateTime> receiverTime { get; set; }
        public Nullable<int> age { get; set; }

        // @ApiModelProperty(value = "是否加测 0 否 1是")
        public string addTest { get; set; }

        /// <summary>
        /// 病理类型：常规、分子、细胞等
        /// </summary>
        public string pathologyType { get; set; }

        private bool isSelected = false;

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
    }
}
