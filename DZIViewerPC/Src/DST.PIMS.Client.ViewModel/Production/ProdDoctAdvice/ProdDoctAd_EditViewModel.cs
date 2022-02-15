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
    public class ProdDoctAd_EditViewModel : CustomBaseViewModel
    {
        private InSpecInfo _selectedInspSpec;
        private EmbedBoxModel _selectedEmbedbox;
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
        public ObservableCollection<InSpecInfo> InSpecDict { get; set; } = new ObservableCollection<InSpecInfo>();

        /// <summary>
        /// 试剂类型字典（标记物/染色剂）
        /// </summary>
        public List<DictItem> ProdReagentDict => ExtendApiDict.Instance.ProdReagentDict;
        /// <summary>
        /// 选中的部位
        /// </summary>
        [Notification]
        public InSpecInfo SelectedInspSpec
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
        public EmbedBoxModel SelectedEmbedbox
        {
            get => _selectedEmbedbox;
            set
            {
                _selectedEmbedbox = value;
                if (!string.IsNullOrEmpty(_selectedEmbedbox?.ID))
                {
                    Slice.DrawMaterPlace = _selectedEmbedbox.DrawMaterPlace;
                    Slice.DrawMaterialsID = _selectedEmbedbox.ID;
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
                        ShowMessageBox("保存特检医嘱切片失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        })).Value;


        public override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 2 && this.Args[0] is SliceModel slice && this.Args[1] is List<EmbedBoxModel> embeds)
            {
                Slice = slice;
                EmbedList = embeds;
                InSpecDict.Clear();
                EmbedList.ForEach(e =>
                {
                    var newSpec = InSpecDict.FirstOrDefault(x => x.ID == e.SampSpecID);
                    if (newSpec == null)
                    {
                        InSpecDict.Add(new InSpecInfo { ID = e.SampSpecID, Name = e.Name });
                    }
                });
            }
        }
        /// <summary>
        /// 取材部位
        /// </summary>
        public class InSpecInfo
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }
    }
}
