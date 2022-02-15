using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
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
    public class MoleDiagExamedViewModel : CustomBaseViewModel
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
                this.CurExamedReturnModel.records.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        /// <summary>
        /// 查询条件对象
        /// </summary>
        [Notification]
        public ExamedQueryModel CurExamedQueryModel { get; set; } = new ExamedQueryModel();

        /// <summary>
        /// 查询返回结果
        /// </summary>
        [Notification]
        public ExamedReturnModel CurExamedReturnModel { get; set; } = new ExamedReturnModel();

        /// <summary>
        /// 当前选中
        /// </summary>
        [Notification]
        public ExamedModel CurSelectedModel { get; set; }

        public ICommand QueryCommand { get; set; }

        public ICommand DetailCommand { get; set; }

        public ICommand ReviewCommand { get; set; }

        public ICommand RepeatedCommand { get; set; }

        [Notification]
        public List<ProductModel> ProductDict { get; set; }

        [Notification]
        public List<DictItem> TrialStatusList { get; set; }

        [Notification]
        public List<DictItem> HpvStatusList { get; set; }

        [Notification]
        public List<DictItem> LargeResultList { get; set; }

        public MoleDiagExamedViewModel()
        {
            this.ProductDict = ExtendApiDict.Instance.ProductDict.Where(x => x.type == "3").ToList();
            this.TrialStatusList = DictService.Instance.GetTrialStatus().Result;
            this.HpvStatusList = DictService.Instance.GetHpvStatus().Result;
            this.LargeResultList = DictService.Instance.GetReportLargeResult().Result;
            ExtendApiDict.Instance.TrialStatusList = this.TrialStatusList;
            ExtendApiDict.Instance.HpvStatusList = this.HpvStatusList;
            ExtendApiDict.Instance.LargeResultList = this.LargeResultList;
        }

        public override void LoadData()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                this.CurExamedReturnModel = MolecularDiagnosisService.Instance.PagePcrByCondition(this.CurExamedQueryModel, this.PageModel.PageIndex, this.PageModel.PageSize);
                this.PageModel.TotalPage = this.CurExamedReturnModel.pages.Value;
                this.PageModel.TotalNum = this.CurExamedReturnModel.total.Value;
                this.SelectAll = false;
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            // 查询
            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });

            // 查看
            this.DetailCommand = new RelayCommand<object>(data =>
            {
                ShowContentWindowMessage msg = new ShowContentWindowMessage("MoleDiagDetail", "检测结果");
                msg.DesignWidth = 800;
                msg.DesignHeight = 750;
                msg.Args = new object[] { this.CurSelectedModel };
                Messenger.Default.Send(msg);
            });

            // 重新取样
            this.ReviewCommand = new RelayCommand<ExamedModel>(data =>
            {
                if (!string.IsNullOrEmpty(data?.sampleId))
                {
                    var msg = new ShowContentWindowMessage("MDE_ReSamp", "重新取样");
                    msg.DesignWidth = 400;
                    msg.DesignHeight = 300;
                    msg.Args = new object[] { data.sampleId };
                    msg.CallBackCommand = new RelayCommand<bool>(res =>
                    {
                        if (res)
                        {
                            this.QueryCommand.Execute(null);
                        }
                    });
                    Messenger.Default.Send(msg);
                }
            });

            // 标记复测
            this.RepeatedCommand = new RelayCommand<object>(data =>
            {
                MolecularDiagnosisService.Instance.UpdateMarkRetest(this.CurSelectedModel.sampleId);
                this.LoadData();
                this.SelectAll = false;
            });

            Messenger.Default.Register<object>(this, EnumMessageKey.RefreshMoleDiagExamed, data =>
            {
                this.QueryCommand.Execute(null);
            });
        }

        /// <summary>
        /// 结果确认
        /// </summary>
        protected override void CheckResult()
        {
            if (this.CurExamedReturnModel.records.FirstOrDefault(x => x.IsSelected) == null)
            {
                this.ShowMessageBox("未选中数据！");
                return;
            }

            List<ExamedModel> tmpList = this.CurExamedReturnModel.records.Where(x => x.IsSelected).ToList();

            ShowContentWindowMessage msg = new ShowContentWindowMessage("MoleDiagResultConfirm", "结果确认");
            msg.DesignWidth = 800;
            msg.DesignHeight = 600;
            msg.Args = new object[] { tmpList };
            msg.CallBackCommand = new RelayCommand<object>(data =>
            {
                this.QueryCommand.Execute(null);
            });
            Messenger.Default.Send(msg);
        }

        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.QueryCommand.Execute(null);
            }
        }
    }
}
