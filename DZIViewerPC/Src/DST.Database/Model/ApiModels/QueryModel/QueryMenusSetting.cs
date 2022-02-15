using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 角色菜单更新查询实体
    /// </summary>
    public class QueryMenusSetting
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 半选菜单id集合
        /// </summary>
        [JsonProperty("indeterminateMenuIds")]
        public List<string> HalfMenuIDs { get; set; } = new List<string>();
        /// <summary>
        /// 勾选菜单id集合
        /// </summary>
        [JsonProperty("menuIds")]
        public List<string> MenuIDs { get; set; } = new List<string>();
        /// <summary>
        /// 菜单类型 1：B端、2：C端
        /// </summary>
        [JsonProperty("menuType")]
        public int? menuType { get; set; } = 2;
    }
}
