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
    public class CWD_EditViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 常用词实体
        /// </summary>
        [Notification]
        public ComWordModel ComWord { get; set; }
        /// <summary>
        /// 常用词类型字典
        /// </summary>
        [Notification]
        public List<ComWordType> TypeDict { get; set; }

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
                this.Result = ComWord;
                this.CloseContentWindow();
            }
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 2 && this.Args[0] is ComWordModel model && this.Args[1] is List<ComWordType> dict)
            {
                ComWord = model;
                TypeDict = dict;
            }
        }
    }
}
