using DST.Common.Converter;
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
    /// <summary>
    /// 样本实体
    /// </summary>
    public class SampleModel : ObservableObject
    {
        /// <summary>
        /// 样本ID
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleID { get; set; }
        // <summary>
        /// 实验室编号
        /// </summary>
        [Notification]
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
        /// <summary>
        /// 病理号（登记右侧）
        /// </summary>
        [Notification]
        [JsonProperty("pathologyId")]
        public string PathID { get; set; }
        /// <summary>
        /// 病理号（重新关联）
        /// </summary>
        [Notification]
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [Notification]
        [JsonProperty("dept")]
        public string Dept { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        [Notification]
        [JsonProperty("doctorName")]
        public string DoctorName { get; set; }
        
        /// <summary>
        /// 患者姓名
        /// </summary>
        [Notification]
        [JsonProperty("patientName")]
        public string PatientName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [Notification]
        [JsonProperty("age")]
        public int? Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Notification]
        [JsonProperty("sex")]
        public string Sex { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [Notification]
        [JsonProperty("productId")]
        public string ProductID { get; set; }
        /// <summary>
        /// 检查项目
        /// </summary>
        [Notification]
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 包埋状态0 未完成 1 已完成
        /// </summary>
        [Notification]
        [JsonProperty("embeddingStatus")]
        public string EmbedStatus { get; set; }
        /// <summary>
        /// 送检样本
        /// </summary>
        [Notification]
        [JsonProperty("inspectionSample")]
        public string InspectSample { get; set; }
        /// <summary>
        /// 标记物
        /// </summary>
        [Notification]
        [JsonProperty("marker")]
        public string Marker { get; set; }
        /// <summary>
        /// 标记物
        /// </summary>
        [Notification]
        [JsonProperty("markers")]
        public string Markers { get; set; }
        /// <summary>
        /// 医嘱时间
        /// </summary>
        [Notification]
        [JsonProperty("adviceTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? AdviceTime { get; set; }
        /// <summary>
        /// 检查状态 0 待处理、1 已处理
        /// </summary>
        [Notification]
        [JsonProperty("status")]
        public string Status { get; set; }
        /// <summary>
        /// 是否加测项目
        /// </summary>
        [Notification]
        [JsonProperty("addTest")]
        public string AddTest { get; set; }
        /// <summary>
        /// 医嘱审核id
        /// </summary>
        [Notification]
        [JsonProperty("adviceAuditId")]
        public string AdviceAuditID { get; set; }
    }
}
