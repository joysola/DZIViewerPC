using DST.ApiClient.Api;
using DST.Database.Model;
using DST.Database.WPFCommonModels;
using HttpClientExtension.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace DST.ApiClient.Service
{
    public class SysManageService : BaseService<SysManageService>
    {
        #region 用户
        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserInfoModel GetUserDetailbyId(string id)
        {
            UserInfoModel result = null;
            var response = SysManageApi.Client.GetUserDetailbyId(id);
            if (response.Success)
            {
                result = response.Data;
            }
            return result;
        }
        /// <summary>
        /// 获取当前登录用户部分信息
        /// </summary>
        /// <returns></returns>
        public LoginUserDetailModel GetLoginUserDetail()
        {
            var result = SysManageApi.Client.GetLoginUserDetail().Data;
            return result;
        }
        /// <summary>
        /// 获取登录用户列表
        /// </summary>
        /// <param name="pageModel">分页实体</param>
        /// <param name="userInfo">查询的信息</param>
        /// <returns></returns>
        public List<UserInfoModel> GetUserListbyPage(CustomPageModel pageModel, UserInfoModel userInfo, string deptID = "")
        {
            var response = SysManageApi.Client.GetUserListbyPage(pageModel.PageSize, pageModel.PageIndex, userInfo.UserName, userInfo.RealName, deptID).Data;
            pageModel.TotalNum = response.Total;
            pageModel.TotalPage = response.Pages;
            var result = response.Records;
            return result;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userInfo">用户实体</param>
        /// <returns></returns>
        public bool AddUser(UserInfoModel userInfo)
        {
            var response = SysManageApi.Client.AddUser(userInfo);
            return response.Success;
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userInfo">用户实体</param>
        /// <returns></returns>
        public bool DeleteUser(UserInfoModel userInfo)
        {
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.ID))
            {
                var response = SysManageApi.Client.DeleteUser(userInfo.ID);
                return response.Success;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="userInfos"></param>
        public bool DeleteUserList(IEnumerable<UserInfoModel> userInfos)
        {
            if (userInfos != null && userInfos.Count() > 0)
            {
                var idList = userInfos.Select(x => x.ID);
                var ids = string.Join(",", idList); // 需要删除的参数用逗号隔开
                //var xx2 = ids.GetType();
                //if (xx2 == typeof(IEnumerable<string>))
                //{

                //}
                var response = SysManageApi.Client.DeleteUserList(ids);
                return response.Success;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userInfo"></param>
        public bool UpdateUser(UserInfoModel userInfo)
        {
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.ID))
            {
                var response = SysManageApi.Client.UpdateUser(userInfo);
                return response.Success;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool ResetPassword(UserInfoModel userInfo)
        {
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.ID))
            {
                var response = SysManageApi.Client.ResetPassword(userInfo.ID);
                return response.Success;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userInfo">用户实体</param>
        /// <param name="isLock">true锁定、fasle解锁</param>
        /// <returns></returns>
        public bool LockUser(UserInfoModel userInfo, bool isLock)
        {
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.ID))
            {
                var response = SysManageApi.Client.LockUser(isLock, userInfo.ID);
                return response.Success;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新用户密码，主要修改newpassword 和 oldpassword字段
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool UpdatePassword(UserInfoModel userInfo)
        {
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.ID))
            {
                var response = SysManageApi.Client.UpdatePassword(userInfo);
                return response.Success;
            }
            else
            {
                return false;
            }

        }
        #endregion 用户

        #region 角色
        /// <summary>
        /// 通过条件查询对应角色树
        /// </summary>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        public List<RoleInfoModel> GetRoleListTree(RoleInfoModel roleInfo)
        {
            var result = new List<RoleInfoModel>();
            if (roleInfo != null)
            {
                result = SysManageApi.Client.GetRoleListTree(roleInfo.RoleName, roleInfo.RoleAlias).Data;
            }
            return result;
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleInfo">角色实体</param>
        /// <returns></returns>
        public bool RemoveRole(RoleInfoModel roleInfo)
        {
            if (roleInfo != null && !string.IsNullOrEmpty(roleInfo.ID))
            {
                var response = SysManageApi.Client.RemoveRole(roleInfo.ID);
                return response.Success;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新角色(id 不存在：新增、id 存在：修改)
        /// </summary>
        /// <param name="roleInfo">角色实体</param>
        /// <returns></returns>
        public bool SaveRole(RoleInfoModel roleInfo)
        {
            if (roleInfo != null)
            {
                var response = SysManageApi.Client.SaveRole(roleInfo);
                return response.Success;
            }
            else
            {
                return false;
            }
        }

        #endregion 角色

        #region 菜单
        public MenuDetailModel GetMenuDetailsbyId(string id)
        {
            return SysManageApi.Client.GetMenuDetailsbyId(id).Data;
        }
        #endregion 菜单

        #region 角色菜单
        /// <summary>
        /// 获取登录用户 的 C端权限
        /// </summary>
        /// <returns></returns>
        public List<MenuInfoModel> GetCSLoginUserMenus()
        {
            var result = new List<MenuInfoModel>();
            var response = SysManageApi.Client.GetCSLoginUserMenus();
            if (response.Success)
            {
                result = response.Data;
            }
            return result;
        }
        /// <summary>
        /// 根据角色id获取对应菜单id集合
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns></returns>
        public List<string> GetMenusbyRoleId(string roleId)
        {
            var result = new List<string>();
            if (!string.IsNullOrEmpty(roleId))
            {
                result = SysManageApi.Client.GetMenusbyRoleId(roleId).Data;
            }
            return result;
        }

        /// <summary>
        /// 获取该用户下所有的菜单
        /// </summary>
        /// <returns></returns>
        public List<MenuInfoModel> GetCustomMenus()
        {
            var result = new List<MenuInfoModel>();
            var resopnse = SysManageApi.Client.GetCustomMenus();
            if (resopnse.Success)
            {
                result = resopnse.Data;
            }
            return result;
        }
        /// <summary>
        /// 更新角色对应的菜单权限
        /// </summary>
        /// <param name="setting">中台需要的角色菜单实体</param>
        /// <returns></returns>
        public bool UpdateRoleMenus(QueryMenusSetting setting)
        {
            if (setting != null && !string.IsNullOrEmpty(setting.ID))
            {
                return SysManageApi.Client.UpdateMenusofRole(setting).Success;
            }
            else
            {
                return false;
            }
        }
        #endregion 角色菜单

        #region 部门
        /// <summary>
        /// 查询部门信息
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <returns></returns>
        public List<DeptInfoModel> GetDeptTrees(string deptName)
        {
            var result = new List<DeptInfoModel>();
            var response = SysManageApi.Client.GetDeptTrees(deptName);
            if (response.Success)
            {
                result = response.Data;
            }
            return result;
        }
        /// <summary>
        /// 获取莫部门详细信息
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns></returns>
        public DeptDetailModel GetDeptDetail(string id)
        {
            DeptDetailModel result = null;
            if (!string.IsNullOrEmpty(id))
            {
                var response = SysManageApi.Client.GetDeptDetail(id);
                if (response.Success)
                {
                    result = response.Data;
                }
            }
            return result;
        }
        /// <summary>
        /// 新增部门
        /// </summary>
        /// <param name="queryDept">部门实体</param>
        /// <returns></returns>
        public bool AddDeptInfo(DeptDetailModel deptModel)
        {
            if (deptModel != null)
            {
                var response = SysManageApi.Client.AddDeptInfo(deptModel);
                return response.Success;
            }
            return false;
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="queryDept">部门实体</param>
        /// <returns></returns>
        public bool UpdateDeptInfo(DeptDetailModel deptModel)
        {
            if (deptModel != null)
            {
                var response = SysManageApi.Client.UpdateDeptInfo(deptModel);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="deptID">部门id</param>
        /// <returns></returns>
        public bool RemoveDeptbyID(string deptID)
        {
            if (!string.IsNullOrEmpty(deptID))
            {
                var response = SysManageApi.Client.RemoveDeptbyID(deptID);
                return response.Success;
            }
            return false;
        }
        #endregion 部门

        #region 医生
        /// <summary>
        /// 分页获取医生列表
        /// </summary>
        /// <param name="doctorInfo">查询实体</param>
        /// <returns></returns>
        public List<DoctorInfoModel> GetDocList(CustomPageModel pageModel, DoctorInfoModel doctorInfo)
        {
            var result = new List<DoctorInfoModel>();
            if (doctorInfo != null)
            {
                var response = SysManageApi.Client.GetDocListbyPage(pageModel.PageSize, pageModel.PageIndex, doctorInfo);
                if (response.Success)
                {
                    pageModel.TotalNum = response.Data.Total;
                    pageModel.TotalPage = response.Data.Pages;
                    result = response.Data.Records;
                }
            }
            return result;
        }
        /// <summary>
        /// 新增医生
        /// </summary>
        /// <param name="doctorInfo">医生实体</param>
        /// <returns></returns>
        public bool AddDoctor(DoctorInfoModel doctorInfo)
        {
            if (doctorInfo != null)
            {
                var response = SysManageApi.Client.AddDoctor(doctorInfo);
                return response.Success;
            }
            return false;
        }

        /// <summary>
        /// 更新医生
        /// </summary>
        /// <param name="doctorInfo">部门实体</param>
        /// <returns></returns>
        public bool UpdateDoctor(DoctorInfoModel doctorInfo)
        {
            if (doctorInfo != null)
            {
                var response = SysManageApi.Client.UpdateDoctor(doctorInfo);
                return response.Success;
            }
            return false;
        }
        /// <summary>
        /// 删除医生
        /// </summary>
        /// <param name="doctorInfo">医生id</param>
        /// <returns></returns>
        public bool DeleteDoctor(DoctorInfoModel doctorInfo)
        {
            if (doctorInfo != null && !string.IsNullOrEmpty(doctorInfo.ID))
            {
                var response = SysManageApi.Client.DeleteDoctor(doctorInfo.ID);
                return response.Success;
            }
            return false;
        }
        #endregion 医生
    }
}
