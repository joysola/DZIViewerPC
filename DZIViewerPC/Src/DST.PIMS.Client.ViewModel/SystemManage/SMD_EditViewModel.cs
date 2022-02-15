using DST.Database.Model;
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
    public class SMD_EditViewModel : CustomBaseViewModel
    {
        [Notification]
        public DoctorInfoModel DoctorInfo { get; set; }

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
                if (DoctorInfo?.Sex == null)
                {
                    ShowMessageBox("性别必填！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                this.Result = DoctorInfo;
                this.CloseContentWindow();
            }
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is DoctorInfoModel doctor)
            {
                this.DoctorInfo = doctor;
            }
        }
    }
}
