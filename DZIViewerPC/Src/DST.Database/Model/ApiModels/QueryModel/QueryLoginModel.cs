using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;

namespace DST.Database.Model
{
    /// <summary>
    /// 查询登录实体
    /// </summary>
    public class QueryLoginModel : ObservableObject
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [JsonProperty("username")]
        [Notification]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty("password")]
        [Notification]
        public string Password { get; set; }
    }
}