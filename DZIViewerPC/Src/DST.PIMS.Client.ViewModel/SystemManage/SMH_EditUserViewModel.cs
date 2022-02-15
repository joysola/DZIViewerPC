using DST.ApiClient.Service;
using DST.Controls.Base;
using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class SMH_EditUserViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        [Notification]
        public UserInfoModel UserInfo { get; set; }
        /// <summary>
        /// 所有部门集合
        /// </summary>
        [Notification]
        public ObservableCollection<DeptInfoModel> AllDeptList { get; set; } = new ObservableCollection<DeptInfoModel>();
        /// <summary>
        /// 所有角色集合
        /// </summary>
        [Notification]
        public ObservableCollection<RoleInfoModel> AllRoleList { get; set; } = new ObservableCollection<RoleInfoModel>();
        /// <summary>
        /// 部门树
        /// </summary>
        private List<DeptInfoModel> DeptListTree { get; set; }
        /// <summary>
        /// 角色树
        /// </summary>
        private List<RoleInfoModel> RoleListTree { get; set; }
        /// <summary>
        /// 编辑前选中的部门
        /// </summary>
        private DeptInfoModel SelectedDept { get; set; }

        public ICommand EditRoleCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var msg = new ShowContentWindowMessage("SMR_Roles", "角色列表");
            msg.DesignWidth = 400;
            msg.DesignHeight = 500;
            msg.Args = new object[] { RoleListTree };
            msg.CallBackCommand = new RelayCommand<RoleInfoModel>(res =>
            {
                if (UserInfo != null && res is RoleInfoModel)
                {
                    if (!UserInfo.RoleId.Contains(res.ID)) // 已存在的不需要再增加
                    {
                        UserInfo.Roles.Add(res); // 将选择的父级菜单ID赋值给当前菜单的ParentID
                    }
                }
            });
            Messenger.Default.Send(msg);
        })).Value;
        /// <summary>
        /// 编辑
        /// </summary>
        public ICommand EditDeptCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var msg = new ShowContentWindowMessage("SMH_Depts", "角色列表");
            msg.DesignWidth = 400;
            msg.DesignHeight = 500;
            msg.Args = new object[] { DeptListTree };
            msg.CallBackCommand = new RelayCommand<DeptInfoModel>(res =>
            {
                if (UserInfo != null && res is DeptInfoModel)
                {
                    // 部门即科室，只能设置一个
                    UserInfo.Depts.Clear();
                    UserInfo.Depts.Add(res);
                }
            });
            Messenger.Default.Send(msg);
        })).Value;
        /// <summary>
        /// 取消
        /// </summary>
        public ICommand CancelCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.CloseContentWindow();
        })).Value;
        /// <summary>
        /// 确认
        /// </summary>
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (IsCheckedOK)
            {
                this.Result = UserInfo;
                this.CloseContentWindow();
            }
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 4
                && this.Args[0] is UserInfoModel userInfo
                && this.Args[1] is List<DeptInfoModel> deptList
                && this.Args[2] is List<RoleInfoModel> roleList)
            {
                this.UserInfo = userInfo;
                this.SelectedDept = this.Args[3] as DeptInfoModel;
                this.RoleListTree = roleList;
                this.DeptListTree = deptList;
                GetAllRoles(RoleListTree);
                GetAllDepts(DeptListTree);

            }
        }

        /// <summary>
        /// 从树形结构获取全部角色集合
        /// </summary>
        /// <param name="roleTree"></param>
        private void GetAllRoles(List<RoleInfoModel> roleTree)
        {
            foreach (var role in roleTree)
            {
                AllRoleList.Add(role);
                if (role.Children != null && role.Children.Count > 0)
                {
                    GetAllRoles(role.Children);
                }
            }
        }
        /// <summary>
        /// 从树形结构获取全部部门集合
        /// </summary>
        /// <param name="deptTree"></param>
        private void GetAllDepts(List<DeptInfoModel> deptTree)
        {
            foreach (var role in deptTree)
            {
                AllDeptList.Add(role);
                if (role.Children != null && role.Children.Count > 0)
                {
                    GetAllDepts(role.Children);
                }
            }
        }
    }
}
