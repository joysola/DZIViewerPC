using DST.Database.Model;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    [NotifyAspect]
    public class ImgViewerManagerViewModel : CustomBaseViewModel
    {
        private CuttingScan _selectedCutScan;
        /// <summary>
        /// 诊断单viewmodel
        /// </summary>
        public ImgViewerDetailViewModel ImgViewerDetailVM { get; } = new ImgViewerDetailViewModel();
        /// <summary>
        /// 主阅片vewimodel
        /// </summary>
        public ImageViewerViewModel ImgViewModel { get; } = new ImageViewerViewModel();
        /// <summary>
        /// 分屏的第二个阅片viewmodel
        /// </summary>
        public ImageViewerViewModel ImgViewModel2 { get; } = new ImageViewerViewModel();
        /// <summary>
        /// 分屏状态 由 分屏控件提供
        /// </summary>
        public bool IsSpliting { get; set; }
        /// <summary>
        /// 切片路径集合
        /// </summary>
        public List<string> ImgPathList { get; set; }
        /// <summary>
        /// 选中的切片
        /// </summary>
        public CuttingScan SelectedCutScan
        {
            get => _selectedCutScan;
            set
            {
                if (!IsSpliting) // 非分屏状态
                {
                    _selectedCutScan = value;
                    if (_selectedCutScan != null)
                    {
                        var selectScan = ImgViewerDetailVM.ImgViewDetail?.CutScanVOList?.FirstOrDefault(x => x.ScanImgUrl == _selectedCutScan.ScanImgUrl);
                        ImgViewModel.InitScaleTileSource(_selectedCutScan.ScanImgUrl, selectScan);
                        Messenger.Default.Send<object>(null, EnumMessageKey.RefreshImgViewerAnnos); // 刷新标记数据
                    }
                }
            }
        }
        /// <summary>
        /// 选择的分屏 由 分屏控件提供
        /// </summary>
        public ObservableCollection<CuttingScan> SelectedCutScanList { get; set; } = new ObservableCollection<CuttingScan>();

        /// <summary>
        /// 切换切片
        /// </summary>
        public ICommand ChangeSelectedCutScan => new Lazy<RelayCommand<CuttingScan>>(() => new RelayCommand<CuttingScan>(data =>
        {
            if (data != null && data.SliceNum != SelectedCutScan?.SliceNum)
            {
                SelectedCutScan = data;
            }
        })).Value;

        public ImgViewerManagerViewModel()
        {
            ImgViewerDetailVM.ChangeSelectedCutScan = ChangeSelectedCutScan;
        }
        public override void OnViewLoaded()
        {
            if (this.Args?.Length == 3
                && this.Args[0] is List<string> imgPaths && imgPaths.Count > 0
                && this.Args[1] is bool autoTest
                && this.Args[2] is AllocSampDetail allocSampDetail && allocSampDetail.CutScanVOList?.Count > 0)
            {
                ImgPathList = imgPaths;
                var firstImgPath = imgPaths[0];
                var firstCutScan = allocSampDetail?.CutScanVOList.FirstOrDefault(x => x.ScanImgUrl == firstImgPath);
                ImgViewModel.InitScaleTileSource(imgPaths[0], firstCutScan);
                //ImgViewModel.ImageBasePath = imgBasePath;
                //ImgViewModel.CuttingScans = allocSampDetail?.CutScanVOList;
                ImgViewerDetailVM.ImgViewDetail = allocSampDetail;
                //CutScanList = allocSampDetail.CutScanVOList;
                if (autoTest)
                {
                    Task.Run(() =>
                    {
                        while (ImgViewModel.CurTileSource == null || !ImgViewModel.CurTileSource.HasLoadFinish)
                        {
                            System.Threading.Thread.Sleep(10 * 1000);
                        }
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            System.Threading.Thread.Sleep(10 * 1000);
                            this.CloseContentWindow();
                        });
                    });
                }
            }
        }
    }
}
