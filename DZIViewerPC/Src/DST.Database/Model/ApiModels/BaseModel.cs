using DST.Common.Converter;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class BaseModel : ObservableObject
    {
        // <summary>
        /// 主键
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [JsonProperty("createUser")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 创建部门
        /// </summary>
        [JsonProperty("createDept")]
        public string CreateDept { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("createTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [JsonProperty("updateUser")]
        public string UpdateUser { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("updateTime")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 状态 0 未处理 1 已处理
        /// </summary>
        [JsonProperty("status")]
        public int? Status { get; set; }
        /// <summary>
        /// 是否已删除
        /// </summary>
        [JsonProperty("isDeleted")]
        public int? IsDeleted { get; set; }
        /// <summary>
        /// 客户id(用以区分医院)
        /// </summary>
        [JsonProperty("customerId")]
        public string CustomerId { get; set; }
    }
}
