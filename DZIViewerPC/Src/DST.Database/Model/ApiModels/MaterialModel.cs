using DST.Common.Converter;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 组织实体
    /// </summary>
    public class SampTissModel : BaseModel
    {
        /// <summary>
        /// 取材状态 0 未取材 1 已取材
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsStatus")]
        public string DrawMaterStatus { get; set; }
        /// <summary>
        /// 取材类型 0 待取材 1延缓取材 2 补取
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsType")]
        public string DrawMaterType { get; set; }
        /// <summary>
        /// 包埋状态 0 未完成 1已完成
        /// </summary>
        [Notification]
        [JsonProperty("embeddingStatus")]
        public string EmbeddStatus { get; set; }
        /// <summary>
        /// 肉眼所见
        /// </summary>
        [JsonProperty("seenNakedEyes")]
        [Notification]
        public string NakedEyes { get; set; }
        /// <summary>
        /// 附言
        /// </summary>
        [Notification]
        [JsonProperty("postscript")]
        public string PostScript { get; set; }
        /// <summary>
        /// 样本ID
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleID { get; set; }
    }
    /// <summary>
    /// 送检部位实体
    /// </summary>
    public class InspSpecimen : BaseModel
    {

        /// <summary>
        /// 样本id
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleId { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>
        [Notification]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 关联字典名称Key值
        /// </summary>
        [Notification]
        [JsonProperty("dictKey")]
        public string DictKey { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Notification]
        [JsonProperty("number")]
        public int? Number { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Notification]
        [JsonProperty("sort")]
        public int? Sort { get; set; }
    }

    /// <summary>
    /// 样本部位明细
    /// </summary>
    public class SampSpecDetail : ObservableObject
    {
        /// <summary>
        /// 送检部位名称
        /// </summary>
        [Notification]
        [JsonProperty("inspectionSample")]
        public string InspecSampleName { get; set; }
        /// <summary>
        /// 样本id
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleId { get; set; }
        /// <summary>
        /// 送检部位名称
        /// </summary>
        [Notification]
        [JsonProperty("sampleSpecimenList")]
        public ObservableCollection<InspSpecimen> SampSpecList { get; set; } = new ObservableCollection<InspSpecimen>();
        /// <summary>
        /// 检验项目id
        /// </summary>
        [JsonIgnore]
        public string ProductID { get; set; }
        /// <summary>
        /// 样本状态 0 未确认 1 已经确认
        /// </summary>
        [JsonIgnore]
        public string SampStatus { get; set; }
    }
    /// <summary>
    /// 包埋盒打码机扫描对象
    /// </summary>
    public class EmbedPrintCode : ObservableObject
    {
        /// <summary>
        /// 患者姓名
        /// </summary>
        [Notification]
        [JsonProperty("patientName")]
        public string PatientName { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        [Notification]
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
        /// <summary>
        /// 蜡块编号
        /// </summary>
        [Notification]
        [JsonProperty("waxBlockCode")]
        public string WaxBlockCode { get; set; }
        /// <summary>
        /// 蜡块号
        /// </summary>
        [Notification]
        [JsonProperty("waxBlockNumber")]
        public string WaxBlockNum { get; set; }

        /// <summary>
        /// 取材部位
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsPlace")]
        public string DrawMaterPlace { get; set; }
    }

    /// <summary>
    /// 延缓取材
    /// </summary>
    public class SampTissDelayInfo : BaseModel
    {
        /// <summary>
        /// 样本id
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleId { get; set; }
        /// <summary>
        /// 延缓原因
        /// </summary>
        [Notification]
        [JsonProperty("delayReason")]
        public string DelayReason { get; set; }


        /// <summary>
        /// 预计完成时间
        /// </summary>
        [Notification]
        [JsonProperty("completionTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CompleteTime { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Notification]
        [JsonProperty("remark")]
        public string Remark { get; set; }
    }
}
