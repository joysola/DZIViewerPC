using DST.Database.Model;
using HttpClientExtension.ApiClient;
using HttpClientExtension.Attribute;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DST.ApiClient.Api
{
    class SysManageApi : BaseApi<SysManageApi>
    {
        #region 用户
        /// <summary>
        /// 根据键值获取字典(注意每次返回一个数组,但是实际上只会取第一项)
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [Url("dst-system/system/user/getDetailsById")]
        [HttpGet]
        internal ApiResponse<UserInfoModel> GetUserDetailbyId(string id) => GetResult();

        /// <summary>
        /// 获取当前登录用户部分信息
        /// </summary>
        /// <returns></returns>
        [Url("dst-system/system/user/getLoginUserDetail")]
        [HttpGet]
        internal ApiResponse<LoginUserDetailModel> GetLoginUserDetail() => GetResult();



        /// <summary>
        /// 根据查询分页条件获取用户信息列表
        /// </summary>
        /// <param name="size"></param>
        /// <param name="current"></param>
        /// <param name="userName">登录名</param>
        /// <param name="realName">用户姓名</param>
        /// <param name="deptID">部门号</param>
        /// <returns></returns>
        //[Url("dst-system/system/user/pageByUser")]
        [Url("dst-system/system/user/client/pageByUser")]
        [HttpGet]
        internal ApiResponse<ResponsePage<UserInfoModel>> GetUserListbyPage(int size, int current, [ParamName("username")] string userName, string realName, [ParamName("deptId")] string deptID = "") => GetResult();

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userInfo">用户实体</param>
        /// <returns></returns>
        [Url("dst-system/system/user/save")]
        [HttpPost]
        internal ApiResponse<object> AddUser([PostContent] UserInfoModel userInfo) => GetResult();

        /// <summary>
        /// 删除(逻辑)用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [Url("dst-system/system/user/removeById")]
        [HttpGet]
        internal ApiResponse<object> DeleteUser(string id) => GetResult();

        /// <summary>
        /// 批量删除(逻辑)用户
        /// </summary>
        /// <param name="ids">用户id集合</param>
        /// <returns></returns>
        [Url("dst-system/system/user/removeByIds")]
        [HttpGet]
        internal ApiResponse<object> DeleteUserList(IEnumerable<string> ids) => GetResult();
        /// <summary>
        /// 批量删除(逻辑)用户
        /// </summary>
        /// <param name="ids">用户id集合(用逗号隔开)</param>
        /// <returns></returns>
        [Url("dst-system/system/user/removeByIds")]
        [HttpGet]
        internal ApiResponse<object> DeleteUserList(string ids) => GetResult();

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="ids">用户实体</param>
        /// <returns></returns>
        [Url("dst-system/system/user/update")]
        [HttpPost]
        internal ApiResponse<object> UpdateUser([PostContent] UserInfoModel userInfoModel) => GetResult();

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [Url("dst-system/system/user/resetPassword")]
        [HttpGet]
        internal ApiResponse<object> ResetPassword(string id) => GetResult();

        /// <summary>
        /// 锁住用户
        /// </summary>
        /// <param name="lockUser"></param>
        /// <returns></returns>
        [Url("dst-system/system/user/lockUser")]
        [HttpGet]
        internal ApiResponse<object> LockUser(bool isLock, string userId) => GetResult();

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [Url("dst-system/system/user/updatePassword")]
        [HttpPost]
        internal ApiResponse<object> UpdatePassword([PostContent] UserInfoModel userInfo) => GetResult();

        #endregion 用户

        #region 角色
        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="roleAlias">别名</param>
        /// <returns></returns>
        [Url("dst-system/system/role/listTree")]
        [HttpGet]
        internal ApiResponse<List<RoleInfoModel>> GetRoleListTree(string roleName, string roleAlias) => GetResult();

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [Url("dst-system/system/role/remove")]
        [HttpGet]
        internal ApiResponse<object> RemoveRole(string id) => GetResult();
        /// <summary>
        /// 新增或修改角色(id 不存在：新增、id 存在：修改)
        /// </summary>
        /// <param name="roleInfo">角色实体</param>
        /// <returns></returns>
        [Url("dst-system/system/role/save")]
        [HttpPost]
        internal ApiResponse<object> SaveRole([PostContent] RoleInfoModel roleInfo) => GetResult();
        #endregion 角色

        #region 菜单
        /// <summary>
        /// 根据菜单id 获取某菜单的明细信息
        /// </summary>
        /// <param name="id">菜单id</param>
        /// <returns></returns>
        [Url("dst-system/system/menu/getDetailsById")]
        [HttpGet]
        internal ApiResponse<MenuDetailModel> GetMenuDetailsbyId(string id) => GetResult();

        #endregion 菜单

        #region 角色菜单
        /// <summary>
        /// 获取C端人员的菜单权限
        /// </summary>
        /// <returns></returns>
        [Url("dst-system/system/role-menu/clientListMenuTreeByUserId")]
        [HttpGet]
        internal ApiResponse<List<MenuInfoModel>> GetCSLoginUserMenus() => GetResult();
        /// <summary>
        /// 获取该客户下的权限
        /// </summary>
        /// <param name="menuType">1： B端，2： C端</param>
        /// <returns></returns>
        [Url("dst-system/system/role-menu/listMenuTree")]
        [HttpGet]
        internal ApiResponse<List<MenuInfoModel>> GetCustomMenus(int menuType = 2) => GetResult();
        /// <summary>
        /// 根据角色获取对应菜单id集合
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="menuType">1： B端，2： C端</param>
        /// <returns></returns>
        [Url("dst-system/system/role-menu/listMenuTreeByRoleId")]
        [HttpGet]
        internal ApiResponse<List<string>> GetMenusbyRoleId(string roleId, int menuType = 2) => GetResult();

        /// <summary>
        /// 获取B端人员的菜单权限（废弃）
        /// </summary>
        /// <returns></returns>
        [Url("dst-system/system/role-menu/listMenuTreeByUserId")]
        [HttpGet]
        private ApiResponse<object> GetBSLoginUserMenus() => GetResult();

        /// <summary>
        /// 更新角色菜单
        /// </summary>
        /// <returns></returns>
        [Url("dst-system/system/role-menu/updateRoleMenu")]
        [HttpPost]
        internal ApiResponse<object> UpdateMenusofRole([PostContent] QueryMenusSetting setting) => GetResult();
        #endregion 角色菜单


        #region 部门
        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <returns></returns>
        //[Url("dst-system/system/dept/listTree")]
        [Url("dst-system/system/dept/client/listTree")]
        [HttpGet]
        internal ApiResponse<List<DeptInfoModel>> GetDeptTrees(string deptName) => GetResult();
        /// <summary>
        /// 获取某部门详细信息
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        [Url("dst-system/system/dept/getDetailsById")]
        [HttpGet]
        internal ApiResponse<DeptDetailModel> GetDeptDetail(string id) => GetResult();

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        [Url("dst-system/system/dept/removeById")]
        [HttpPost]
        internal ApiResponse<DeptDetailModel> RemoveDeptbyID(string id) => GetResult();
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="queryDept">部门实体</param>
        /// <returns></returns>
        [Url("dst-system/system/dept/save")]
        [HttpPost]
        internal ApiResponse<object> AddDeptInfo([PostContent] DeptDetailModel deptModel) => GetResult();
        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="deptModel"></param>
        /// <returns></returns>
        [Url("dst-system/system/dept/update")]
        [HttpPost]
        internal ApiResponse<object> UpdateDeptInfo([PostContent] DeptDetailModel deptModel) => GetResult();
        #endregion 部门

        #region 医生
        /// <summary>
        /// C端医生分页查询
        /// </summary>
        /// <param name="doctorInfo">医生实体</param>
        /// <returns></returns>
        [Url("dst-fund/fund/doctor/client/pageByDoctor")]
        [HttpPost]
        internal ApiResponse<ResponsePage<DoctorInfoModel>> GetDocListbyPage(int size, int current, [PostContent] DoctorInfoModel doctorInfo) => GetResult();

        /// <summary>
        /// 新建C端医生信息
        /// </summary>
        /// <param name="doctorInfo"></param>
        /// <returns></returns>
        [Url("dst-fund/fund/doctor/client/saveDoctor")]
        [HttpPost]
        internal ApiResponse<object> AddDoctor([PostContent] DoctorInfoModel doctorInfo) => GetResult();
        
        /// <summary>
        /// 更新C端医生信息
        /// </summary>
        /// <param name="doctorInfo"></param>
        /// <returns></returns>
        [Url("dst-fund/fund/doctor/client/updateDoctor")]
        [HttpPost]
        internal ApiResponse<object> UpdateDoctor([PostContent] DoctorInfoModel doctorInfo) => GetResult();

        /// <summary>
        /// 删除C端医生信息
        /// </summary>
        /// <param name="id">医生id</param>
        /// <returns></returns>
        [Url("dst-fund/fund/doctor/client/removeById")]
        [HttpGet]
        internal ApiResponse<object> DeleteDoctor(string id) => GetResult();
        #endregion 医生

    }
}
