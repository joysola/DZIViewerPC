using DST.ApiClient.Service;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Nico.Common;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ScanCellViewModel : CustomBaseViewModel
    {
        private bool selectAll = false;

        /// <summary>
        /// 全选
        /// </summary>
        public bool SelectAll
        {
            get { return this.selectAll; }
            set
            {
                this.selectAll = value;
                this.RaisePropertyChanged("SelectAll");
                this.CurScanReturn.records.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        [Notification]
        public ScanQueryModel CurScanQuery { get; set; } = new ScanQueryModel() { cellType = "2"};

        [Notification]
        public ScanReturnModel CurScanReturn { get; set; } = new ScanReturnModel();

        public ICommand QueryCommand { get; set; }

        public ICommand BatchReceiveCommand { get; set; }

        public ICommand ResampleCommand { get; set; }

        public ICommand RollbackCommand { get; set; }

        [Notification]
        public List<ProductModel> ProductDict { get; set; }

        [Notification]
        public List<DictItem> ReceiveStatus { get; set; }

        // TCT 和 细胞双染，特殊处理
        private List<string> scanProductId = new List<string>() { "1395620901236842498", "1395618831419121666" };

        public ScanCellViewModel()
        {
            this.ProductDict = ExtendApiDict.Instance.ProductDict.Where(x => this.scanProductId.Contains(x.id)).ToList();
            this.ReceiveStatus = ExtendApiDict.Instance.ReceiveStatus;
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });

            this.BatchReceiveCommand = new RelayCommand<object>(data =>
            {
                if(this.CurScanReturn.records.FirstOrDefault(x => x.IsSelected) == null)
                {
                    this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }

                ScanService.Instance.BathReceive(this.CurScanReturn.records.Where(x => x.IsSelected).Select(x => x.id).ToList());

                this.QueryCommand.Execute(null);
            });

            this.RollbackCommand = new RelayCommand<object>(data =>
            {
                if (this.CurScanReturn.records.FirstOrDefault(x => x.IsSelected) == null)
                {
                    this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }

                ApiResponse<object> res = ScanService.Instance.SaveTechnicalAdviceByScan(this.CurScanReturn.records.Where(x => x.IsSelected).Select(x => x.code).ToList());
                if(!res.Success)
                {
                    Logger.Error(res.Msg);
                    this.ShowMessageBox("退回制片失败：" + res.Msg, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                else
                {
                    this.QueryCommand.Execute(null);
                }
            });

            this.ResampleCommand = new RelayCommand<ScanModel>(data =>
            {
                if (!string.IsNullOrEmpty(data?.sampleId))
                {
                    var msg = new ShowContentWindowMessage("MDE_ReSamp", "重新取样");
                    msg.DesignWidth = 400;
                    msg.DesignHeight = 300;
                    msg.Args = new object[] { data.sampleId };
                    msg.CallBackCommand = new RelayCommand<bool>(res =>
                    {
                        if (res)
                        {
                            this.QueryCommand.Execute(null);
                        }
                    });
                    Messenger.Default.Send(msg);
                }
            });
        }

        public override void LoadData()
        {
            this.CurScanReturn = ScanService.Instance.PageByCellType(this.CurScanQuery, this.PageModel.PageIndex, this.PageModel.PageSize);
            this.PageModel.TotalPage = this.CurScanReturn.pages.Value;
            this.PageModel.TotalNum = this.CurScanReturn.total.Value;
        }

        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.QueryCommand.Execute(null);
            }
        }
    }
}
