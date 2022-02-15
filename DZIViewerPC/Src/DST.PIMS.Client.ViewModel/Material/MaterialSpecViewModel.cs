using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls;
using DST.Controls.Base;
using DST.Database;
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
    public class MaterialSpecViewModel : CustomBaseViewModel
    {
        public MaterialTissImgViewModel MaterialTissImgVM { get; set; } = new MaterialTissImgViewModel();
        private static readonly SemaphoreSlim locker = new SemaphoreSlim(1, 1);
        /// <summary>
        /// 连打数量
        /// </summary>
        [Notification]
        public int ComboNum { get; set; } = 1;
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
        /// 选中的标本部位
        /// </summary>
        [Notification]
        public InspSpecimen SelectedSpec { get; set; } = new InspSpecimen();
        /// <summary>
        /// 选中的样本
        /// </summary>
        [Notification]
        public SampleModel SelectedSamp { get; set; }
        /// <summary>
        /// 送检部位集合
        /// </summary>
        [Notification]
        public List<InspSpecimen> SpecList { get; set; } = new List<InspSpecimen>();

        [Notification]
        public List<EmbedPrintCode> EmbedPrintList { get; set; } = new List<EmbedPrintCode>();
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
        /// 取材类型
        /// </summary>
        [Notification]
        public EnumMaterialType MaterialType { get; set; }
        /// <summary>
        /// 打印配置
        /// </summary>
        public PrintSetting Setting { get; set; }
        #region 字典
        /// <summary>
        /// 附言字典
        /// </summary>
        public List<DictItem> PostscriptDict => ExtendApiDict.Instance.PostscriptDict;
        /// <summary>
        /// 登录的用户
        /// </summary>
        public LoginModel CurUser => ExtendAppContext.Current.LoginModel;
        #endregion 字典

        #region 命令
        /// <summary>
        /// 查询组织和部位
        /// </summary>
        public ICommand QueryAllCommand => new Lazy<RelayCommand<SampleModel>>(() => new RelayCommand<SampleModel>(async data =>
        {
            WhirlingControlManager.ShowWaitingForm();
            //await locker.WaitAsync();
            try
            {
                if (!string.IsNullOrEmpty(data?.SampleID))
                {
                    SelectedSamp = data;
                    ScanCodeStr = null;
                    var imgTask = Application.Current.Dispatcher.InvokeAsync(() => MaterialTissImgVM.QueryCommand.Execute(SelectedSamp)).Task;
                    var sampTissTask = Task.Run(() => SampTissMaterService.Instance.GetSampleTissue(data));
                    var specTask = Task.Run(() => SampTissMaterService.Instance.GetInspSpecList(data));
                    var embedTask = Task.Run(() => SampTissMaterService.Instance.GetEmbedList(data));
                    await Task.WhenAll(sampTissTask, specTask, embedTask, imgTask).ConfigureAwait(false);
                    // 查询组织大体图像
                    SampTiss = sampTissTask.Result;
                    if (string.IsNullOrEmpty(SampTiss?.SampleID))
                    {
                        SampTiss.SampleID = SelectedSamp.SampleID;
                    }
                    SpecList = specTask.Result;
                    UpdateEmbedBoxList(embedTask.Result); // 更新包埋盒

                }
            }
            finally
            {
                //locker.Release();
                WhirlingControlManager.CloseWaitingForm();
            }
        })).Value;
        /// <summary>
        /// 打号
        /// </summary>
        public ICommand PrintCodeCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = await Task.Run(() => MaterialService.Instance.PrintCodebySampID(SelectedSamp));
                    // 连接打印机打码
                    if (result?.Count > 0)
                    {
                        await GetEmbedBoxList();
                        Task.Run(() => PrintCodes(result)).NoWarning();
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;
        /// <summary>
        /// 增加
        /// </summary>
        public ICommand AddCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID) && !string.IsNullOrEmpty(SelectedSpec?.ID))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var newEmbedBox = new EmbedBoxModel
                    {
                        SampSpecID = SelectedSpec.ID,
                        SampleId = SelectedSamp.SampleID,
                    };
                    var result = MaterialService.Instance.AddEmBedBoxInfo(ComboNum, newEmbedBox);
                    // 打码
                    if (result.Count > 0)
                    {
                        Task.Run(() => PrintCodes(result));
                        //GetEmbedBoxList();
                        ChangeSelectCommand.Execute(SelectedSamp?.SampleID);
                    }
                    else
                    {
                        ShowMessageBox($"新增包埋盒数据失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }

        })).Value;





        /// <summary>
        /// 编辑包埋盒
        /// </summary>
        public ICommand EditBoxCommand => new Lazy<RelayCommand<EmbedBoxModel>>(() => new RelayCommand<EmbedBoxModel>(data =>
        {
            if (data != null)
            {
                var msg = new ShowContentWindowMessage("Material_Edit", "编辑包埋盒");
                msg.DesignWidth = 400;
                msg.DesignHeight = 350;
                msg.Args = new object[] { data.DeepCopy() };
                msg.CallBackCommand = new RelayCommand<EmbedBoxModel>(async res =>
                {
                    if (res != null)
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            var result = false;
                            result = await Task.Run(() => MaterialService.Instance.SaveEmbedBoxInfo(res));
                            if (result)
                            {
                                await GetEmbedBoxList();
                            }
                            else
                            {
                                ShowMessageBox($"更新包埋盒数据失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        finally
                        {
                            WhirlingControlManager.CloseWaitingForm();
                        }
                    }
                });
                Messenger.Default.Send(msg);
            }
        })).Value;

        /// <summary>
        /// 删除包埋盒
        /// </summary>
        public ICommand DeleteBoxCommand => new Lazy<RelayCommand<EmbedBoxModel>>(() => new RelayCommand<EmbedBoxModel>(data =>
        {
            if (data != null)
            {
                ShowMessageBox("确定要删除此包埋盒吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, async res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            var result = await Task.Run(() => MaterialService.Instance.DeleteEmbedInfo(data)); ;
                            if (result)
                            {
                                //GetEmbedBoxList();
                                ChangeSelectCommand.Execute(data.SampleId);
                            }
                            else
                            {
                                ShowMessageBox($"删除包埋盒数据失败！", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// <summary>
        /// 打印包埋盒
        /// </summary>
        public ICommand PrintBoxCommand => new Lazy<RelayCommand<EmbedBoxModel>>(() => new RelayCommand<EmbedBoxModel>(async data =>
        {
            if (data != null)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = await Task.Run(() => MaterialService.Instance.PrintEmbedBoxList(data));
                    Task.Run(() => PrintCodes(result)).NoWarning();
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;

        /// <summary>
        /// 测试扫码打印
        /// </summary>
        public ICommand ScanCodeCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(ScanCodeStr))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = await Task.Run(() => MaterialService.Instance.UpdateMaterialStatus(ScanCodeStr));
                    if (!string.IsNullOrEmpty(result))
                    {
                        //GetEmbedBoxList();
                        ChangeSelectCommand.Execute(result);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
                //else
                //{
                //    ShowMessageBox($"扫码确认失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
            }
        })).Value;
        /// <summary>
        /// 肉眼所见
        /// </summary>
        public ICommand ChangeSampTissEyeCommand => new Lazy<RelayCommand<ComWordModel>>(() => new RelayCommand<ComWordModel>(data =>
        {
            if (data != null)
            {
                SampTiss.NakedEyes += data.Content;
            }
        })).Value;

        /// <summary>
        /// 取材延迟原因查看
        /// </summary>
        public ICommand MaterDelayCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var msg = new ShowContentWindowMessage("MaterialDelay", $"延迟取材");
                    msg.DesignWidth = 450;
                    msg.DesignHeight = 300;
                    var delayInfo = await Task.Run(() => MaterialService.Instance.GetSampTissDelayInfo(SelectedSamp)); // 获取存在的延迟信息
                    msg.Args = new object[] { delayInfo, false };
                    Messenger.Default.Send(msg);
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;

        /// <summary>
        /// 标本信息维护
        /// </summary>
        public ICommand MaintainCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
            {
                var msg = new ShowContentWindowMessage("Material_SpecEdit", $"标本信息维护");
                msg.DesignWidth = 600;
                msg.DesignHeight = 500;
                var args = new SampSpecDetail
                {
                    InspecSampleName = SelectedSamp?.InspectSample,
                    SampleId = SelectedSamp?.SampleID,
                    ProductID = SelectedSamp?.ProductID,
                    SampStatus = SelectedSamp?.Status
                };
                SpecList?.ForEach(sp => args.SampSpecList.Add(sp.DeepCopy()));
                msg.Args = new object[] { args };
                msg.CallBackCommand = new RelayCommand<bool>(res =>
                {
                    if (res)
                    {
                        // 更新样本列表 和 包埋盒相关数据
                        //QueryAllCommand.Execute(SelectedSamp);
                        QueryPathList?.Execute(null);
                    }
                });
                Messenger.Default.Send(msg);
            }
        })).Value;
        /// <summary>
        /// 一键确认
        /// </summary>
        public ICommand AutoScanAllCodesCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (EmbedBoxList?.Count > 0)
            {
                var uncheckList = EmbedBoxList.Where(x => x.DrawMaterStatus != "1").ToList(); // 未确认的进行处理
                uncheckList.ForEach(box =>
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        MaterialService.Instance.UpdateMaterialStatus(box.WaxBlockCode);
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                });
                var result = uncheckList.FirstOrDefault();
                if (!string.IsNullOrEmpty(result?.SampleId))
                {
                    //GetEmbedBoxList();
                    ChangeSelectCommand.Execute(result.SampleId);
                }
            }
        })).Value;
        /// <summary>
        /// 查询样本列表
        /// </summary>
        public ICommand QueryPathList { get; set; }
        /// <summary>
        /// 扫码后，通知其他viewmodel更新选中样本
        /// </summary>
        public ICommand ChangeSelectCommand { get; set; }
        #endregion 命令



        public MaterialSpecViewModel()
        {
            Setting = PrintSetHelper.GetPrintSetting(IniSectionConst.MaterialSection);
            this.RegisterMessenger();
        }

        private void RegisterMessenger()
        {
            // 扫码确认
            Messenger.Default.Register<string>(this, EnumMessageKey.ScanMaterialConfirm, data =>
            {
                if (Setting?.IsNoFocus ?? false && !string.IsNullOrEmpty(data))
                {
                    ScanCodeStr = data;
                    ScanCodeCommand.Execute(null);
                }
            });
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
        /// 获取包埋盒列表
        /// </summary>
        private async Task GetEmbedBoxList()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
                {
                    var list = await Task.Run(() => SampTissMaterService.Instance.GetEmbedList(SelectedSamp));
                    UpdateEmbedBoxList(list);
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
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
                    if (item.DrawMaterStatus == "1")
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
        /// 打印包埋盒
        /// </summary>
        /// <param name="printCodes"></param>
        private void PrintCodes(List<EmbedPrintCode> printCodes)
        {
            PrintLabelManager.Singleton.Print(printCodes);
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
