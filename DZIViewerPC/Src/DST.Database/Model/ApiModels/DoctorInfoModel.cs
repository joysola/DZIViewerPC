using DST.Common.Converter;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class DoctorInfoModel
    {
        /// <summary>
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
        /// 业务状态
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
        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("userIdList")]
        public List<string> userIdList { get; set; }



        /// <summary>
        /// 医生姓名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
        /// <summary>
        /// 性别 男1 女2
        /// </summary>
        [JsonProperty("sex")]
        public int? Sex { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [JsonProperty("department")]
        public string Department { get; set; }
        /// <summary>
        /// 医院Id
        /// </summary>
        [JsonProperty("hospitalId")]
        public string HospitalId { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        [JsonProperty("hospitalName")]
        public string HospitalName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
