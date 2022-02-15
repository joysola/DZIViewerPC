using DST.ApiClient.Service;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.Controls;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Nico.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    [NotifyAspect]
    public class ImgViewerDetailViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 阅片数据源
        /// </summary>
        public AllocSampDetail ImgViewDetail { get; set; }
        public ICommand ChangeSelectedCutScan { get; set; }
        /// <summary>
        /// 定位
        /// </summary>
        public ICommand RprtLocateCommand => new Lazy<RelayCommand<ReportImg>>(() => new RelayCommand<ReportImg>(data =>
        {
            if (data != null)
            {
                try
                {
                    var cutScan = ImgViewDetail?.CutScanVOList?.FirstOrDefault(x => x.SliceNum == data.SliceNum);
                    if (cutScan != null)
                    {
                        ChangeSelectedCutScan?.Execute(cutScan);
                        var imgCrds = data.ImgCoordinate?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (imgCrds?.Length == 2 && decimal.TryParse(imgCrds[0], out decimal x) && decimal.TryParse(imgCrds[1], out decimal y))
                        {
                            var msiPoint = new Point((double)DSTReviewScanImg.GetPixelX(x), (double)DSTReviewScanImg.GetPixelY(y));
                            Messenger.Default.Send(msiPoint, EnumMessageKey.LocateRptImgViewer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("报告视野定位错误！", ex);
                }
            }
        })).Value;

        /// <summary>
        /// 删除
        /// </summary>
        public ICommand RprtRemoveCommand => new Lazy<RelayCommand<ReportImg>>(() => new RelayCommand<ReportImg>(data =>
        {
            if (data != null)
            {
                ShowMessageBox("是否要删除该报告视野？", MessageBoxButton.OKCancel, MessageBoxImage.Question, async res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            var result = await Task.Run(() => AllocationService.Instance.RemoveReportMainImg(data));
                            if (result)
                            {
                                ImgViewDetail.ReportImgList.Remove(data);
                            }
                        }
                        finally
                        {
                            WhirlingControlManager.CloseWaitingForm();
                        }
                    }
                });
            }
        })).Value;
    }
}
