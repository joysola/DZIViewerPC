using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
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
    public class MoleDiagCheckConfirmViewModel : CustomBaseViewModel
    {
        private int auditStatus = 1;
        private List<SampleApproveModel> allSample = new List<SampleApproveModel>();

        [Notification]
        public int SampleCount { get; set; }

        /// <summary>
        /// HPV类型数据
        /// </summary>
        [Notification]
        public ObservableCollection<StatisModel> HPVStatisList { get; set; } = new ObservableCollection<StatisModel>();

        /// <summary>
        /// PCR类型数据
        /// </summary>
        [Notification]
        public ObservableCollection<StatisModel> PCRStatisList { get; set; } = new ObservableCollection<StatisModel>();

        public ICommand RejectCommand { get; set; }

        public ICommand PassCommand { get; set; }

        public MoleDiagCheckConfirmViewModel()
        {
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null)
            {
                this.auditStatus = int.Parse(this.Args[0].ToString());
                this.allSample = this.Args[1] as List<SampleApproveModel>;
                this.SampleCount = this.allSample.Count;
            }
        }

        protected override void RegisterCommand()
        {
            // 驳回
            this.RejectCommand = new RelayCommand<object>(data =>
            {
                this.Result = 2;
                this.CloseContentWindow();
            });

            // 通过
            this.PassCommand = new RelayCommand<object>(data =>
            {
                this.Result = 1;
                this.CloseContentWindow();
            });
        }

        public override void LoadData()
        {
            base.LoadData();

            // 创建数据
            foreach(IGrouping<string, SampleApproveModel> group in this.allSample.GroupBy(x => x.productId))
            {
                string productName = ExtendApiDict.Instance.ProductDict.FirstOrDefault(x => x.id.Equals(group.Key))?.name;

                // 根据子类型分组
                foreach (IGrouping<string, SampleApproveModel> childGroup in group.GroupBy(x => x.productType))
                {
                    if (childGroup != null && !string.IsNullOrEmpty(childGroup.Key))
                    {
                        this.HPVStatisList.Add(new StatisModel() { RootType = productName, ChildType = childGroup.Key, Amount = childGroup.ToList().Count });
                    }
                }

                // 根据检测结果分组
                foreach (IGrouping<string, SampleApproveModel> childGroup in group.GroupBy(x => x.reportResult))
                {
                    if (childGroup != null && !string.IsNullOrEmpty(childGroup.Key))
                    {
                        this.PCRStatisList.Add(new StatisModel() { RootType = productName, Result = childGroup.Key, Amount = childGroup.ToList().Count });
                    }
                }
            }
        }
    }

    /// <summary>
    /// 界面显示的数据模型
    /// </summary>
    public class StatisModel
    {
        public string RootType { get; set; }
        public string ChildType { get; set; }
        public string Result { get; set; }
        public int Amount { get; set; }
    }
}
