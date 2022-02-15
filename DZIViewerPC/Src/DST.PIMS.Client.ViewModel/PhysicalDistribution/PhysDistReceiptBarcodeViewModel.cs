using DST.ApiClient.Service;
using DST.Database.Model;
using DST.PIMS.Framework;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistReceiptBarcodeViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 查询实体
        /// </summary>
        [Notification]
        public SignExpressByScan QueryExpressModel { get; set; } = new SignExpressByScan() { expressId = "" };

        /// <summary>
        /// 所有签收样本
        /// </summary>
        [Notification]
        public ObservableCollection<Sample> AllScanSignList { get; set; } = new ObservableCollection<Sample>();

        /// <summary>
        /// 扫码返回接收到数据实体，用于条码打印
        /// </summary>
        [Notification]
        public ObservableCollection<PhysDistReceiptBarcodeModel> CurReceiptModelList { get; set; } = new ObservableCollection<PhysDistReceiptBarcodeModel>();

        public PhysDistReceiptBarcodeViewModel()
        {
        }

        public void PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                List<PhysDistReceiptBarcodeModel> newModel = PhysicalDistributionService.Instance.SaveSignExpressByScan(this.QueryExpressModel);
                if(newModel == null)
                {
                    this.ShowMessageBox("未找到匹配样本，请确认！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                }
                else
                {
                    newModel.ForEach(x =>
                    {
                        this.CurReceiptModelList.Add(x);
                    });

                    Task.Run(() => PrintLabelManager.Singleton.Print(newModel));
                    this.LoadData();
                    this.ShowMessageBox("签收成功！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information, null, true, 1000);
                }

                Messenger.Default.Send<object>(null, EnumMessageKey.PhysDistReceiptBarcodeFocus);
            }
        }

        public override void LoadData()
        {
            this.AllScanSignList = new ObservableCollection<Sample>(PhysicalDistributionService.Instance.GetListScanSign());
        }
    }
}
