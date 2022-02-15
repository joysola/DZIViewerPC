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
    public class ProdDoctAdSliceViewModel : CustomBaseViewModel
    {
        public QueryPathTitleViewModel QueryPathTitleVM { get; set; } = new QueryPathTitleViewModel();

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
        /// 查询制片信息
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand<SampleModel>>(() => new RelayCommand<SampleModel>(async data =>
        {
            if (!string.IsNullOrEmpty(data?.SampleID))
            {
                SelectedSamp = data;

                QueryPathTitleVM.PathModel = SelectedSamp;
                WhirlingControlManager.ShowWaitingForm();
                try
                {
                    var embedTask = Task.Run(() => ProductService.Instance.GetEmbedBoxListbyPathID(SelectedSamp));
                    var sliceTask = Task.Run(() => ProductService.Instance.GetSliceList(SelectedSamp));
                    await Task.WhenAll(embedTask, sliceTask).ConfigureAwait(false);
                    EmbedList = embedTask.Result;
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
                var advice = ExtendApiDict.Instance.DoctAdviceDict?.FirstOrDefault(x => x.dictValue.Contains("免疫组化"));
                AddorEditDoctAddSlice("新增", new SliceModel { SampleID = SelectedSamp?.SampleID, AdviceType = advice?.dictKey }, true);
            }
        })).Value;

        /// <summary>
        /// 编辑切片
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand<SliceModel>>(() => new RelayCommand<SliceModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                AddorEditDoctAddSlice("编辑", data.DeepCopy(), false);
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
        private void AddorEditDoctAddSlice(string title, SliceModel slice, bool isAdd)
        {
            if (!string.IsNullOrEmpty(slice?.SampleID))
            {
                var msg = new ShowContentWindowMessage("ProdDoctAd_Edit", $"{title}特检医嘱制片信息");
                msg.DesignWidth = 500;
                msg.DesignHeight = 350;
                msg.Args = new object[] { slice, EmbedList };
                msg.CallBackCommand = new RelayCommand<SliceModel>(res =>
                {
                    if (!string.IsNullOrEmpty(res?.ID))
                    {
                        PintSliceList(new List<SliceModel> { res }).NoWarning();
                        // 更新样本列表 和 包埋盒相关数据
                        //ChangeSelectCommand?.Execute(SelectedSamp?.SampleID);
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
                    var result = await Task.Run(() => ProductService.Instance.GetDoctAdSliceCodeList(slices, SelectedSamp));
                    if (result?.Count > 0)
                    {
                        //result?.ForEach(s => s.ProductID = SelectedSamp?.ProductID);
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
