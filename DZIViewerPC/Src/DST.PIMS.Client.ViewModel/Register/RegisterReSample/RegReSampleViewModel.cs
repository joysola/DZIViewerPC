using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
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
    public class RegReSampleViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        [Notification]
        public QueryReSample QueryModel { get; set; } = new QueryReSample();
        /// <summary>
        /// 重新取样列表
        /// </summary>
        [Notification]
        public List<ReSampModel> ReSampList { get; set; } = new List<ReSampModel>();
        /// <summary>
        /// 重新取样字典
        /// </summary>
        public List<DictItem> ReSampStatusDict => ExtendApiDict.Instance.ReSampStatusDict;
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            PageModel.PageIndex = 1;
            LoadData();
        })).Value;

        /// <summary>
        /// 关联
        /// </summary>
        public ICommand RelateCommand => new Lazy<RelayCommand<ReSampModel>>(() => new RelayCommand<ReSampModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                var msg = new ShowContentWindowMessage("RegReSamp_Relate", "重新取样");
                msg.DesignWidth = 400;
                msg.DesignHeight = 300;
                msg.Args = new object[] { data };
                msg.CallBackCommand = new RelayCommand<bool>(res =>
                {
                    if (res)
                    {
                        this.QueryCommand.Execute(null);
                    }
                });
                Messenger.Default.Send(msg);
            }
        })).Value;
        /// <summary>
        /// 撤销
        /// </summary>
        public ICommand WithdrawCommand => new Lazy<RelayCommand<ReSampModel>>(() => new RelayCommand<ReSampModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                ShowMessageBox("是否需要撤销该重新取样样本？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            var result = ReSampService.Instance.WithdrawReSamp(data);
                            if (result)
                            {
                                this.QueryCommand.Execute(null);
                            }
                            else
                            {
                                ShowMessageBox("撤销失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        finally
                        {
                            WhirlingControlManager.CloseWaitingForm();
                        }
                    }
                });
            }

            //    var msg = new ShowContentWindowMessage("RegReSamp_Withdraw", "撤销");
            //    msg.DesignWidth = 400;
            //    msg.DesignHeight = 300;
            //    msg.Args = new object[] { data };
            //    msg.CallBackCommand = new RelayCommand<bool>(res =>
            //    {
            //        if (res)
            //        {
            //            this.QueryCommand.Execute(null);
            //        }
            //    });
            //    Messenger.Default.Send(msg);
            //}
        })).Value;

        public RegReSampleViewModel()
        {
            this.QueryCommand.Execute(null);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        public override void LoadData()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                ReSampList = ReSampService.Instance.GetReSampList(PageModel, QueryModel);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
    }
}
