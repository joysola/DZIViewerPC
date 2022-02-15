using DST.ApiClient.Service;
using DST.Common.Helper.ExcelHelper;
using DST.Controls;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistPreTreatViewModel : CustomBaseViewModel
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
                this.CurInspectionInfo.records.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        /// <summary>
        /// 检查项目列表
        /// </summary>
        [Notification]
        public List<ProductModel> ProductModelList { get; set; } = new List<ProductModel>();

        /// <summary>
        /// 送检查询条件
        /// </summary>
        [Notification]
        public QueryInspection CurQueryInspection { get; set; } = new QueryInspection() { code = "", laboratoryCode = "", mailNo = "", patientName = "", productId = "", pathologyType = "" };

        /// <summary>
        /// 送检列表明细
        /// </summary>
        [Notification]
        public InspectionInfo CurInspectionInfo { get; set; }

        /// <summary>
        /// 搜索按钮命令
        /// </summary>
        public ICommand QueryCommand { get; set; }

        /// <summary>
        /// 批量送检
        /// </summary>
        public ICommand BatchTreatCommand { get; set; }

        /// <summary>
        /// 病理类型
        /// </summary>
        public List<DictItem> PathologyTypeList { get; set; }

        public PhysDistPreTreatViewModel()
        {
            this.ProductModelList = ExtendApiDict.Instance.ProductDict;
            if(ExtendApiDict.Instance.PathologyTypeList == null)
            {
                ExtendApiDict.Instance.PathologyTypeList = DictService.Instance.GetPathologyTypeList().Result;
            }

            this.PathologyTypeList = ExtendApiDict.Instance.PathologyTypeList;
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });

            this.BatchTreatCommand = new RelayCommand<object>(data =>
            {
                this.BatchTreat();
            });

            Messenger.Default.Register<object>(this, EnumMessageKey.RefreshPhysDistSign, data =>
            {
                this.QueryCommand.Execute(null);
            });
        }

        /// <summary>
        /// 批量送检样本
        /// </summary>
        private void BatchTreat()
        {
            if (this.CurInspectionInfo.records.FirstOrDefault(x => x.IsSelected) == null)
            {
                this.ShowMessageBox("请选中批量送检样本！");
                return;
            }

            List<Inspection> tmpSelected = this.CurInspectionInfo.records.Where(x => x.IsSelected).ToList();
            string msg = $" 本次前处理将提交{tmpSelected.Count}个样本，请仔细核对!\r\n";

            // 分组计算
            foreach (IGrouping<string, Inspection> group in tmpSelected.GroupBy(x => x.productId))
            {
                string productName = ExtendApiDict.Instance.ProductDict.FirstOrDefault(x => x.id.Equals(group.Key))?.name;
                int length = group.ToList().Count;
                msg += $" {productName} : {length} \r\n";
            }

            msg += " 请确认是否继续？";
            this.ShowMessageBox(msg, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question,
               async res =>
               {
                   if (res == System.Windows.MessageBoxResult.Yes ||
                      res == System.Windows.MessageBoxResult.OK)
                   {
                       //ApiResponse<object> result = PhysicalDistributionService.Instance.SaveInspectionSampleList(tmpSelected.Select(x => x.id).ToList());
                       //if (result.Success)
                       //{
                       //DataTable dt = TreatRepertManager.Instance.TreatRepertListToDataTable(JsonConvert.DeserializeObject<List<TreatRepertModel>>(result.Data?.ToString()));
                       //    string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + $"\\{DateTime.Now.ToString("yyyyMMddHHmmssms")}.xlsx";
                       //    ExcelHelper.WriteExcelFile(deskDir, dt);
                       //    this.LoadData();
                       //}
                       try
                       {
                           WhirlingControlManager.ShowWaitingForm();
                           var result = await PhysicalDistributionService.Instance.SaveInspeSampleListandGetExcel(tmpSelected.Select(x => x.id).ToList());
                           if (result)
                           {
                               this.LoadData();
                           }
                       }
                       finally
                       {
                           WhirlingControlManager.CloseWaitingForm();
                       }
                   }
               });
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public override void LoadData()
        {
            this.CurInspectionInfo = PhysicalDistributionService.Instance.PageByInspection(this.CurQueryInspection, this.PageModel.PageIndex, this.PageModel.PageSize);
            this.PageModel.TotalPage = this.CurInspectionInfo.pages.Value;
            this.PageModel.TotalNum = this.CurInspectionInfo.total.Value;
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
