using DST.PIMS.Framework.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Framework.Extensions
{
    /// <summary>
    /// 申请单权限扩展
    /// </summary>
    public static class AppFrmPermissionExtension
    {
        /// <summary>
        /// 设置申请单某个expander的权限
        /// </summary>
        /// <param name="permissions">权限集合</param>
        /// <param name="name">申请单expander的名称</param>
        /// <param name="visible">可见性</param>
        /// <param name="isEnable">可用性</param>
        /// <param name="isExpanded">折叠性</param>
        /// <returns>设置失败返回false，成功返回true</returns>
        public static bool SetPermission(this IEnumerable<AppFrmPermission> permissions, string name, Visibility visible = Visibility.Visible, bool isEnable = true, bool isExpanded = true)
        {
            var result = false;
            var perm = permissions?.FirstOrDefault(x => x.Name == name);
            if (perm != null)
            {
                perm.Visible = visible;
                perm.IsEnable = isEnable;
                perm.IsExpanded = isExpanded;
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 设置申请单是否可以编辑
        /// </summary>
        /// <param name="permissions">权限集合</param>
        /// <param name="isEdit">是否可以编辑</param>
        public static void SetPermissionIsEdit(this IEnumerable<AppFrmPermission> permissions, bool isEdit)
        {
            foreach (var afp in permissions)
            {
                afp.IsEnable = isEdit;
            }
        }
        /// <summary>
        /// 获取header对应的申请单的expander权限
        /// </summary>
        /// <param name="permissions">权限集合</param>
        /// <param name="name">expander的名称</param>
        /// <returns></returns>
        public static AppFrmPermission GetPermission(this IEnumerable<AppFrmPermission> permissions, string name)
        {
            var result = permissions?.FirstOrDefault(x => x.Name == name);
            return result;
        }
    }
}
