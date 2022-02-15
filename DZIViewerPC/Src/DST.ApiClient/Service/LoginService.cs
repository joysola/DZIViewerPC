using DST.ApiClient.Api;
using DST.Database;
using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Service;
using System.Collections.Generic;
using System.Linq;

namespace DST.ApiClient.Service
{
    /// <summary>
    /// 登录服务
    /// </summary>
    public class LoginService : BaseService<LoginService>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="queryLoginModel"></param>
        /// <returns></returns>
        public LoginModel Login(QueryLoginModel queryLoginModel)
        {
            var response = LoginApi.Client.Login(queryLoginModel);
            if (response.Success)
            {
                HttpClientEx.SetCustomRequestHead("deepsight-auth", $"{response.Data.Token_Type} {response.Data.Access_Token}");
            }
            return response.Data;
        }

        /// <summary>
        /// 获取登录者的菜单列表
        /// 12118208329482361587 标记系统=》1. 1211826713957290011 标记页面；2. 1211826713957290012 标记符合页面
        /// </summary>
        public List<MVAuthorizedSubMenu> GetAuthorizedMens()
        {
            List<MVAuthorizedSubMenu> result = new List<MVAuthorizedSubMenu>();
            List<MVAuthorizedMenus> menus = LoginApi.Client.GetAuthorizedMens().Data;
            if (menus != null)
            {
                MVAuthorizedMenus markMenus = menus.FirstOrDefault(x => x.ID.Equals("12118208329482361587"));
                if (markMenus != null)
                {
                    result = markMenus.Children;
                }
            }

            return result;
        }
    }
}