using DST.Common.Converter;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class UserInfoModel : BaseModel
    {
        /// <summary>
        /// 状态（0 锁定、1 正常）
        /// </summary>
  

        /// <summary>
        /// 客户id(用以区分医院)
        /// </summary>
       
        /// <summary>
        /// 用户名
        /// </summary>
        [Notification]
        [JsonProperty("username")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        [JsonProperty("realName")]
        [Notification]
        public string RealName { get; set; }
        /// <summary>
        /// 英文名称
        /// </summary>
        [JsonProperty("enName")]
        public string EnName { get; set; }
        /// <summary>
        /// 性别  男1 女2 
        /// </summary>
        [JsonProperty("sex")]
        public int? Sex { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
        /// <summary>
        /// 办公电话
        /// </summary>
        [JsonProperty("officeTel")]
        public string OfficeTel { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        [JsonProperty("station")]
        public string Station { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        [JsonProperty("joinedDate")]
        //[JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? JoinedDate { get; set; }
        /// <summary>
        /// 是否更新过密码标识  0为未更新过  1为更新过
        /// </summary>
        [JsonProperty("updatePw")]
        public int? UpdatePw { get; set; }
        /// <summary>
        /// 名称转拼音首字母
        /// </summary>
        [JsonProperty("firstSpell")]
        public string FirstSpell { get; set; }
        /// <summary>
        /// 微信open_id
        /// </summary>
        [JsonProperty("openId")]
        public string OpenId { get; set; }
        /// <summary>
        /// 修改密码时的 旧密码
        /// </summary>
        [Notification]
        [JsonProperty("oldPassword")]
        public string OldPassword { get; set; }
        /// <summary>
        /// 修改密码时的 新密码
        /// </summary>
        [Notification]
        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }
        /// <summary>
        /// 拥有的角色
        /// </summary>
        //[Notification]
        [JsonProperty("roles")]
        public ObservableCollection<RoleInfoModel> Roles { get; set; } = new ObservableCollection<RoleInfoModel>();
        /// <summary>
        /// 所属部门
        /// </summary>
        //[Notification]
        [JsonProperty("depts")]
        public ObservableCollection<DeptInfoModel> Depts { get; set; } = new ObservableCollection<DeptInfoModel>();
        /// <summary>
        /// 角色id集合（用于传给中台）
        /// </summary>
        [JsonProperty("roleId")]
        public List<string> RoleId => Roles.Select(x => x.ID).ToList();
        /// <summary>
        /// 部门id集合（用于传给中台）
        /// </summary>
        [JsonProperty("deptId")]
        public List<string> DeptID => Depts.Select(x => x.ID).ToList();
        /// <summary>
        /// 确认密码
        /// </summary>
        [Notification]
        [JsonIgnore]
        public string CheckPassword { get; set; }
    }

    /// <summary>
    /// 当前登录用户部分信息
    /// </summary>
    public class LoginUserDetailModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("loginUserName")]
        public string LoginUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("loginCustomerName")]
        public string LoginCustomerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("now")]
        public DateTime? Now { get; set; }
    }
}
