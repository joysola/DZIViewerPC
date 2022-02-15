using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class MoleDiagCheckViewModel : CustomBaseViewModel
    {
        private bool selectAll = false;

        /// <summary>
        /// 全选
        /// </summary>
        public bool SelectAll
        {
            get { return this.selectAll; }
            set
            {
                this.selectAll = value;
                this.RaisePropertyChanged("SelectAll");
                this.CurSampleApproveReturn.records.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        [Notification]
        public string BtnContext { get; set; } = "批量初审";

        /// <summary>
        /// 查询实体
        /// </summary>
        [Notification]
        public SampleApproveQuery CurSampleApproveQuery { get; set; } = new SampleApproveQuery();

        [Notification]
        public SampleApproveReturn CurSampleApproveReturn { get; set; } = new SampleApproveReturn();

        [Notification]
        public SampleApproveModel CurSelectedApproveModel { get; set; }

        public ICommand QueryCommand { get; set; }

        public ICommand BatchCheckCommand { get; set; }

        public ICommand ReviewReportComman { get; set; }

        [Notification]
        public List<ProductModel> ProductDict { get; set; }

        [Notification]
        public List<DictItem> LargeResultList { get; set; }

        public MoleDiagCheckViewModel()
        {
            this.ProductDict = ExtendApiDict.Instance.ProductDict.Where(x => x.type == "3").ToList();
            this.LargeResultList = DictService.Instance.GetReportLargeResult().Result;
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if (this.Args != null)
            {
                int checkType = int.Parse(this.Args[0].ToString());
                this.BtnContext = checkType == 1 ? "批量初审" : "批量复审";
                this.CurSampleApproveQuery.auditStatus = checkType;
            }
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });

            this.BatchCheckCommand = new RelayCommand<object>(data =>
            {
                this.BatchCheck();
            });

            this.ReviewReportComman = new RelayCommand<SampleApproveModel>(data =>
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    if (data != null)
                    {
                        ReportUrl res = ReportSystemService.Instance.GetReportUrlBySampleId(data.sampleId);
                        if (res != null && !string.IsNullOrEmpty(res.reportUrl))
                        {
                            ShowContentWindowMessage msg = new ShowContentWindowMessage("ReportShow", "报告预览");
                            msg.DesignHeight = 750;
                            msg.DesignWidth = 1000;
                            string info = $"当前病例：{ data.laboratoryCode }                 {data.patientName}      {ExtendApiDict.Instance.SexDict.FirstOrDefault(x => x.dictKey.Equals(data.sex)).dictValue}      {data.age}岁                 检查项目：{data.productName}";
                            msg.Args = new object[] { info, res };
                            Messenger.Default.Send(msg);
                        }
                        else
                        {
                            WhirlingControlManager.CloseWaitingForm();
                            this.ShowMessageBox("获取报告链接失败！");
                        }
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            });
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        private void BatchCheck()
        {
            if (this.CurSampleApproveReturn.records.FirstOrDefault(x => x.IsSelected) == null)
            {
                this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                return;
            }

            string title = this.CurSampleApproveQuery.auditStatus == 1 ? "批量初审" : "批量复审";
            ShowContentWindowMessage msg = new ShowContentWindowMessage("MoleDiagCheckConfirm", title);
            msg.DesignHeight = 500;
            msg.DesignWidth = 600;
            msg.Args = new object[] { this.CurSampleApproveQuery.auditStatus, this.CurSampleApproveReturn.records.Where(x => x.IsSelected).ToList() };
            msg.CallBackCommand = new RelayCommand<object>(res =>
            {
                if (res != null)
                {
                    MolecularDiagnosisService.Instance.BatchApproveSampleList(int.Parse(res.ToString()), this.CurSampleApproveQuery.auditStatus, this.CurSampleApproveReturn.records.Where(x => x.IsSelected).Select(x => x.sampleId).ToList());
                    this.LoadData();
                }
            });
            Messenger.Default.Send(msg);
        }

        public override void LoadData()
        {
            this.CurSampleApproveReturn = MolecularDiagnosisService.Instance.PagePcrByCondition(this.CurSampleApproveQuery, this.PageModel.PageIndex, this.PageModel.PageSize);
            this.PageModel.TotalPage = this.CurSampleApproveReturn.pages ?? 0;
            this.PageModel.TotalNum = this.CurSampleApproveReturn.total ?? 0;
        }
    }
}
