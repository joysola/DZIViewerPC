using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ProdSpec_EditViewModel : CustomBaseViewModel
    {
        [Notification]
        public SliceModel Slice { get; set; }

        /// <summary>
        /// 确认
        /// </summary>
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (!string.IsNullOrEmpty(Slice?.ID))
                {
                    var result = ProductService.Instance.SaveSlice(Slice); // 保存信息
                    if (result)
                    {
                        this.Result = true;
                        this.CloseContentWindow();
                    }
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }

        })).Value;
        /// <summary>
        /// 取消
        /// </summary>
        public ICommand CancelCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.CloseContentWindow();
        })).Value;
        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is SliceModel model)
            {
                if (!string.IsNullOrEmpty(model?.ID))
                {
                    Slice = model;
                }
            }
        }
    }
}
