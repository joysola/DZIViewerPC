using DST.ApiClient.Service;
using DST.Database.Model;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ReceiveByScanViewModel : CustomBaseViewModel
    {
        [Notification]
        public int ScanCount { get; set; } = 0;

        [Notification]
        public string Code { get; set; }

        [Notification]
        public ScanModel CurSelectedScanModel { get; set; }

        [Notification]
        public ObservableCollection<ScanModel> ScanModelList { get; set; } = new ObservableCollection<ScanModel>();

        [Notification]
        public ICommand DeleteCommand { get; set; }

        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand SaveCommand { get; set; }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.DeleteCommand = new RelayCommand<object>(data =>
            {
                this.ScanModelList.Remove(this.CurSelectedScanModel);
                this.ScanCount = this.ScanModelList.Count;
            });

            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.SaveCommand = new RelayCommand<object>(data =>
            {
                bool res = ScanService.Instance.ReceiveByCodeList(this.ScanModelList.Select(x => x.code).ToList());
                if (res)
                {
                    this.CloseContentWindow();
                }
            });
        }

        /// <summary>
        /// 根据实验室编号查询
        /// </summary>
        public void PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ScanModel newIns = ScanService.Instance.GetSampleScanByCode(this.Code);
                if(newIns == null)
                {
                    this.ShowMessageBox("未找到数据！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    Messenger.Default.Send<bool>(true, EnumMessageKey.ResetFocus);
                    this.Code = "";
                    return;
                }

                if(this.ScanModelList.FirstOrDefault(x => x.code.Equals(newIns.code)) != null)
                {
                    this.CurSelectedScanModel = this.ScanModelList.FirstOrDefault(x => x.code.Equals(newIns.code));
                    this.ShowMessageBox("请勿重复扫码！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    Messenger.Default.Send<bool>(true, EnumMessageKey.ResetFocus);
                    this.Code = "";
                    return;
                }

                newIns.receiverTime = DateTime.Now;
                this.ScanModelList.Insert(0, newIns);
                this.ScanModelList = new ObservableCollection<ScanModel>(this.ScanModelList);
                this.Code = "";
                this.ScanCount = this.ScanModelList.Count;
            }
        }
    }
}
