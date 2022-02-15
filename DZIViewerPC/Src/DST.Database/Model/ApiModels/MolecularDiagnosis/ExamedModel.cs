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
    public class ExamedModel : ObservableObject
    {
        public string sampleId { get; set; }
        public string laboratoryCode { get; set; }
        public string patientName { get; set; }
        public string sex { get; set; }
        public int? age { get; set; }
        public string productName { get; set; }
        public string productType { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? gatherTime { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? receiverTime { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? inspectionTime { get; set; }
        public string reportResult { get; set; }
        public string reportLargeResult { get; set; }
        public string trialStatus { get; set; }
        public string importStatus { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? importTime { get; set; }

        // @ApiModelProperty(value = "是否加测 0 否 1是")
        public string addTest { get; set; }

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
