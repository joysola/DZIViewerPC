using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls;
using DST.Database.Model;
using DST.Database.Model.DictModel;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DST.PIMS.Client.ViewModel
{
    public class EmbedSpecimenViewModel : CustomBaseViewModel
    {
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
        public int UnEmbedCount { get; set; }
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
        /// 包埋盒列表
        /// </summary>
        [Notification]
        public ObservableCollection<EmbedBoxModel> EmbedBoxList { get; set; } = new ObservableCollection<EmbedBoxModel>();
        /// <summary>
        /// 组织实体
        /// </summary>
        [Notification]
        public SampTissModel SampTiss { get; set; } = new SampTissModel();
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
        /// 查询包埋信息
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
                    var sampTissTask = Task.Run(() => SampTissMaterService.Instance.GetSampleTissue(data));
                    var embedListTask = Task.Run(() => SampTissMaterService.Instance.GetEmbedList(SelectedSamp));
                    await Task.WhenAll(sampTissTask, embedListTask).ConfigureAwait(false);
                    SampTiss = sampTissTask.Result;
                    UpdateEmbedBoxList(embedListTask.Result); // 更新包埋盒数据
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
            var tmpCode = ScanCodeStr;
            if (!string.IsNullOrEmpty(tmpCode))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = await Task.Run(() => EmbedService.Instance.UpdateEmbedStatus(tmpCode)); // 返回sampleid
                    if (!string.IsNullOrEmpty(result))
                    {
                        ChangeSelectCommand?.Execute(result); // 通知样本列表选择对应样本
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
        /// 一键确认
        /// </summary>
        public ICommand AutoScanAllCodesCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (EmbedBoxList?.Count > 0)
            {
                var uncheckList = EmbedBoxList.Where(x => x.EmbedStatus != "1").ToList(); // 未确认的进行处理
                uncheckList.ForEach(box =>
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        EmbedService.Instance.UpdateEmbedStatus(box.WaxBlockCode);
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                });
                var result = uncheckList.FirstOrDefault();
                if (!string.IsNullOrEmpty(result?.SampleId))
                {
                    ChangeSelectCommand.Execute(result.SampleId);
                }
            }
        })).Value;
        /// <summary>
        /// 扫码后，通知其他viewmodel更新选中样本
        /// </summary>
        public ICommand ChangeSelectCommand { get; set; }




        public EmbedSpecimenViewModel()
        {
            Setting = PrintSetHelper.GetPrintSetting(IniSectionConst.EmbedSection);
            this.RegisterMessenger();
        }


        private void RegisterMessenger()
        {
            // 扫码确认
            Messenger.Default.Register<string>(this, EnumMessageKey.ScanEmbedConfirm, data =>
            {
                if (Setting?.IsNoFocus ?? false && !string.IsNullOrEmpty(data))
                {
                    ScanCodeStr = data;
                    ScanCodeCommand.Execute(null);
                }
            });
        }

        /// <summary>
        /// 获取包埋盒列表
        /// </summary>
        private void GetEmbedBoxList()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
                {
                    var list = SampTissMaterService.Instance.GetEmbedList(SelectedSamp);
                    UpdateEmbedBoxList(list);
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
        /// <summary>
        /// 更新包埋盒列表
        /// </summary>
        /// <param name="embedBoxes"></param>
        private void UpdateEmbedBoxList(List<EmbedBoxModel> embedBoxes)
        {
            if (embedBoxes != null)
            {
                // 更新包埋盒列表
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EmbedBoxList.Clear();
                    embedBoxes?.ForEach(box =>
                    {
                        EmbedBoxList.Add(box);
                    });
                });
                // 更新进度条
                Application.Current.Dispatcher.InvokeAsync(() => ProcessValue = CalculateProcessValue());
                Application.Current.Dispatcher.InvokeAsync(() => UnEmbedCount = CalculateUnEmbedCount());
            }
        }
        /// <summary>
        /// 刷新进度条
        /// </summary>
        /// <returns></returns>
        private double CalculateProcessValue()
        {
            var processValue = 0.0;
            if (EmbedBoxList.Count > 0)
            {
                foreach (var item in EmbedBoxList)
                {
                    if (item.EmbedStatus == "1") // 包埋已完成
                    {
                        processValue++;
                    }
                }
                processValue = processValue / EmbedBoxList.Count;
            }
            return processValue * 100;
        }
        /// <summary>
        /// 计算未包埋数
        /// </summary>
        /// <returns></returns>
        private int CalculateUnEmbedCount()
        {
            var unEmbedList = EmbedBoxList?.Where(x => x.EmbedStatus == "0").ToList();
            return unEmbedList?.Count ?? 0;
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
