using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;

namespace DST.Database
{
    /// <summary>
    /// 登录实体
    /// </summary>
    [NotifyAspect]
    public class LoginModel : ObservableObject
    {
        /// <summary>
        /// token
        /// </summary>
        [JsonProperty("access_token")]
        public string Access_Token { get; set; }

        /// <summary>
        /// token类型
        /// </summary>
        [JsonProperty("token_type")]
        public string Token_Type { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("refresh_token")]
        public string Refresh_Token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [JsonProperty("expires_in")]
        public int Expires_In { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("license")]
        public string License { get; set; }

        /// <summary>
        ///
        /// </summary>
        //public string user_id { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        [JsonProperty("user_name")]
        public string User_Name { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        [JsonProperty("realName")]
        public string RealName { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        [JsonProperty("dept_id")]
        public string Dept_Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("jti")]
        public string Jti { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        [JsonProperty("station")]
        public string Station { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        [JsonProperty("aliasName")]
        public string AliasName { get; set; }

        /// <summary>
        /// 客户ID
        /// </summary>
        [JsonProperty("customer_id")]
        public string Customer_id { get; set; }
    }
}