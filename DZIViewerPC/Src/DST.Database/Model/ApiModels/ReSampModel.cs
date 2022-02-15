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
    /// 重新取样实体
    /// </summary>
    public class ReSampModel : ObservableObject
    {
        // <summary>
        /// 重新取样id
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 样本ID
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleID { get; set; }
        /// <summary>
        /// 重新取样的样本ID
        /// </summary>
        [Notification]
        [JsonProperty("resampleId")]
        public string ReSampleID { get; set; }
        /// <summary>
        /// 病理号
        /// </summary>
        [Notification]
        [JsonProperty("code")]
        public string Code { get; set; }
        // <summary>
        /// 实验室编号
        /// </summary>
        [Notification]
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
        // <summary>
        /// 重新取样的实验室编号
        /// </summary>
        [Notification]
        [JsonProperty("resLaboratoryCode")]
        public string ReLabCode { get; set; }
        // <summary>
        /// 医院id
        /// </summary>
        [Notification]
        [JsonProperty("hospitalId")]
        public string HospitalID { get; set; }
        // <summary>
        /// 医院名称
        /// </summary>
        [Notification]
        [JsonProperty("hospitalName")]
        public string HospitalName { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        [JsonProperty("patientName")]
        public string PatientName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [JsonProperty("age")]
        public int? Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty("sex")]
        public string Sex { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [Notification]
        [JsonProperty("productId")]
        public string ProductID { get; set; }
        /// <summary>
        /// 销售代表名称
        /// </summary>
        [Notification]
        [JsonProperty("salesUserName")]
        public string SalesUserName { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        [Notification]
        [JsonProperty("reason")]
        public string Reason { get; set; }
        /// <summary>
        /// 是否重新取样 0 未处理 1 已处理
        /// </summary>
        [Notification]
        [JsonProperty("status")]
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Notification]
        [JsonProperty("remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Notification]
        [JsonProperty("operateUserName")]
        public string OperUserName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [Notification]
        [JsonProperty("operateTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? OperTime { get; set; }
    }

    public class SubmitReSampleModel : BaseModel
    {
        /// <summary>
        /// 病理id
        /// </summary>
        [Notification]
        [JsonProperty("pathologyId")]
        public string PathID { get; set; }
        /// <summary>
        /// 样本ID
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleID { get; set; }

        /// <summary>
        /// 重新取样样本id
        /// </summary>
        [Notification]
        [JsonProperty("id")]
        public string ResampID { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        [Notification]
        [JsonProperty("reason")]
        public string Reason { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Notification]
        [JsonProperty("remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Notification]
        [JsonProperty("operateUserName")]
        public string OperUserName { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [Notification]
        [JsonProperty("operateUserId")]
        public string OperUserID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [Notification]
        [JsonProperty("operateTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? OperTime { get; set; }
    }
}
