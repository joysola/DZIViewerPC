using DST.Common.Converter;
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
    /// 包埋盒实体
    /// </summary>
    public class EmbedBoxModel : BaseModel
    {
        /// <summary>
        /// 样本id
        /// </summary>
        [Notification]
        [JsonProperty("sampleId")]
        public string SampleId { get; set; }
        /// <summary>
        /// 标本ID
        /// </summary>
        [Notification]
        [JsonProperty("sampleSpecimenId")]
        public string SampSpecID { get; set; }
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
        /// 备注
        /// </summary>
        [Notification]
        [JsonProperty("remark")]
        public string Remark { get; set; }
        /// <summary>
        ///  打印状态(1：已打印，2未打印)
        /// </summary>
        [Notification]
        [JsonProperty("printStatus")]
        public string PrintStatus { get; set; }

        /// <summary>
        /// 取材人员姓名
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsUserName")]
        public string DrawMaterUserName { get; set; }

        /// <summary>
        /// 取材人员id
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsUserId")]
        public string DrawMaterUserID { get; set; }

        /// <summary>
        /// 取材部位
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsPlace")]
        public string DrawMaterPlace { get; set; }
        /// <summary>
        /// 取材时间
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? DrawMaterTime { get; set; }
        /// <summary>
        /// 包埋时间
        /// </summary>
        [Notification]
        [JsonProperty("embeddingTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? EmbedTime { get; set; }
        /// <summary>
        /// 取材状态 0 未完成  1 已完成
        /// </summary>
        [Notification]
        [JsonProperty("drawMaterialsStatus")]
        public string DrawMaterStatus { get; set; }
        /// <summary>
        /// 包埋状态 0 未完成 1 已完成
        /// </summary>
        [Notification]
        [JsonProperty("embeddingStatus")]
        public string EmbedStatus { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Notification]
        [JsonProperty("sort")]
        public int? Sort { get; set; }
        /// <summary>
        /// 标本部位名称
        /// </summary>
        [Notification]
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
