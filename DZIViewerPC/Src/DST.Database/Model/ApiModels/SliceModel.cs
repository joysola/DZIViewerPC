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
    /// 切片实体
    /// </summary>
    public class SliceModel : BaseModel
    {
        /// <summary>
        /// 样本id
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleID { get; set; }
        /// <summary>
        /// 切片短编号
        /// </summary>
        [Notification]
        [JsonProperty("sliceShortNumber")]
        public string SliceShortNum { get; set; }

        /// <summary>
        /// 切片编号
        /// </summary>
        [Notification]
        [JsonProperty("sliceNumber")]
        public string SliceNum { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>
        [Notification]
        [JsonProperty("sampleSpecimenName")]
        public string SampSpecName { get; set; }

        /// <summary>
        /// 送检部位
        /// </summary>
        [Notification]
        [JsonProperty("inspectionPlace")]
        public string InspecPlace { get; set; }

        /// <summary>
        /// 取材部位
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsPlace")]
        public string DrawMaterPlace { get; set; }

        /// <summary>
        /// 取材id
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsId")]
        public string DrawMaterialsID { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        [Notification]
        [JsonProperty("productId")]
        public string ProductID { get; set; }

        /// <summary>
        /// 标记物
        /// </summary>
        [Notification]
        [JsonProperty("marker")]
        public string Marker { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Notification]
        [JsonProperty("remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 蜡块号
        /// </summary>
        [Notification]
        [JsonProperty("waxBlockNumber")]
        public string WaxBlockNum { get; set; }
        /// <summary>
        ///  打印状态(1：已打印，2未打印)
        /// </summary>
        [Notification]
        [JsonProperty("printStatus")]
        public string PrintStatus { get; set; }
        /// <summary>
        ///  排序
        /// </summary>
        [Notification]
        [JsonProperty("sort")]
        public int? Sort { get; set; }
        /// <summary>
        ///  医嘱类型
        /// </summary>
        [Notification]
        [JsonProperty("adviceType")]
        public string AdviceType { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        [Notification]
        [JsonIgnore]
        public bool IsSelected { get; set; }
        /// <summary>
        ///  包埋时间
        /// </summary>
        [Notification]
        [JsonProperty("embeddingTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? EmbedTime { get; set; }
        
    }
    /// <summary>
    /// 切片打印实体
    /// </summary>
    public class SlicePrintCode
    {
        /// <summary>
        /// 样本id
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleID { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        [Notification]
        [JsonProperty("patientName")]
        public string PatientName { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        [Notification]
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
        /// <summary>
        /// 切片编号
        /// </summary>
        [Notification]
        [JsonProperty("sliceNumber")]
        public string SliceNum { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [Notification]
        [JsonProperty("hospitalName")]
        public string HospName { get; set; }
        /// <summary>
        /// 切片短编号
        /// </summary>
        [Notification]
        [JsonProperty("sliceShortNumber")]
        public string SliceShortNum { get; set; }
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
        /// 省名称
        /// </summary>
        [Notification]
        [JsonProperty("provinceName")]
        public string ProvinceName { get; set; }
        /// <summary>
        /// 市名称
        /// </summary>
        [Notification]
        [JsonProperty("cityName")]
        public string CityName { get; set; }
        /// <summary>
        /// 区名称
        /// </summary>
        [Notification]
        [JsonProperty("areaName")]
        public string AreaName { get; set; }
        /// <summary>
        /// 取材部位
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsPlace")]
        public string DrawMaterPlace { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        [Notification]
        [JsonProperty("hospitalId")]
        public string HospID { get; set; }
        /// <summary>
        /// 项目号
        /// </summary>
        [Notification]
        [JsonProperty("productId")]
        public string ProductID { get; set; }
        /// <summary>
        /// 标记物/染色剂
        /// </summary>
        [Notification]
        [JsonProperty("marker")]
        public string Marker { get; set; }
        /// <summary>
        /// 送检部位
        /// </summary>
        [Notification]
        [JsonProperty("inspectionPlace")]
        public string InspecPlace { get; set; }
    }
    /// <summary>
    /// TCT制片
    /// </summary>
    public class SliceTCTModel : SliceModel
    {

        /// <summary>
        /// 病理号(样本编码)
        /// </summary>
        [Notification]
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        [Notification]
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
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
        /// 项目名称
        /// </summary>
        [Notification]
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 送检时间
        /// </summary>
        [JsonProperty("inspectionTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? InspecTime { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [Notification]
        [JsonProperty("dept")]
        public string Dept { get; set; }
        /// <summary>
        /// 送检医生名称
        /// </summary>
        [Notification]
        [JsonProperty("doctorName")]
        public string DoctorName { get; set; }

        /// <summary>
        /// 是否加测项目 （0：为加测、1：加测）
        /// </summary>
        [Notification]
        [JsonProperty("addTest")]
        public string AddTest { get; set; }

    }
    /// <summary>
    /// 切片完成情况统计实体
    /// </summary>
    public class SliceStatistics : ObservableObject
    {
        /// <summary>
        /// 总数
        /// </summary>
        [JsonProperty("totalAmount")]
        [Notification]
        public int? TotalAmount { get; set; }
        /// <summary>
        /// 完成数量
        /// </summary>
        [Notification]
        [JsonProperty("completeAmount")]
        public int? CompleteAmount { get; set; }
        /// <summary>
        /// 进度
        /// </summary>
        [JsonIgnore]
        [Notification]
        public double ProcessValue { get => !TotalAmount.HasValue ? 0.0 : (CompleteAmount ?? 0.0) / TotalAmount.Value * 100.0; set => _ = value; }
    }

    /// <summary>
    /// 样本制片实体
    /// </summary>
    public class SliceProdModel : SliceModel
    {
        /// <summary>
        /// 样本编码
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
        /// <summary>
        /// 病人名称
        /// </summary>
        [JsonProperty("patientName")]
        public string PatientName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty("sex")]
        public string Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        [JsonProperty("age")]
        public int? Age { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [JsonProperty("dept")]
        public string Dept { get; set; }
        /// <summary>
        /// 送检医生名称
        /// </summary>
        [JsonProperty("doctorName")]
        public string DoctorName { get; set; }

        /// <summary>
        /// 是否加测 0 否 1是
        /// </summary>
        [JsonProperty("addTest")]
        public string AddTest { get; set; }
        /// <summary>
        /// 制片时间
        /// </summary>
        [JsonProperty("makeTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? MakeTime { get; set; }
        /// <summary>
        /// 制片人员
        /// </summary>
        [JsonProperty("makeUserName")]
        public string MakeUserName { get; set; }
    }
}
