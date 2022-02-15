using DST.ApiClient.Service;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class RegisterDeliveryViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 查询barcode
        /// </summary>
        [Notification]
        public string BarCode { get; set; }

        /// <summary>
        /// 登记列表
        /// </summary>
        [Notification]
        public ObservableCollection<SampleRegisterModel> RegSampList { get; set; } = new ObservableCollection<SampleRegisterModel>();
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(BarCode))
            {
                var result = RegisterService.Instance.GetDeliverySamplebyBarcode(BarCode);
                result.ForEach(reg =>
                {
                    // 没找到则加入
                    if (RegSampList.FirstOrDefault(x => x.ID == reg.ID) == null)
                    {
                        RegSampList.Add(reg);
                    }
                });
                BarCode = null; // 清空
            }
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
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var result = RegisterService.Instance.SaveDeliverySampleList(RegSampList);
            if (result)
            {
                this.Result = result;
                this.CloseContentWindow();
            }
            else
            {
                ShowMessageBox("批量外送失败！", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        })).Value;

        public RegisterDeliveryViewModel()
        {

        }
        /// <summary>
        /// 回车搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void QueryPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryCommand.Execute(null);
            }
        }
    }
}
