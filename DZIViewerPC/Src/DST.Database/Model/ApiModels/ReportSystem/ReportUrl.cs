using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class ReportUrl : ObservableObject
    {
        //@ApiModelProperty(value = "样本id")
        public string sampleId { get; set; }

        //@ApiModelProperty(value = "报告路径")
        public string reportUrl { get; set; }

        //@ApiModelProperty(value = "英文报告路径")
        public string reportUrlEnglish { get; set; }

        /// <summary>
        /// 根据url下载到本地的PDF路径
        /// </summary>
        [Notification]
        [JsonIgnore]
        public string LocalReportUrl { get; set; }

        /// <summary>
        /// 根据url 下载到本地的英文报告PDF路径
        /// </summary>
        [Notification]
        [JsonIgnore]
        public string LocalReportUrlEnglish { get; set; }
    }
}
