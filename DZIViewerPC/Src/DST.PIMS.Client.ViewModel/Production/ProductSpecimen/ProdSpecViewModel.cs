using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DST.PIMS.Client.ViewModel
{
    public class ProdSpecViewModel : CustomBaseViewModel
    {
        private static readonly SemaphoreSlim _locker = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim _locker2 = new SemaphoreSlim(1, 1);
        #region 属性
        public QueryPathTitleViewModel QueryPathTitleVM { get; set; } = new QueryPathTitleViewModel();
        /// <summary>
        /// 进度条
        /// </summary>
        [Notification]
        public double ProcessValue { get; set; }
        /// <summary>
        /// 待包埋数
        /// </summary>
        [Notification]
        public int SliceCount { get; set; }
        /// <summary>
        /// 统计数据
        /// </summary>
        [Notification]
        public SliceStatistics Statistics { get; set; }
        /// <summary>
        /// 确认扫码code
        /// </summary>
        [Notification]
        public string ScanCodeStr { get; set; }
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
        /// 组织实体
        /// </summary>
        [Notification]
        public SampTissModel SampTiss { get; set; } = new SampTissModel();
        /// <summary>
        /// 特检配置集合
        /// </summary>
        [Notification]
        public List<SpecStainSetting> SpecStainSetList { get; set; } = new List<SpecStainSetting>();
        /// <summary>
        /// 附言字典
        /// </summary>
        public List<DictItem> PostscriptDict => ExtendApiDict.Instance.PostscriptDict;
        /// <summary>
        /// 打印配置
        /// </summary>
        public PrintSetting Setting { get; set; }
        #endregion 属性

        /// <summary>
        /// 查询制片信息
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand<SampleModel>>(() => new RelayCommand<SampleModel>(async data =>
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (!string.IsNullOrEmpty(data?.SampleID))
                {
                    SelectedSamp = data;
                    //Dispatcher.CurrentDispatcher.InvokeAsync(() => ScanCodeStr = null).Task.NoWarning();
                    ScanCodeStr = null;
                    QueryPathTitleVM.PathModel = SelectedSamp;
                    var sampTissTask = Task.Run(() => SampTissMaterService.Instance.GetSampleTissue(SelectedSamp));
                    var sliceListTask = Task.Run(() => ProductService.Instance.GetProdSliceList(SelectedSamp));
                    var statisTask = Task.Run(() => ProductService.Instance.GetSliceStatistics(SelectedSamp));
                    var specStainListTask = Task.Run(() => ProductService.Instance.GetSpecStainSetList(SelectedSamp));
                    await Task.WhenAll(sampTissTask, sliceListTask, statisTask, specStainListTask).ConfigureAwait(false);
                    SampTiss = sampTissTask.Result;
                    Statistics = statisTask.Result;
                    SpecStainSetList = specStainListTask.Result;
                    UpdateSilceList(sliceListTask.Result); // 更新包埋盒数据
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }

        })).Value;
        /// <summary>
        /// 扫码打印
        /// </summary>
        public ICommand ScanCodeCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(ScanCodeStr))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var tmpCode = ScanCodeStr;
                    ScanCodeStr = null;
                    var result = await Task.Run(() => ProductService.Instance.ScanCodeGeneSlice(tmpCode)); ; // 返回切片打印实体SlicePrintCode
                    if (!string.IsNullOrEmpty(result?.SampleID))
                    {
                        Task.Run(() => PrintCode(new List<SlicePrintCode> { result })).NoWarning(); // 打印
                        await ChangeSelect(result?.SampleID);
                    }
                    //else
                    //{
                    //    ShowMessageBox($"扫码确认失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;

        /// <summary>
        /// 新增切片（特殊染色）
        /// 
        /// </summary>
        public ICommand AddCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
            {
                var msg = new ShowContentWindowMessage("ProdSpec_Add", $"新增制片");
                msg.DesignWidth = 500;
                msg.DesignHeight = 350;
                msg.Args = new object[] { SelectedSamp, SpecStainSetList };
                msg.CallBackCommand = new RelayCommand<SlicePrintCode>(async res =>
                {
                    if (res != null)
                    {
                        // 打印
                        Task.Run(() => PrintCode(new List<SlicePrintCode> { res })).NoWarning();
                        // 更新样本列表 和 包埋盒相关数据
                        await ChangeSelect();
                    }
                });
                Messenger.Default.Send(msg);
            }
        })).Value;
        /// <summary>
        /// 编辑切片
        /// </summary>
        public ICommand EditCommand => new Lazy<RelayCommand<SliceModel>>(() => new RelayCommand<SliceModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                var msg = new ShowContentWindowMessage("ProdSpec_Edit", $"编辑制片信息");
                msg.DesignWidth = 400;
                msg.DesignHeight = 250;
                msg.Args = new object[] { data.DeepCopy() };
                msg.CallBackCommand = new RelayCommand<bool>(async res =>
                {
                    if (res)
                    {
                        // 更新样本列表 和 包埋盒相关数据
                        await ChangeSelect();
                    }
                });
                Messenger.Default.Send(msg);
            }
        })).Value;
        /// <summary>
        /// 编辑切片
        /// </summary>
        public ICommand DeleteCommand => new Lazy<RelayCommand<SliceModel>>(() => new RelayCommand<SliceModel>(data =>
        {
            ShowMessageBox("是否需要删除该切片？", MessageBoxButton.OKCancel, MessageBoxImage.Question, async res =>
             {
                 if (res == MessageBoxResult.OK)
                 {
                     if (!string.IsNullOrEmpty(data?.ID))
                     {
                         try
                         {
                             WhirlingControlManager.ShowWaitingForm();
                             var result = await Task.Run(() => ProductService.Instance.DeleteSlice(data));
                             if (result)
                             {
                                 await ChangeSelect();
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
        /// 打印
        /// </summary>
        public ICommand PrintSliceCodeCommand => new Lazy<RelayCommand<SliceModel>>(() => new RelayCommand<SliceModel>(async data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                var result = await Task.Run(() => ProductService.Instance.PrintSliceCodeList(new List<SliceModel>() { data }));
                if (result?.Count > 0)
                {
                    //result.ForEach(r => r.ProductID = SelectedSamp?.ProductID); // 填充productid
                    Task.Run(() => PrintCode(result)).NoWarning();
                }
            }
        })).Value;

        /// <summary>
        /// 扫码后，通知其他viewmodel更新选中样本
        /// </summary>
        public ICommand ChangeSelectCommand { get; set; }

        public ProdSpecViewModel()
        {
            Setting = PrintSetHelper.GetPrintSetting(IniSectionConst.ProductionSection);
            this.RegisterMessenger();
        }

        private void RegisterMessenger()
        {
            // 扫码确认
            Messenger.Default.Register<string>(this, EnumMessageKey.ScanProdConfirm, async data =>
            {
                if (Setting?.IsNoFocus ?? false && !string.IsNullOrEmpty(data))
                {
                    await _locker2.WaitAsync();
                    ScanCodeStr = data;
                    ScanCodeCommand.Execute(null);
                    _locker2.Release();
                }
            });
        }
        /// <summary>
        /// 更新统计数据 后 触发命令（否则，无法更新样本的ststus）
        /// </summary>
        private async Task ChangeSelect(string sampleID = null)
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    await Task.Run(() => ProductService.Instance.GetSliceStatistics(SelectedSamp)); // 为了更新样本状态
                    ChangeSelectCommand?.Execute(sampleID ?? SelectedSamp?.SampleID);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        }
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

        private void PrintCode(List<SlicePrintCode> printCodeList)
        {
            //TSCPrintManager.Instance.Print(printCodeList);
            PrintLabelManager.Singleton.Print(printCodeList);
        }
        /// <summary>
        /// 手动输入码后回车确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ScanCode_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.ScanCodeCommand.Execute(null);
            }
        }
    }
}
