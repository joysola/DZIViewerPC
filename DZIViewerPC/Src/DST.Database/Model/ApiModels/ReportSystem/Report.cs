using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class Report : ObservableObject
    {
        //(value = "样本id")
        public string id { get; set; }

        //(value = "病理id")
        public string pathologyId { get; set; }

        //(value = "病人id")
        public string patientId { get; set; }

        //(value = "病人名称")
        public string patientName { get; set; }

        //(value = "实验室编号")
        public string laboratoryCode { get; set; }

        //(value = "物流id")
        public string expressId { get; set; }

        //(value = "物流单号")
        public string mailNo { get; set; }

        //(value = "项目名称")
        public string productName { get; set; }

        //(value = "取样时间")
        public DateTime? gatherTime { get; set; }

        //(value = "送检样本")
        public string inspectionSample { get; set; }

        //(value = "病理编号")
        public string pathologyCode { get; set; }

        //(value = "年龄")
        public int? patientAge { get; set; }

        //(value = "性别")
        public string patientSex { get; set; }

        //(value = "报告结果")
        public string reportResult { get; set; }

        //(value = "报告时间")
        public DateTime? reportTime { get; set; }

        //(value = "报告路径")
        public string reportUrl { get; set; }

        //(value = "英文报告路径")
        public string reportUrlEnglish { get; set; }

        //(value = "报告确认标识（0：未确认 1：已确认）")
        public int? confirmStatus { get; set; }

        //(value = "导出状态（0：未导出 1：已导出）")
        public int? isDown { get; set; }

        /// <summary>
        ///(value = "是否加测（0：不加测 1：加测）")
        /// </summary>
        public int? isAdd { get; set; }

        /// <summary>
        /// "加测类型: 0：不加测 1：通用加测 2：特检加测"
        /// </summary>
        public int? addType { get; set; }

        // @ApiModelProperty(value = "科室")
        public string dept;

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
