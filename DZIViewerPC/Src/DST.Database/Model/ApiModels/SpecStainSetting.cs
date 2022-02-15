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
    /// 特殊染色配置
    /// </summary>
    public class SpecStainSetting : BaseModel
    {

        /// <summary>
        /// 项目id(特检类型)
        /// </summary>
        [Notification]
        [JsonProperty("productId")]
        public string ProductID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [Notification]
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        [Notification]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Notification]
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// 试剂组合key
        /// </summary>
        [Notification]
        [JsonProperty("groupKey")]
        public string GroupKey { get; set; }
        /// <summary>
        /// 试剂组合名称
        /// </summary>
        [Notification]
        [JsonProperty("groupKeyName")]
        public string GroupKeyName { get; set; }
    }
}
