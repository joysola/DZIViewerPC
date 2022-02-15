using DST.Common.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 病理信息
    /// </summary>
    public class PathInfoModel
    {
        /// <summary>
        /// Code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [JsonProperty("dept")]
        public string Dept { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [JsonProperty("doctorName")]
        public string DoctorName { get; set; }
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
    }

    /// <summary>
    /// 样本登记实体
    /// </summary>
    public class SampleRegisterModel
    {
        /// <summary>
        /// 样本id
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 病理号
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [JsonProperty("dept")]
        public string Dept { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        [JsonProperty("doctorName")]
        public string DoctorName { get; set; }
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
        /// 检查项目
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 检查类型
        /// </summary>
        [JsonProperty("productType")]
        public string ProductType { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        [JsonProperty("checkTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CheckTime { get; set; }
        /// <summary>
        /// 登记人员
        /// </summary>
        [JsonProperty("checkUser")]
        public string CheckUser { get; set; }
        /// <summary>
        /// 检查状态 0 待处理、1 已处理
        /// </summary>
        [JsonProperty("status")]
        public int? Status { get; set; }
    }
}
