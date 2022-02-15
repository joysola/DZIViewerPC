using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class SysManaRoleViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 角色树
        /// </summary>
        [Notification]
        public List<RoleInfoModel> RoleInfoList { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        [Notification]
        public RoleInfoModel QueryRole { get; set; } = new RoleInfoModel { RoleName = string.Empty, RoleAlias = string.Empty };
        /// <summary>
        /// 污垢的查询条件，用于获取所有角色
        /// </summary>
        private RoleInfoModel QueryOriginRole { get; set; } = new RoleInfoModel { RoleName = string.Empty, RoleAlias = string.Empty };

        /// <summary>
        /// 角色列表集合
        /// </summary>
        public List<RoleInfoModel> AllRoleList { get; set; } = new List<RoleInfoModel>();
        /// <summary>
        /// 菜单树
        /// </summary>
        private List<MenuInfoModel> AllMenuTrees { get; set; } = new List<MenuInfoModel>();
        /// <summary>
        /// 所有菜单集合
        /// </summary>
        private List<MenuInfoModel> AllMenus { get; set; } = new List<MenuInfoModel>();
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                RoleInfoList = SysManageService.Instance.GetRoleListTree(QueryRole);
                UpdateCommand?.Execute(null);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }

        })).Value;
        /// <summary>
        /// 新增
        /// </summary>
        public ICommand AddCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            AddorEditRoleInfo(new RoleInfoModel(), "新增");
        })).Value;
        /// <summary>
        /// 编辑
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand<RoleInfoModel>>(() => new RelayCommand<RoleInfoModel>(data =>
        {
            if (data != null)
            {
                AddorEditRoleInfo(data.DeepCopy(), "编辑");
            }
        })).Value;
        /// <summary>
        /// 删除
        /// </summary>
        public ICommand RemoveCommand => new Lazy<RelayCommand<RoleInfoModel>>(() => new RelayCommand<RoleInfoModel>(data =>
        {

            if (data == null)
            {
                return;
            }
            ShowMessageBox("该操作即将删除用户，请确认？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = SysManageService.Instance.RemoveRole(data);
                        if (result)
                        {
                            this.QueryCommand.Execute(null);
                        }
                        else
                        {
                            ShowMessageBox($"删除角色信息失败！", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// 角色分配菜单权限
        /// </summary>
        public ICommand MenusSetCommand => new Lazy<RelayCommand<RoleInfoModel>>(() => new RelayCommand<RoleInfoModel>(data =>
        {
            if (data != null)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var menusList = SysManageService.Instance.GetMenusbyRoleId(data.ID);
                    var msg = new ShowContentWindowMessage("SMR_Menus", "权限配置");
                    msg.DesignWidth = 400;
                    msg.DesignHeight = 400;
                    msg.Args = new object[] { menusList, AllMenuTrees, data };
                    msg.CallBackCommand = new RelayCommand<bool>(res =>
                    {
                        if (res) // 操作成功
                        {
                            this.QueryCommand.Execute(null);
                        }
                    });
                    Messenger.Default.Send(msg);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }

        })).Value;
        /// <summary>
        /// 初始化命令
        /// </summary>
        public ICommand InitCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            // 查询角色信息
            var queryTask = Task.Run(() =>
            {
                this.QueryCommand.Execute(null);
                GetAllRoles(RoleInfoList);
            });
            // 查询所有菜单
            var MenusTask = Task.Run(() =>
            {
                AllMenuTrees = SysManageService.Instance.GetCustomMenus();
                GetAllMenus(AllMenuTrees);
            });
            await Task.WhenAll(queryTask, MenusTask).ConfigureAwait(false);
        })).Value;
        /// <summary>
        /// 触发其他viewmodel的更改
        /// </summary>
        public ICommand UpdateCommand { get; set; }

        public SysManaRoleViewModel()
        {
            InitCommand.Execute(null);
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
        /// 从树形结构获取所有菜单集合
        /// </summary>
        /// <param name="menuTree"></param>
        private void GetAllMenus(List<MenuInfoModel> menuTree)
        {
            foreach (var menu in menuTree)
            {
                AllMenus.Add(menu);
                if (menu.Children != null && menu.Children.Count > 0)
                {
                    GetAllMenus(menu.Children);
                }
            }
        }
        /// <summary>
        /// 新增或删除弹窗
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <param name="title">标题</param>
        private void AddorEditRoleInfo(RoleInfoModel role, string title)
        {
            var msg = new ShowContentWindowMessage("SMR_Edit", title);
            msg.DesignWidth = 400;
            msg.DesignHeight = 400;
            msg.Args = new object[] { role, RoleInfoList, AllRoleList };
            msg.CallBackCommand = new RelayCommand<RoleInfoModel>(res =>
            {
                if (res != null)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        var result = SysManageService.Instance.SaveRole(res);
                        if (result)
                        {
                            this.QueryCommand.Execute(null);
                            // 更新所有角色列表
                            AllRoleList.Clear();
                            //var tmp = SysManageService.Instance.GetRoleListTree(QueryOriginRole);
                            GetAllRoles(RoleInfoList);
                        }
                        else
                        {
                            ShowMessageBox($"{title}角色信息失败！", MessageBoxButton.OK, MessageBoxImage.Error);
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
        public void QueryPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryCommand.Execute(null);
            }
        }
    }
}
