using DST.ApiClient.Service;
using DST.Controls;
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
    public class RegReSamp_QueryViewModel : CustomBaseViewModel
    {
        [Notification]
        public List<SampleModel> SampList { get; set; } = new List<SampleModel>();

        public ReSampModel ReSamp { get; set; }
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            PageModel.PageIndex = 1;
            LoadData();
        })).Value;
        /// <summary>
        /// 选择样本
        /// </summary>
        public ICommand SelectSampCommand => new Lazy<RelayCommand<SampleModel>>(() => new RelayCommand<SampleModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.SampleID))
            {
                this.Result = data;
                this.CloseContentWindow();
            }
        })).Value;
        
        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 1 && this.Args[0] is ReSampModel reSamp)
            {
                ReSamp = reSamp;
                this.QueryCommand.Execute(null);
            }
        }
        /// <summary>
        /// 分页
        /// </summary>
        public override void LoadData()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                SampList = ReSampService.Instance.GetSampListbyReSampInfo(PageModel, ReSamp);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
    }
}
