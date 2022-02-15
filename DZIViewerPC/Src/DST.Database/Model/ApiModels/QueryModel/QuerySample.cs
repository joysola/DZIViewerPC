using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class QuerySample
    {
        /// <summary>
        /// 取材类型： 0 待取材 1延缓取材 2 补取
        /// </summary>
        public string DrawMaterialsType { get; set; }
        /// <summary>
        /// 姓名（或 实验室编号）
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        public string LabCode { get; set; }
        /// <summary>
        /// 医嘱类型
        /// </summary>
        public string AdviceType { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
    /// <summary>
    /// 重新取样查询实体
    /// </summary>
    public class QueryReSample
    {
        /// <summary>
        /// 病理号
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 患者姓名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 实验室编号
        /// </summary>
        [JsonProperty("laboratoryCode")]
        public string LabCode { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty("startOperateTime")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty("endOperateTime")]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 是否重新取样
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
