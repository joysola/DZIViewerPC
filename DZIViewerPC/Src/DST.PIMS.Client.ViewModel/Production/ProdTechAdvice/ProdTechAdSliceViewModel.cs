using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
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
    public class ProdTechAdSliceViewModel : CustomBaseViewModel
    {
        public QueryPathTitleViewModel QueryPathTitleVM { get; set; } = new QueryPathTitleViewModel();
        /// <summary>
        /// 是否是细胞类型
        /// </summary>
        [Notification]
        public bool IsCell { get; set; }
        /// <summary>
        /// 选中的样本
        /// </summary>
        [Notification]
        public SampleModel SelectedSamp { get; set; }
        /// <summary>
        /// 切片列表
        /// </summary>
        [Notification]
        public ObservableCollection<SliceModel> SliceList { get; set; } = new ObservableCollection<SliceModel>();
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
        /// 医嘱类型 1  重切 2 深切 3 薄切 4 多切 5 连切 6 补切 7 重新扫描 8 重新制片
        /// </summary>
        public List<DictItem> TechAdviceDict => ExtendApiDict.Instance.TechAdviceDict;
        /// <summary>
        /// 查询制片信息
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand<SampleModel>>(() => new RelayCommand<SampleModel>(async data =>
        {
            if (!string.IsNullOrEmpty(data?.SampleID))
            {
                SelectedSamp = data;
                //var specialList = ExtendApiDict.Instance.ProductDict.Where(x => DSTCode.TechAdCells.Contains(x.code)).ToList();
                IsCell = DSTCode.TechAdCellProdIDList.Exists(x => x == SelectedSamp.ProductID);
                QueryPathTitleVM.PathModel = SelectedSamp;
                WhirlingControlManager.ShowWaitingForm();
                try
                {
                    var inSpecTask = Task.Run(() => SampTissMaterService.Instance.GetInspSpecList(SelectedSamp));
                    var embedListTask = Task.Run(() => SampTissMaterService.Instance.GetEmbedList(SelectedSamp));
                    var sliceTask = Task.Run(() => ProductService.Instance.GetTechAdviSliceList(SelectedSamp));
                    await Task.WhenAll(inSpecTask, embedListTask, sliceTask).ConfigureAwait(false);
                    InSpecList = inSpecTask.Result;
                    EmbedList = embedListTask.Result;
                    UpdateSilceList(sliceTask.Result);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;


        /// <summary>
        /// 新增切片（特殊染色）
        /// </summary>
        public ICommand AddSliceCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
            {
                AddorEditTechAddSlice("新增", new SliceModel { SampleID = SelectedSamp?.SampleID }, true);
            }
        })).Value;

        /// <summary>
        /// 编辑切片
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand<SliceModel>>(() => new RelayCommand<SliceModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                AddorEditTechAddSlice("编辑", data.DeepCopy(), false);
            }
        })).Value;


        /// <summary>
        /// 编辑切片
        /// </summary>
        public ICommand DeleteCommand => new Lazy<RelayCommand<SliceModel>>(() => new RelayCommand<SliceModel>(data =>
        {
            ShowMessageBox("是否需要删除该切片？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    if (!string.IsNullOrEmpty(data?.ID))
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            var result = ProductService.Instance.DeleteSlice(data);
                            if (result)
                            {
                                ChangeSelectCommand?.Execute(SelectedSamp?.SampleID);
                            }
                            else
                            {
                                ShowMessageBox($"删除切片失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        finally
                        {
                            WhirlingControlManager.CloseWaitingForm();
                        }
                    }
                }
            });
        })).Value;
        /// <summary>
        /// 打印所有
        /// </summary>
        public ICommand PrintAllCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            PintSliceList(SliceList).NoWarning();
        })).Value;
        /// <summary>
        /// 打印选中项
        /// </summary>
        public ICommand PrintSelectCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            PintSliceList(SliceList.Where(x => x.IsSelected)).NoWarning();
        })).Value;
        /// <summary>
        /// 更新后，通知其他viewmodel更新选中样本
        /// </summary>
        public ICommand ChangeSelectCommand { get; set; }



        /// <summary>
        /// 更新切片列表
        /// </summary>
        /// <param name="slices"></param>
        private void UpdateSilceList(List<SliceModel> slices)
        {
            if (slices != null)
            {
                // 更新包埋盒列表
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SliceList.Clear();
                    slices?.ForEach(box =>
                    {
                        SliceList.Add(box);
                    });
                });

            }
        }

        /// <summary>
        /// 新增或编辑技术医嘱切片
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="slice">切片</param>
        /// <param name="isAdd">是否是编辑</param>
        private void AddorEditTechAddSlice(string title, SliceModel slice, bool isAdd)
        {
            if (!string.IsNullOrEmpty(slice?.SampleID))
            {
                var msg = new ShowContentWindowMessage("ProdTechAd_Edit", $"{title}技术医嘱制片信息");
                msg.DesignWidth = 500;
                msg.DesignHeight = 350;
                msg.Args = new object[] { slice, EmbedList, InSpecList, isAdd, IsCell };
                msg.CallBackCommand = new RelayCommand<SliceModel>(res =>
                {
                    if (!string.IsNullOrEmpty(res?.ID))
                    {
                        // res.ProductID = SelectedSamp?.ProductID;
                        PintSliceList(new List<SliceModel> { res }).NoWarning();
                        // 更新样本列表 和 包埋盒相关数据
                        // ChangeSelectCommand?.Execute(SelectedSamp?.SampleID);
                    }
                });
                Messenger.Default.Send(msg);
            }
        }
        /// <summary>
        /// 打印切片
        /// </summary>
        /// <param name="slices"></param>
        private async Task PintSliceList(IEnumerable<SliceModel> slices)
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = await Task.Run(() => ProductService.Instance.GetTechAdSliceCodeList(slices, SelectedSamp));
                    if (result?.Count > 0)
                    {
                        //result.ForEach(r => r.ProductID = SelectedSamp?.ProductID);
                        Task.Run(() => PrintCode(result)).NoWarning();
                        ChangeSelectCommand?.Execute(SelectedSamp?.SampleID);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printCodeList"></param>
        private void PrintCode(List<SlicePrintCode> printCodeList)
        {
            //TSCPrintManager.Instance.Print(printCodeList);
            PrintLabelManager.Singleton.Print(printCodeList);
        }
    }
}
