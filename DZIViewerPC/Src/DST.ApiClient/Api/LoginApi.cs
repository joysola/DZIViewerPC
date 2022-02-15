using DST.Database;
using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System.Collections.Generic;

namespace DST.ApiClient.Api
{
    /// <summary>
    /// 登录所需Api
    /// </summary>
    class LoginApi : BaseApi<LoginApi>
    {
        /// <summary>
        /// 登录获取token
        /// </summary>
        /// <param name="postLoginModel"></param>
        /// <returns></returns>
        [Url("dst-auth/oauth/login")]
        [HttpPost]
        internal ApiResponse<LoginModel> Login([PostContent] QueryLoginModel postLoginModel) => GetResult();
        /// <summary>
        /// 获取用户授权后的菜单信息
        /// 12118208329482361587 标记系统=》1. 1211826713957290011 标记页面；2. 1211826713957290012 标记符合页面
        /// </summary>
        /// <returns></returns>
        //[Url("api/deepsight-system/system/menu/routes")]
        [Url("api/deepsight-system/system/menu/query/routes?clientType=C")]
        [HttpGet]
        internal ApiResponse<List<MVAuthorizedMenus>> GetAuthorizedMens() => GetResult();
    }
}