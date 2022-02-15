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
    /// 菜单实体
    /// </summary>
    public class MenuInfoModel
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        [JsonProperty("parentId")]
        public string ParentID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 菜单路径
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }
        /// <summary>
        /// 菜单code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 菜单别名
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        [JsonProperty("children")]
        public List<MenuInfoModel> Children { get; set; }
        /// <summary>
        /// 对应菜单下的按钮键值
        /// </summary>
        [JsonProperty("codeList")]
        public List<string> CodeList { get; set; }
        /// <summary>
        /// 保存本地图片
        /// </summary>
        [JsonIgnore]
        public string Url { get; set; }
    }

    /// <summary>
    /// 菜单明细实体
    /// </summary>
    public class MenuDetailModel
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        [JsonProperty("parentId")]
        public string ParentID { get; set; }
        /// <summary>
        /// 父菜单名称
        /// </summary>
        [JsonProperty("parentName")]
        public string ParentName { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 菜单路径
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }
        /// <summary>
        /// 菜单code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }
        /// <summary>
        /// 菜单别名
        /// </summary>
        [JsonProperty("alias")]
        public string Alias { get; set; }
      
        /// <summary>
        /// 对应菜单下的按钮键值
        /// </summary>
        [JsonProperty("codeList")]
        public List<string> CodeList { get; set; }
        /// <summary>
        /// 菜单类型 1：b端、2：c端
        /// </summary>
        [JsonProperty("menuType")]
        public int? MenuType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty("status")]
        public int? Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [JsonProperty("sort")]
        public int? Sort { get; set; }


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
        /// 是否已删除
        /// </summary>
        [JsonProperty("isDeleted")]
        public int? IsDeleted { get; set; }
    }
}
