using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
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
    public class MaterialDelayViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 包埋盒
        /// </summary>
        [Notification]
        public SampTissDelayInfo SamplTissDelay { get; set; }
        /// <summary>
        /// 是否新增
        /// </summary>
        [Notification]
        public bool IsAdd { get; set; }
        /// <summary>
        /// 样本取材延迟原因
        /// </summary>
        public List<DictItem> SamplTissDelayDict => ExtendApiDict.Instance.SamplTissDelayDict;
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
            this.Result = SamplTissDelay;
            this.CloseContentWindow();
        })).Value;

        public override void OnViewLoaded()
        {
            if (this.Args != null&&this.Args.Length==2 && this.Args[0] is SampTissDelayInfo sampDelay && this.Args[1] is bool isAdd)
            {
                SamplTissDelay = sampDelay;
                IsAdd = isAdd;
            }
        }
    }
}
