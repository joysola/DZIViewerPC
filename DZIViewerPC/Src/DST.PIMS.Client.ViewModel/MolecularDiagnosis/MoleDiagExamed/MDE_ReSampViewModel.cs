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
    public class MDE_ReSampViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 重新取样提交实体
        /// </summary>
        [Notification]
        public SubmitReSampleModel SubmitReSample { get; set; } = new SubmitReSampleModel();
        /// <summary>
        /// 重新取样原因字典
        /// </summary>
        public List<DictItem> ReSampReasonDict => ExtendApiDict.Instance.ReSampReasonDict;
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
                    var result = ReSampService.Instance.SubmitReSample(SubmitReSample);
                    if (result)
                    {
                        this.Result = true;
                        this.CloseContentWindow();
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
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] != null)
            {
                SubmitReSample.SampleID = this.Args[0].ToString();
            }
        }
    }
}
