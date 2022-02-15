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
    public class QueryPathTitleViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 样本信息
        /// </summary>
        [Notification]
        public SampleModel PathModel { get; set; } = new SampleModel();

        /// <summary>
        /// 切换样本信息
        /// </summary>
        public ICommand ChangePathModelCommand => new Lazy<RelayCommand<SampleModel>>(() => new RelayCommand<SampleModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.SampleID))
            {
                PathModel = data;
            }
        })).Value;

    }
}
