using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class SysManaHospViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 部门查询条件
        /// </summary>
        [Notification]
        public string QueryDeptName { get; set; } = string.Empty;
        /// <summary>
        /// 查询的部门列表树
        /// </summary>
        [Notification]
        public List<DeptInfoModel> DeptList { get; set; }
        /// <summary>
        /// 所有部门列表
        /// </summary>
        private List<DeptInfoModel> DeptListTree { get; set; }
        /// <summary>
        /// 所有角色列表
        /// </summary>
        private List<RoleInfoModel> RoleListTree { get; set; }
        /// <summary>
        /// 选中的部门
        /// </summary>
        private DeptInfoModel SelectedDept { get; set; }
        /// <summary>
        /// 查询的用户列表
        /// </summary>
        [Notification]
        public List<UserInfoModel> UserInfoList { get; set; } = new List<UserInfoModel>();
        /// <summary>
        /// 查询实体
        /// </summary>
        [Notification]
        public UserInfoModel QueryUserModel { get; set; } = new UserInfoModel { UserName = "", RealName = "" };

        #region 部门Command
        /// <summary>
        /// 查询部门
        /// </summary>
        public ICommand QueryDeptCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                DeptList = SysManageService.Instance.GetDeptTrees(QueryDeptName);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        })).Value;
        /// <summary>
        /// 新增部门
        /// </summary>
        public ICommand AddDeptCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            AddorEditDeptInfo(new DeptDetailModel(), "新增", true);
        })).Value;
        /// <summary>
        /// 删除部门
        /// </summary>
        public ICommand DeleteDeptCommand => new Lazy<RelayCommand<DeptInfoModel>>(() => new RelayCommand<DeptInfoModel>(data =>
        {
            if (data != null && !string.IsNullOrEmpty(data.ID))
            {
                ShowMessageBox("请确认是否删除部门？", MessageBoxButton.OKCancel, MessageBoxImage.Warning, res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            var result = SysManageService.Instance.RemoveDeptbyID(data.ID);
                            if (!result)
                            {
                                ShowMessageBox("删除失败！", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            this.QueryDeptCommand.Execute(null);
                            DeptListTree = SysManageService.Instance.GetDeptTrees(string.Empty); // 更新所有部门列表
                        }
                        finally
                        {
                            WhirlingControlManager.CloseWaitingForm();
                        }

                    }
                });
            }
        })).Value;
        /// <summary>
        /// 编辑部门
        /// </summary>
        public ICommand EditDeptCommand => new Lazy<RelayCommand<DeptInfoModel>>(() => new RelayCommand<DeptInfoModel>(data =>
        {
            if (data != null)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = SysManageService.Instance.GetDeptDetail(data.ID);
                    AddorEditDeptInfo(result, "编辑", false);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;
        /// <summary>
        /// 选中项
        /// </summary>
        public ICommand SelectDeptCommand => new Lazy<RelayCommand<DeptInfoModel>>(() => new RelayCommand<DeptInfoModel>(data =>
        {
            SelectedDept = data;
            var deptID = SelectedDept?.ID ?? string.Empty; // 部门id
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                UserInfoList = SysManageService.Instance.GetUserListbyPage(PageModel, QueryUserModel, deptID);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        })).Value;
        #endregion 部门Command

        #region 用户Command
        /// <summary>
        /// 查询用户信息
        /// </summary>
        public ICommand QueryUserCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.PageModel.PageIndex = 1;
            LoadData();
        })).Value;
        /// <summary>
        /// 新增用户信息
        /// </summary>
        public ICommand AddUserCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var newUser = new UserInfoModel();
            newUser.Depts.Add(SelectedDept); // 将选中的科室加入用户信息里
            AddorEditUserInfo(newUser, "新增", true);

        })).Value;

        /// <summary>
        /// 更新单个用户
        /// </summary>
        public ICommand EditUserCommand => new Lazy<RelayCommand<UserInfoModel>>(() => new RelayCommand<UserInfoModel>(data =>
        {
            AddorEditUserInfo(data, "编辑", false);
        })).Value;

        /// <summary>
        /// 删除单个用户
        /// </summary>
        public ICommand DeleteUserCommand => new Lazy<RelayCommand<UserInfoModel>>(() => new RelayCommand<UserInfoModel>(data =>
        {
            ShowMessageBox("请确认是否删除用户信息？", MessageBoxButton.OKCancel, MessageBoxImage.Warning, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = SysManageService.Instance.DeleteUser(data);
                        if (!result)
                        {
                            ShowMessageBox("删除失败！", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        this.QueryUserCommand.Execute(null);
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
        })).Value;


        /// <summary>
        /// 重置密码
        /// </summary>
        public ICommand ResetPwCommand => new Lazy<RelayCommand<UserInfoModel>>(() => new RelayCommand<UserInfoModel>(data =>
        {
            ShowMessageBox("此操作将重置当前选中用户密码, 是否继续?", MessageBoxButton.OKCancel, MessageBoxImage.Warning, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = SysManageService.Instance.ResetPassword(data);
                        if (!result)
                        {
                            ShowMessageBox("重置密码失败！", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
        })).Value;
        /// <summary>
        /// 锁定用户账户
        /// </summary>
        public ICommand LockUserCommand => new Lazy<RelayCommand<UserInfoModel>>(() => new RelayCommand<UserInfoModel>(data =>
        {
            ShowMessageBox("此操作将修改当前用户状态，是否继续？", MessageBoxButton.OKCancel, MessageBoxImage.Warning, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var isLock = data.Status == 0 ? false : true;
                        var result = SysManageService.Instance.LockUser(data, isLock);
                        if (!result)
                        {
                            ShowMessageBox("修改状态失败！", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        this.QueryUserCommand.Execute(null);
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
        })).Value;

        #endregion 用户Command
        /// <summary>
        /// 初始化命令
        /// </summary>
        public ICommand InitCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var roleTask = Task.Run(() =>
                {
                    RoleListTree = SysManageService.Instance.GetRoleListTree(new RoleInfoModel { RoleName = string.Empty, RoleAlias = string.Empty });
                });
                var deptTask = Task.Run(() =>
                {
                    this.QueryDeptCommand.Execute(null);
                    DeptListTree = DeptList;
                });
                var userTask = Task.Run(() =>
                {
                    this.QueryUserCommand.Execute(null);
                });
                await Task.WhenAll(roleTask, deptTask, userTask).ConfigureAwait(false);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        })).Value;

        public SysManaHospViewModel()
        {
            //this.Init().ConfigureAwait(false).GetAwaiter().GetResult();
            InitCommand.Execute(null);
        }
        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        public override void LoadData()
        {
            WhirlingControlManager.ShowWaitingForm();
            try
            {
                var deptID = SelectedDept?.ID ?? string.Empty; // 部门id
                UserInfoList = SysManageService.Instance.GetUserListbyPage(PageModel, QueryUserModel, deptID);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
        /// <summary>
        /// 新增或删除弹窗
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <param name="title">标题</param>
        private void AddorEditDeptInfo(DeptDetailModel deptDetail, string title, bool isAdd)
        {
            var msg = new ShowContentWindowMessage("SMH_Edit", title);
            msg.DesignWidth = 400;
            msg.DesignHeight = 250;
            msg.Args = new object[] { deptDetail, DeptListTree };
            msg.CallBackCommand = new RelayCommand<DeptDetailModel>(res =>
            {
                if (res != null)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = false;
                        if (isAdd)
                        {
                            result = SysManageService.Instance.AddDeptInfo(res);
                        }
                        else
                        {
                            result = SysManageService.Instance.UpdateDeptInfo(res);
                        }

                        if (result)
                        {
                            this.QueryDeptCommand.Execute(null);
                            DeptListTree = SysManageService.Instance.GetDeptTrees(string.Empty); // 更新所有部门列表
                        }
                        else
                        {
                            ShowMessageBox($"{title}部门信息失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
            Messenger.Default.Send(msg);
        }


        /// <summary>
        /// 新增或删除弹窗
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <param name="title">标题</param>
        private void AddorEditUserInfo(UserInfoModel userInfo, string title, bool isAdd)
        {
            var msg = new ShowContentWindowMessage("SMH_EditUser", title);
            msg.DesignWidth = 400;
            msg.DesignHeight = 550;
            msg.Args = new object[] { userInfo, DeptListTree, RoleListTree, SelectedDept };
            msg.CallBackCommand = new RelayCommand<UserInfoModel>(res =>
            {
                if (res != null)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = false;
                        if (isAdd)
                        {
                            result = SysManageService.Instance.AddUser(res);
                        }
                        else
                        {
                            result = SysManageService.Instance.UpdateUser(res);
                        }

                        if (result)
                        {
                            this.QueryUserCommand.Execute(null);
                        }
                        else
                        {
                            ShowMessageBox($"{title}用户信息失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 回车搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void QueryDeptPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryDeptCommand.Execute(null);
            }
        }
        /// <summary>
        /// 回车搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void QueryUserPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryUserCommand.Execute(null);
            }
        }
    }
}
