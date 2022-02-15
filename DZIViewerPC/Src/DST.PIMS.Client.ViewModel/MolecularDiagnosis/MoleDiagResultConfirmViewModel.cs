using DST.ApiClient.Service;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class MoleDiagResultConfirmViewModel : CustomBaseViewModel
    {
        private List<ExamedModel> curSelectList = null;

        [Notification]
        public ObservableCollection<ResultInfo> ResultInfoList { get; set; } = new ObservableCollection<ResultInfo>();

        [Notification]
        public string SampleCount { get; set; } = "0";

        [Notification]
        public List<SampleRcrResultConfirm> CurResultConfirmList { get; set; }

        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand ConfirmCommand { get; set; }

        public MoleDiagResultConfirmViewModel()
        {

        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null)
            {
                this.curSelectList = this.Args[0] as List<ExamedModel>;
                this.SampleCount = this.curSelectList?.Count.ToString();
            }
        }

        public override void LoadData()
        {
            if(this.curSelectList == null)
            {
                return;
            }

            List<DictItem> largeResultList = DictService.Instance.GetReportLargeResult().Result;

            foreach (IGrouping<string, ExamedModel> group in this.curSelectList.GroupBy(x => x.reportLargeResult))
            {
                this.ResultInfoList.Add(new ResultInfo() { Result = largeResultList.FirstOrDefault(x => x.dictKey.Equals(group.Key))?.dictValue, Count = group.ToList().Count });
            }

            this.CurResultConfirmList = MolecularDiagnosisService.Instance.ListPositiveResultBySampleIds(this.curSelectList.Select(x => x.sampleId).ToList());
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();
            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.ConfirmCommand = new RelayCommand<object>(data =>
            {
                MolecularDiagnosisService.Instance.BatchConfirmPcrSampleList(this.curSelectList.Select(x => x.sampleId).ToList());
                this.CloseContentWindow();
            });
        }
    }

    public class ResultInfo
    {
        public string Result { get; set; }
        public int Count { get; set; }
    }
}
