using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 物流患者信息
    /// </summary>
    public class ExpressPathology : ObservableObject
    {
        private List<Sample> _sampleList = null;
        public string id { get; set; }
        public string hospitalId { get; set; }
        public string doctorId { get; set; }
        public string doctorName { get; set; }
        public string patientId { get; set; }
        public string patientName { get; set; }

        [Notification]
        public Nullable<int> age { get; set; }
        public string sex { get; set; }
        public string phone { get; set; }

        /// <summary>
        /// 病理号
        /// </summary>
        public string code { get; set; }

        [Notification]
        public string productNames { get; set; }
        public List<Sample> sampleList 
        {
            get { return this._sampleList; }
            set
            {
                this._sampleList = value;
                if(this._sampleList != null && this._sampleList.Count > 0)
                {
                    this.productNames = string.Join(";", this._sampleList.Select(x => x.productName).ToArray());
                }
                else
                {
                    this.productNames = string.Empty;
                }
            }
        }
        public string expressId { get; set; }
        public string inpatientNumber { get; set; }
        public string bedNumber { get; set; }
    }

}
