using DST.ApiClient.Service;
using DST.Controls.Base;
using DST.PIMS.Framework;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ProductionMainViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 查询样本列表
        /// </summary>
        public QueryPathListViewModel QueryPathListVM { get; set; } = new QueryPathListViewModel(ProductService.Instance.GetProductList);
        /// <summary>
        /// 常规制片
        /// </summary>
        public ProdSpecViewModel ProdSpecVM { get; set; } = new ProdSpecViewModel();
        /// <summary>
        /// TCT制片
        /// </summary>
        public ProdTCTListViewModel ProdTCTListVM { get; set; } = new ProdTCTListViewModel();
        /// <summary>
        /// 技术医嘱
        /// </summary>
        public ProdTechAdviceViewModel ProdTechAdVM { get; set; } = new ProdTechAdviceViewModel();
        /// <summary>
        /// 特检医嘱
        /// </summary>
        public ProdDoctAdviceViewModel ProdDoctAdVM { get; set; } = new ProdDoctAdviceViewModel();
        /// <summary>
        /// 制片列表
        /// </summary>
        public ProdSliceListViewModel ProdSlicListVM { get; set; } = new ProdSliceListViewModel();
        /// <summary>
        /// 申请单ViewModel
        /// </summary>
        public AppFrmViewModel AppViewModel { get; set; } = new AppFrmViewModel();
        /// <summary>
        /// 当前tabitem名称
        /// </summary>
        [Notification]
        public HandyControl.Controls.TabItem SelectedTabItem { get; set; }
        /// <summary>
        /// 查询申请单
        /// </summary>
        public ICommand AppViewCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            string code = string.Empty;
            if (SelectedTabItem.Header is string currentTabName)
            {
                switch (currentTabName)
                {
                    case "常规制片":
                        code = QueryPathListVM?.SelectedModel?.LabCode;
                        break;
                    case "技术医嘱":
                        code = ProdTechAdVM?.TechAdQueryVM?.SelectedModel?.LabCode;
                        break;
                    case "特检医嘱":
                        code = ProdDoctAdVM?.DoctAdQueryVM?.SelectedModel?.LabCode;
                        break;
                    case "制片列表":
                        code = ProdSlicListVM?.SelectedModel?.LabCode;
                        break;
                    case "TCT制片":
                        code = ProdTCTListVM?.SelectedModel?.LabCode;
                        break;
                    default:
                        break;
                }
            }
            if (!string.IsNullOrEmpty(code))
            {
                var msg = new ShowContentWindowMessage("AppImgView", $"申请单查看");
                msg.DesignWidth = 1200;
                msg.DesignHeight = 900;
                msg.Args = new object[] { code };
                Messenger.Default.Send(msg);
            }

        })).Value;

        public ProductionMainViewModel()
        {
            PrintLabelManager.Singleton.InitPrintManager(IniSectionConst.ProductionSection);
            QueryPathListVM.SelectedCommandList.Add(ProdSpecVM.QueryCommand); // 左侧选中样本，右侧刷新对应切片列表数据
            ProdSpecVM.ChangeSelectCommand = QueryPathListVM.ChangeSelectedCommand; // 右侧扫完码，左侧选中对应样本
        }

        /// <summary>
        /// 申请单查看
        /// </summary>
        protected override void ViewAppFrm()
        {
            AppViewCommand?.Execute(null);
        }
    }
}
