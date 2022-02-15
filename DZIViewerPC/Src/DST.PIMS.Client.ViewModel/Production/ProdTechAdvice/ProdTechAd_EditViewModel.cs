using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class ProdTechAd_EditViewModel : CustomBaseViewModel
    {
        private InspSpecimen _selectedInspSpec;
        private EmbedBoxModel _selectedEmbedBox;
        /// <summary>
        /// 是否是新增
        /// </summary>
        [Notification]
        public bool IsAdd { get; set; }
        /// <summary>
        /// 样本
        /// </summary>
        [Notification]
        public SliceModel Slice { get; set; }
        /// <summary>
        /// 已经制作包埋盒（即 蜡块集合）
        /// </summary>
        [Notification]
        public List<EmbedBoxModel> EmbedList { get; set; } = new List<EmbedBoxModel>();
        /// <summary>
        /// 临时包埋盒集合（通过部位筛选）
        /// </summary>
        [Notification]
        public ObservableCollection<EmbedBoxModel> TempEmbedList { get; set; } = new ObservableCollection<EmbedBoxModel>();
        /// <summary>
        /// 送检部位集合
        /// </summary>
        [Notification]
        public List<InspSpecimen> InSpecList { get; set; } = new List<InspSpecimen>();
        /// <summary>
        /// 医嘱类型 0  重切 1 深切 2 薄切 3 多切 4 白片 5 连切 6 补切
        /// </summary>
        [Notification]
        public ObservableCollection<DictItem> TechAdviceDict { get; set; } = new ObservableCollection<DictItem>(ExtendApiDict.Instance.TechAdviceDict);

        /// <summary>
        /// 选中的部位
        /// </summary>
        [Notification]
        public InspSpecimen SelectedInspSpec
        {
            get => _selectedInspSpec;
            set
            {
                _selectedInspSpec = value;
                TempEmbedList.Clear(); // 每次选择先清空包埋盒选项
                if (!string.IsNullOrEmpty(_selectedInspSpec?.ID))
                {
                    var list = EmbedList.Where(x => x.SampSpecID == _selectedInspSpec?.ID);
                    foreach (var item in list)
                    {
                        TempEmbedList.Add(item);
                    }
                }
            }
        }
        /// <summary>
        /// 选中的包埋盒
        /// </summary>
        [Notification]
        public EmbedBoxModel SelectedEmbedBox
        {
            get => _selectedEmbedBox;
            set
            {
                _selectedEmbedBox = value;
                if (!string.IsNullOrEmpty(_selectedEmbedBox?.ID))
                {
                    Slice.DrawMaterPlace = _selectedEmbedBox.DrawMaterPlace;
                    Slice.DrawMaterialsID = _selectedEmbedBox.ID;
                }
            }
        }


        /// <summary>
        /// 取消
        /// </summary>
        public ICommand CancelCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.CloseContentWindow();
        })).Value;
        /// <summary>
        /// 确认
        /// </summary>
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (IsCheckedOK)
            {
                if (!string.IsNullOrEmpty(Slice?.SampleID))
                {
                    WhirlingControlManager.ShowWaitingForm();
                    SliceModel result = null;
                    try
                    {
                        result = ProductService.Instance.SaveTechAdviSlice(Slice);
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                    if (!string.IsNullOrEmpty(result?.ID))
                    {
                        this.Result = result;
                        this.CloseContentWindow();
                    }
                    else
                    {
                        ShowMessageBox("报错技术医嘱切片失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        })).Value;


        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 5 &&
                this.Args[0] is SliceModel slice &&
                this.Args[1] is List<EmbedBoxModel> embeds &&
                this.Args[2] is List<InspSpecimen> inSpecs &&
                this.Args[3] is bool isAdd &&
                this.Args[4] is bool isCell)
            {
                Slice = slice;
                EmbedList = embeds;
                InSpecList = inSpecs;
                IsAdd = isAdd;
                if (!isCell) // 非细胞类型(即组织)，需要去掉细胞对应的技术医嘱
                {
                    TechAdviceDict = new ObservableCollection<DictItem>(TechAdviceDict.Where(x => !DSTCode.TechCellAdvices.Contains(x.dictKey)));
                }
            }
        }
    }
}
