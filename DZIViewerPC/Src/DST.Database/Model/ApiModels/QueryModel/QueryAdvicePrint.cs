using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 医嘱切片打印查询实体
    /// </summary>
    public class QueryAdvicePrint
    {
        /// <summary>
        /// 医嘱审核id
        /// </summary>
        [JsonProperty("adviceAuditId")]
        public string AdviceAuditID { get; set; }
        /// <summary>
        /// 类型 技术医嘱3， 特检是4
        /// </summary>
        [JsonProperty("cellType")]
        public string CellType { get; set; }
        /// <summary>
        /// 样本id
        /// </summary>
        [JsonProperty("sampleId")]
        public string SampleID { get; set; }
        /// <summary>
        /// 切片id List
        /// </summary>
        [JsonProperty("cuttingIds")]
        public List<string> CuttingIDs { get; set; } = new List<string>();
    }
}
