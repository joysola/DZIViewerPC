using DST.ApiClient.Service;
using DST.Controls;
using DST.Database;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
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
    public class ModifyPasswordViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 用户实体
        /// </summary>
        [Notification]
        public UserInfoModel User { get; set; } = new UserInfoModel();
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
                if (User.CheckPassword != User.NewPassword)
                {
                    ShowMessageBox("输入的新密码不一致！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = SysManageService.Instance.UpdatePassword(User);
                    if (result)
                    {
                        this.Result = true;
                        this.CloseContentWindow();
                    }
                    else
                    {
                        ShowMessageBox("修改密码失败！", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }

                //this.Result = ComWord;
            }
        })).Value;

        public override void OnViewLoaded()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                var result = SysManageService.Instance.GetUserDetailbyId(ExtendAppContext.Current.LoginModel.UserId); // 查询登录用户的 用户明细数据
                if (result != null)
                {
                    User = result;
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
    }
}
