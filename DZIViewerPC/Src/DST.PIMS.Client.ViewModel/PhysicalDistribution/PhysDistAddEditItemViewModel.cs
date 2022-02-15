using DST.ApiClient.Service;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.Command;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PhysDistAddEditItemViewModel : CustomBaseViewModel
    {
        private string hospitalID = "";

        [Notification]
        public ICommand CloseCommand { get; set; }

        [Notification]
        public ICommand ConfirmCommand { get; set; }

        [Notification]
        public ICommand NewCommand { get; set; }

        [Notification]
        public ICommand DeleteCommand { get; set; }

        /// <summary>
        /// 当前选中的检查项目
        /// </summary>
        [Notification]
        public ProductModel CurProductModel { get; set; }

        /// <summary>
        /// 检查项目的子项目
        /// </summary>
        [Notification]
        public ProductType CurProductType { get; set; }

        /// <summary>
        /// 检查项目列表
        /// </summary>
        [Notification]
        public ObservableCollection<ProductModel> ProductList { get; set; } = new ObservableCollection<ProductModel>();

        /// <summary>
        /// 检查项目表格明细
        /// </summary>
        [Notification]
        public ObservableCollection<Sample> SampleList { get; set; } = new ObservableCollection<Sample>();

        [Notification]
        public Sample CurSelectedSample { get; set; }

        /// <summary>
        /// 活动类型：38妇女
        /// </summary>
        [Notification]
        public ObservableCollection<DictItem> ActivityTypeList { get; set; } = new ObservableCollection<DictItem>();

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            if(this.Args != null && this.Args[0] != null)
            {
                this.hospitalID = this.Args[0].ToString();
                
                if(null != this.Args[1] && this.Args[1] is List<Sample>)
                {
                    (this.Args[1] as List<Sample>).ForEach(x =>
                    {
                        x.ProdReagentDict = ApplyFormService.Instance.GenerateMarkerList(x.productId);
                        if(!string.IsNullOrEmpty(x.markers))
                        {
                            // 胃镜单独处理
                            if (DSTCode.GastroscopeProdID == x.productId)
                            {
                                x.CurSelectedProdReagent.Add(x.ProdReagentDict.FirstOrDefault(t => t.dictKey.Equals(x.markers)));
                            }
                            else if (DSTCode.ImmuhistchmProdID == x.productId)
                            {
                                string[] arr = x.markers.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    if (x.ProdReagentDict.FirstOrDefault(t => t.dictKey.Equals(arr[i])) != null)
                                    {
                                        x.CurSelectedProdReagent.Add(x.ProdReagentDict.FirstOrDefault(t => t.dictKey.Equals(arr[i])));
                                    }
                                }
                            }
                        }
                    });
                    this.SampleList = new ObservableCollection<Sample>(this.Args[1] as List<Sample>);
                }
            }
        }

        public override void LoadData()
        {
            this.ProductList = new ObservableCollection<ProductModel>(DictService.Instance.GetProductDict(this.hospitalID).Result);
            this.ActivityTypeList = new ObservableCollection<DictItem>(DictService.Instance.GetActivityTypeList().Result);
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
                if(this.SampleList.FirstOrDefault(x => string.IsNullOrEmpty(x.screen)) != null)
                {
                    this.ShowMessageBox("活动类型不能为空！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }
                // 免疫组化，标记物必填
                Sample tmp = this.SampleList.FirstOrDefault(x => x.productId.Equals("1395620999601659906"));
                if ( tmp != null)
                {
                    if(tmp.CurSelectedProdReagent == null || tmp.CurSelectedProdReagent.Count == 0)
                    {
                        this.ShowMessageBox("免疫组化的标记物不能为空！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                        return;
                    }
                }

                this.SampleList.ToList().ForEach(x =>
                {
                    if(x.CurSelectedProdReagent != null && x.CurSelectedProdReagent.Count > 0)
                    {
                        x.markers = string.Join(",", x.CurSelectedProdReagent.Select(t => t.dictKey));
                    }
                });
                this.Result = this.SampleList.ToList();
                this.CloseContentWindow();
            });

            // 添加新检查项目
            this.NewCommand = new RelayCommand<object>(data =>
            { 
                if(this.CurProductModel != null)
                {
                    if(this.SampleList.FirstOrDefault(x => x.productId.Equals(this.CurProductModel.id)) != null)
                    {
                        this.ShowMessageBox("请勿重复添加检查项目！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                        return;
                    }

                    Sample newSample = new Sample();
                    newSample.screen = "1";
                    newSample.productId = this.CurProductModel.id;
                    newSample.productName = this.CurProductModel.name;
                    newSample.gatherTime = null;

                    if(this.CurProductType != null)
                    {
                        newSample.productType = this.CurProductType.value;
                        newSample.productName += "/" + this.CurProductType.name;
                    }

                    // 免疫组化特殊处理
                    newSample.ProdReagentDict = ApplyFormService.Instance.GenerateMarkerList(this.CurProductModel.id);
                    this.SampleList.Add(newSample);
                }
            });

            this.DeleteCommand = new RelayCommand<object>(data =>
            { 
                if(null != this.CurSelectedSample)
                {
                    this.SampleList.Remove(this.CurSelectedSample);
                }
            });
        }
    }
}
