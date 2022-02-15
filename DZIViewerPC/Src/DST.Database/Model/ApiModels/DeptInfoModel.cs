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
    /// 部门信息
    /// </summary>
    public class DeptInfoModel : ObservableObject
    {
        /// <summary>
        /// 主键
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; } = string.Empty;
        /// <summary>
        /// 科室（部门）名称
        /// </summary>
        [JsonProperty("deptName")]
        public string DeptName { get; set; }
        /// <summary>
        /// 从父级到子级的完整名称
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        [Notification]
        [JsonProperty("parentId")]
        public string ParentID { get; set; }

        /// <summary>
        /// 部门类型（部门：2、公司：1）
        /// </summary>
        [JsonProperty("deptType")]
        public string DeptType { get; set; } = "2";
        /// <summary>
        /// 子部门
        /// </summary>
        [JsonProperty("children")]
        public List<DeptInfoModel> Children { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
    /// <summary>
    /// 明细
    /// </summary>
    public class DeptDetailModel : DeptInfoModel
    {
        /// <summary>
        /// 父级名称
        /// </summary>
        [JsonProperty("parentName")]
        public string ParentName { get; set; }
        /// <summary>
        /// 从父级到子级的拼接路径
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// 从父级到子级的拼接路径名称
        /// </summary>
        [JsonProperty("pathName")]
        public string PathName { get; set; }

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
        /// 电话
        /// </summary>
        [JsonProperty("telephone")]
        public string Telephone { get; set; }

    }
    /// <summary>
    /// 删除部门实体
    /// </summary>
    public class QueryDeptInfoDelete
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }
}
