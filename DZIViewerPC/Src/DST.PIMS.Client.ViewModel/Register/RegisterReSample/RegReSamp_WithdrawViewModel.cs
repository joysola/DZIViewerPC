using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.Database.Model.DictModel;
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
    public class RegReSamp_WithdrawViewModel : CustomBaseViewModel
    {
        [Notification]
        public ReSampModel ReSamp { get; set; } = new ReSampModel();
        /// <summary>
        /// 重新取样撤销原因字典
        /// </summary>
        public List<DictItem> ReSampWithdrawReasonDict => ExtendApiDict.Instance.ReSampWithdrawReasonDict;

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
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = ReSampService.Instance.WithdrawReSamp(ReSamp);
                    if (result)
                    {
                        this.Result = true;
                        this.CloseContentWindow();
                    }
                    else
                    {
                        ShowMessageBox("撤销重新取样失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args!=null && this.Args!=null && this.Args[0] is ReSampModel model)
            {
                ReSamp = model;
            }
        }
    }
}
