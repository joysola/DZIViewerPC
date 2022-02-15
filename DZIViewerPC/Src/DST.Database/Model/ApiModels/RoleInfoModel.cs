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
    public class RoleInfoModel : ObservableObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        /// <summary>
        /// 角色别名
        /// </summary>
        [JsonProperty("roleAlias")]
        public string RoleAlias { get; set; }
        /// <summary>
        /// 父级角色
        /// </summary>
        [Notification]
        [JsonProperty("parentId")]
        public string ParentID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [JsonProperty("remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        [JsonProperty("children")]
        public List<RoleInfoModel> Children { get; set; } = new List<RoleInfoModel>();
    }
}
