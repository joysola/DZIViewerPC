using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
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
    public class Material_SpecEditViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 样本部位实体
        /// </summary>
        [Notification]
        public SampSpecDetail SSDetai { get; set; }
        /// <summary>
        /// 是否可以编辑
        /// </summary>
        [Notification]
        public bool IsEnabled { get; set; } = true;
        /// <summary>
        /// 新增送检部位
        /// </summary>
        public ICommand AddSpecCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var newSpecItem = new InspSpecimen
            {
                SampleId = SSDetai.SampleId,
                Number = 1,
            };
            SSDetai.SampSpecList.Add(newSpecItem);
        })).Value;

        /// <summary>
        /// 删除送检部位
        /// </summary>
        public ICommand DeleteSpecCommand => new Lazy<RelayCommand<InspSpecimen>>(() => new RelayCommand<InspSpecimen>(data =>
        {
            ShowMessageBox("删除取材部位时，关联的蜡块也将删除，是否继续？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    SSDetai?.SampSpecList?.Remove(data);
                }
            });

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
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            WhirlingControlManager.ShowWaitingForm();
            try
            {
                if (!string.IsNullOrEmpty(SSDetai?.SampleId) && IsCheckedOK)
                {
                    var result = await Task.Run(() => MaterialService.Instance.SaveSampSpecList(SSDetai));
                    this.Result = result;
                    this.CloseContentWindow();
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }

        })).Value;
        /// <summary>
        /// 样本常用词
        /// </summary>
        public ICommand SampleNameCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            ShowComWord(DSTCode.SampleStr, "标本名称常用词选择", new RelayCommand<ComWordModel>(res =>
            {
                if (res != null)
                {
                    this.SSDetai.InspecSampleName = res.Content;
                }
            }));

        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is SampSpecDetail ssd)
            {
                SSDetai = ssd;
                // 1. 胃镜特殊处理 2. 样本状态是已经确认的后，无法操作
                if (SSDetai?.ProductID == DSTCode.GastroscopeProdID || SSDetai?.SampStatus == "1")
                {
                    IsEnabled = false;
                }
            }
        }


        /// <summary>
        /// 显示常用词
        /// </summary>
        /// <param name="type"></param>
        /// <param name="title"></param>
        private void ShowComWord(string type, string title, ICommand command)
        {
            var msg = new ShowContentWindowMessage("ComWordDict", title);
            msg.DesignWidth = 600;
            msg.DesignHeight = 400;
            msg.Args = new object[] { type };
            msg.CallBackCommand = command;
            Messenger.Default.Send(msg);
        }
    }
}
