using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.Extensions;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class AppFrmMaintainViewModel : CustomBaseViewModel
    {
        public AppFrmViewModel AppViewModel { get; } = new AppFrmViewModel();
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
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (AppViewModel.CheckSaveData())
                {
                    var result = ApplyFormService.Instance.SavePathInfo(AppViewModel.AppModel);
                    if (result.Count > 0)
                    {
                        ShowMessageBox("保存成功！", MessageBoxButton.OK, MessageBoxImage.Information, null, true);
                        this.Result = true;
                        this.CloseContentWindow();
                    }
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
            this.CloseContentWindow();
        })).Value;


        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args[0] is string code)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = ApplyFormService.Instance.GetPathInfobyCode(code) ?? new ApplyFrmModel();
                    AppViewModel.AppModel = result;
                    AppViewModel.Permissions.SetPermissionIsEdit(false);
                    AppViewModel.Permissions.SetPermission("临床诊断相关", Visibility.Visible, true); // 开发临床诊断的编辑功能
                    //AppViewModel.Permissions.SetPermission("检查项目信息", Visibility.Collapsed);
                    //AppViewModel.Permissions.SetPermission("标本信息", Visibility.Collapsed);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        }
    }
}
