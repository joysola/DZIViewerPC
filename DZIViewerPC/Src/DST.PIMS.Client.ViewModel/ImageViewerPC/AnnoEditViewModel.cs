using DST.Database.Model;
using DST.PIMS.Framework.Controls;
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
    [NotifyAspect]
    public class AnnoEditViewModel : CustomBaseViewModel
    {
        public AnnoEditModel Model { get; set; }
       
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
            this.Result = Model;
            this.CloseContentWindow();
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args?.Length == 1 && this.Args[0] is AnnoEditModel model)
            {
                Model = model;
            }
        }
    }
}
