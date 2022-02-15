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
    public class ProdSpec_AddViewModel : CustomBaseViewModel
    {
        private InspSpecimen _selectedInspSpec;
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
        /// 试剂类型字典（标记物/染色剂）
        /// </summary>
        [Notification]
        public List<DictItem> ProdReagentDict { get; set; } = new List<DictItem>();
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
        public EmbedBoxModel SelectedEmbedBox { get; set; }
        /// <summary>
        /// 样本
        /// </summary>
        [Notification]
        public SampleModel Sample { get; set; }
        /// <summary>
        /// 特殊染色新增切片实体
        /// </summary>
        [Notification]
        public QueryAddSlice QuerySlice { get; set; } = new QueryAddSlice();

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
                SlicePrintCode slicePrintCode = null;
                WhirlingControlManager.ShowWaitingForm();
                try
                {
                    slicePrintCode = ProductService.Instance.ScanCodeGeneSliceSpec(QuerySlice); // 生成制片
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }

                if (slicePrintCode != null)
                {
                    this.Result = slicePrintCode;
                    this.CloseContentWindow();
                }
                else
                {
                    ShowMessageBox("新增切片失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        })).Value;


        public async override void OnViewLoaded()
        {
            if (this.Args != null && this.Args.Length == 2 && this.Args[0] is SampleModel samp && this.Args[1] is List<SpecStainSetting> speStaSetList)
            {
                if (!string.IsNullOrEmpty(samp?.SampleID))
                {
                    WhirlingControlManager.ShowWaitingForm();
                    Sample = samp;
                    // 从标记物染色剂字配置项目，生成动态字典
                    var marks = speStaSetList.FirstOrDefault(x => x.GroupKey == Sample.Markers)?.GroupKey?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?.ToList();
                    if (marks != null)
                    {
                        ProdReagentDict = ExtendApiDict.Instance.ProdReagentDict.Where(x => marks.Contains(x.dictKey))?.ToList();
                    }
                    try
                    {
                        var inSpecTask = Task.Run(() => SampTissMaterService.Instance.GetInspSpecList(Sample));
                        var embedListTask = Task.Run(() => SampTissMaterService.Instance.GetEmbedList(Sample));
                        await Task.WhenAll(inSpecTask, embedListTask).ConfigureAwait(false);

                        EmbedList = embedListTask.Result;
                        InSpecList = inSpecTask.Result;
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            }
        }
    }
}
