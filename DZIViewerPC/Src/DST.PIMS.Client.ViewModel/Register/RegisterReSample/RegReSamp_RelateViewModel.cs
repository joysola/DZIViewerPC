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
    public class RegReSamp_RelateViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 重新取样实体
        /// </summary>
        [Notification]
        public ReSampModel ReSamp { get; set; }
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
            if (IsCheckedOK && !string.IsNullOrEmpty(ReSamp?.ReSampleID))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = ReSampService.Instance.RelateReSample(ReSamp);
                    if (result)
                    {
                        this.Result = true;
                        this.CloseContentWindow();
                    }
                    else
                    {
                        ShowMessageBox("重新取样关联失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
              
            }
        })).Value;
        /// <summary>
        /// 选择重新取样样本
        /// </summary>
        public ICommand ChooseReSampCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(ReSamp?.ID) && !string.IsNullOrEmpty(ReSamp?.SampleID))
            {
                var msg = new ShowContentWindowMessage("RegReSamp_Query", "样本查询");
                msg.DesignWidth = 800;
                msg.DesignHeight = 650;
                msg.Args = new object[] { ReSamp };
                msg.CallBackCommand = new RelayCommand<SampleModel>(res =>
                {
                    if (res != null && !string.IsNullOrEmpty(res.SampleID) && !string.IsNullOrEmpty(res.LabCode))
                    {
                        ReSamp.ReSampleID = res.SampleID;
                        ReSamp.ReLabCode = res.LabCode;
                    }
                });
                Messenger.Default.Send(msg);
            }
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is ReSampModel model)
            {
                ReSamp = model;
            }
        }
    }
}
