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
    public class SMR_EditViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 角色信息
        /// </summary>
        [Notification]
        public RoleInfoModel RoleInfo { get; set; }
        /// <summary>
        /// 树菜单
        /// </summary>
        private List<RoleInfoModel> RoleInfoList { get; set; }
        /// <summary>
        /// 全部菜单
        /// </summary>
        [Notification]
        public ObservableCollection<RoleInfoModel> AllRoleList { get; set; } = new ObservableCollection<RoleInfoModel>();

        /// <summary>
        /// 编辑
        /// </summary>
        public ICommand EditParentRole => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var msg = new ShowContentWindowMessage("SMR_Roles", "角色列表");
            msg.DesignWidth = 400;
            msg.DesignHeight = 500;
            msg.Args = new object[] { RoleInfoList };
            msg.CallBackCommand = new RelayCommand<RoleInfoModel>(res =>
            {
                if (RoleInfo != null && res is RoleInfoModel)
                {
                    if (res.ID == RoleInfo.ID) // id ！= parentid
                    {
                        ShowMessageBox("不可已将上级角色设置为此角色自身！", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    RoleInfo.ParentID = res.ID; // 将选择的父级菜单ID赋值给当前菜单的ParentID
                }
            });
            Messenger.Default.Send(msg);
        })).Value;
        /// <summary>
        /// 清理parentid
        /// </summary>
        public ICommand ClearParentCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (RoleInfo != null)
            {
                RoleInfo.ParentID = null;
            }
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
            this.Result = RoleInfo;
            this.CloseContentWindow();
        })).Value;


        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 3 && this.Args[0] is RoleInfoModel role && this.Args[1] is List<RoleInfoModel> roleList && this.Args[2] is List<RoleInfoModel> allRoles)
            {
                RoleInfo = role;
                RoleInfoList = roleList;
                AllRoleList = new ObservableCollection<RoleInfoModel>(allRoles);
            }
        }

    }
}
