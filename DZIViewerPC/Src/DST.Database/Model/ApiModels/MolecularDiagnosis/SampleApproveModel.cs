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
    /// 样本审核
    /// </summary>
    public class SampleApproveModel : ObservableObject
    {
        public string sampleId { get; set; }
        public string laboratoryCode { get; set; }
        public string patientName { get; set; }
        public string sex { get; set; }
        public int? age { get; set; }
        public string productName { get; set; }
        public string productType { get; set; }
        public string productId { get; set; }
        public string hospitalName { get; set; }
        [JsonConverter(typeof(DST.Common.Converter.CustomDateTimeConverter))]
        public DateTime? reportTime { get; set; }
        public string reportResult { get; set; }
        public string reportUrl { get; set; }
        public string reportUrlEnglish { get; set; }

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
