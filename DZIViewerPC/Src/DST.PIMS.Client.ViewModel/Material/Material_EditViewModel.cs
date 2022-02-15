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
    public class Material_EditViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 包埋盒
        /// </summary>
        [Notification]
        public EmbedBoxModel EmbedBox { get; set; }
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
            this.Result = EmbedBox;
            this.CloseContentWindow();
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args[0] is EmbedBoxModel embed)
            {
                EmbedBox = embed;
            }
        }
    }
}
