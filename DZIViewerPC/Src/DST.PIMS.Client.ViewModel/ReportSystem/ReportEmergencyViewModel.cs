using DST.ApiClient.Service;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ReportEmergencyViewModel : CustomBaseViewModel
    {
        [Notification]
        public List<ProductModel> ProductModelList { get; set; }

        [Notification]
        public ProductModel CurProductModel { get; set; }

        /// <summary>
        /// 检查项目的子项目
        /// </summary>
        [Notification]
        public ProductType CurProductType { get; set; }

        [Notification]
        public List<DictItem> ActivityTypeDict { get; set; }

        [Notification]
        public List<DictItem> DoctAdviceDict { get; set; }

        [Notification]
        public List<ReportSliceModel> ReportSliceModelList { get; set; }

        [Notification]
        public ReportSliceModel CurSlice { get; set; }

        [Notification]
        public ObservableCollection<MapMarks> MapMarksList { get; set; } = new ObservableCollection<MapMarks>();

        [Notification]
        public DictItem CurDocAdvice { get; set; }

        [Notification]
        public Report CurSelectedPeport { get; set; }

        [Notification]
        public DictItem CurActivityType { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        [Notification]
        public List<MarkModel> CurSelectedMark { get; set; } = new List<MarkModel>();

        [Notification]
        public ReportDoctAdvice CurReportDoctAdvice { get; set; } = new ReportDoctAdvice();

        [Notification]
        public DocAdviceModel CurSelectedDocAdviceModel { get; set; }

        public ReportEmergencyViewModel()
        {
            this.ProductModelList = ReportSystemService.Instance.ListByAddProduct();
            this.ActivityTypeDict = ExtendApiDict.Instance.ActivityTypeDict;
            this.DoctAdviceDict = ExtendApiDict.Instance.DoctAdviceDict;
            List<MarkModel> markList = ReportSystemService.Instance.ListByMarkers();
            foreach(IGrouping<string, MarkModel> item in markList.GroupBy(x => x.valueShort))
            {
                this.MapMarksList.Add(new MapMarks() { key = item.Key, markList = new ObservableCollection<MarkModel>(item.ToList()) });
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="markModel">对象</param>
        /// <param name="isAdd">true = 新增，false = 删除</param>
        public void AddOrRemoveMarkModel(MarkModel markModel, bool isAdd)
        {
            if(isAdd)
            {
                if (null == this.CurSlice)
                {
                    this.ShowMessageBox("请重新选择切片！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }
                else if(null == this.CurDocAdvice)
                {
                    this.ShowMessageBox("请重新选择特检医嘱类型！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }
                else if (null == this.CurActivityType)
                {
                    this.ShowMessageBox("请重新选择活动类型！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }

                this.CurReportDoctAdvice.sampleTissueDoctorAdviceList.Add(new DocAdviceModel() 
                { 
                    waxBlockNumber = this.CurSlice.waxBlockNumber,
                    marker = markModel.key,
                    markerValue = markModel.value,
                    code = this.CurSlice.sliceNumber
                });
            }
            else
            {
                DocAdviceModel delModel = this.CurReportDoctAdvice.sampleTissueDoctorAdviceList.FirstOrDefault(x => x.marker.Equals(markModel.key));
                if (delModel != null)
                {
                    this.CurReportDoctAdvice.sampleTissueDoctorAdviceList.Remove(delModel);
                }
            }
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.CloseCommand = new RelayCommand<object>(data =>
            {
                this.CloseContentWindow();
            });

            this.SaveCommand = new RelayCommand<object>(data =>
            {
                if(this.SaveNewData())
                {
                    this.CloseContentWindow();
                }
            });

            this.DeleteCommand = new RelayCommand<object>(data =>
            {
                string marker = this.CurSelectedDocAdviceModel?.marker;
                this.MapMarksList.ToList().ForEach(x =>
                {
                    MarkModel tmp = x.selectedMarkList.FirstOrDefault(t => !string.IsNullOrEmpty(marker) && t.key.Equals(marker));
                    if(tmp != null)
                    {
                        x.selectedMarkList.Remove(tmp);
                        x.selectedMarkList = new ObservableCollection<MarkModel>(x.selectedMarkList);
                        return;
                    }
                });
            });
        }

        protected bool SaveNewData()
        {
            bool result = true;
            if (this.CurSelectedPeport.addType.HasValue && this.CurSelectedPeport.addType.Value == 1)
            {
                if (null == this.CurProductModel)
                {
                    this.ShowMessageBox("请选择加测项目！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return false;
                }
                else if (null == this.CurActivityType)
                {
                    this.ShowMessageBox("请重新选择活动类型！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return false;
                }

                return ReportSystemService.Instance.SaveAddSample(this.CurSelectedPeport.pathologyId, this.CurProductModel.id, this.CurProductType?.value, this.CurActivityType.dictKey);
            }
            else if (this.CurSelectedPeport.addType.HasValue && this.CurSelectedPeport.addType.Value == 2)
            {
                if (null == this.CurDocAdvice)
                {
                    this.ShowMessageBox("请选择加测项目！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return false;
                }
                else if (null == this.CurActivityType)
                {
                    this.ShowMessageBox("请重新选择活动类型！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return false;
                }
                else if (null == this.CurSlice)
                {
                    this.ShowMessageBox("请重新选择切片！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return false;
                }

                this.CurReportDoctAdvice.screen = this.CurActivityType.dictKey;
                this.CurReportDoctAdvice.productId = this.CurDocAdvice.dictKey;
                return ReportSystemService.Instance.SaveAddSampleDoctorAdviceAudit(this.CurReportDoctAdvice);
            }

            return result;
        }

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null && this.Args[0] != null)
            {
                this.CurSelectedPeport = this.Args[0] as Report;
                this.ReportSliceModelList = ReportSystemService.Instance.ListCuttingScanVoBySampleId(this.CurSelectedPeport.id);
                this.CurReportDoctAdvice.sampleId = this.CurSelectedPeport.id;
            }
        }

        public override void LoadData()
        {

        }
    }
}
