using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class MaterialMainViewModel : CustomBaseViewModel
    {
        private HandyControl.Controls.TabItem _selectedItem;
        /// <summary>
        /// 取材核心viewmodel
        /// </summary>
        public MaterialCoreViewModel MaterialCoreVM { get; set; } = new MaterialCoreViewModel();

        /// <summary>
        /// 申请单ViewModel
        /// </summary>
        public AppFrmViewModel AppViewModel { get; set; } = new AppFrmViewModel();

        /// <summary>
        /// 选中的Tabitem
        /// </summary>
        [Notification]
        public HandyControl.Controls.TabItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (_selectedItem?.Tag is EnumMaterialType mType)
                {
                    MaterialCoreVM.MaterialType = mType;
                }
            }
        }


        /// <summary>
        /// 查询申请单
        /// </summary>
        public ICommand AppViewCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var code = MaterialCoreVM?.QueryPathListVM?.SelectedModel?.LabCode;
            if (!string.IsNullOrEmpty(code))
            {
                var msg = new ShowContentWindowMessage("AppFrmMaintain", $"申请单查看及编辑");
                msg.DesignWidth = 1200;
                msg.DesignHeight = 900;
                msg.Args = new object[] { code };
                msg.CallBackCommand = new RelayCommand<bool>(res =>
                {
                    MaterialCoreVM.QueryPathListVM.QueryCommand.Execute(null);
                });
                Messenger.Default.Send(msg);
            }

        })).Value;

        public MaterialMainViewModel()
        {
            PrintLabelManager.Singleton.InitPrintManager(IniSectionConst.MaterialSection);
        }

        /// <summary>
        /// 申请单查看
        /// </summary>
        protected override void ViewAppFrm()
        {
            AppViewCommand.Execute(null);
        }

        /// <summary>
        /// 延迟取材
        /// </summary>
        /// <param name="isAdd"></param>
        protected override void DelayDrawMaterial()
        {
            var sampID = MaterialCoreVM?.QueryPathListVM?.SelectedModel?.SampleID;

            if (!string.IsNullOrEmpty(sampID))
            {
                if (MaterialCoreVM?.MaterialType != EnumMaterialType.Delay)
                {
                    var msg = new ShowContentWindowMessage("MaterialDelay", $"延迟取材");
                    msg.DesignWidth = 450;
                    msg.DesignHeight = 300;
                    msg.Args = new object[] { new SampTissDelayInfo { SampleId = sampID }, true };
                    msg.CallBackCommand = new RelayCommand<SampTissDelayInfo>(async res =>
                    {
                        if (res != null)
                        {
                            try
                            {
                                WhirlingControlManager.ShowWaitingForm();
                                var result = await Task.Run(() => MaterialService.Instance.SaveSampTissDelayInfo(res));
                                if (result)
                                {
                                    MaterialCoreVM?.QueryPathListVM?.QueryCommand.Execute(null);
                                    MaterialCoreVM.MaterialSpecVM?.QueryAllCommand.Execute(null);
                                }
                                else
                                {
                                    ShowMessageBox("延迟取材报错失败！", MessageBoxButton.OK, MessageBoxImage.Error);
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
                else
                {
                    if (MaterialCoreVM?.QueryPathListVM?.SelectedModel?.Status == "1") // 延迟取材里 已经完成取材的，提示下
                    {
                        ShowMessageBox("该项目已完成取材，不可操作延缓取材，请确认！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        /// <summary>
        /// 上一列
        /// </summary>
        protected override void PreviousOne() => MoveSelectSamp(-1);


        /// <summary>
        /// 下一例
        /// </summary>
        protected override void NextOne() => MoveSelectSamp(1);
        /// <summary>
        /// 保存
        /// </summary>
        protected override void Save()
        {
            SaveSampData();
        }
        /// <summary>
        /// 保存下一例
        /// </summary>
        protected override void SaveNext()
        {
            if (SaveSampData())
            {
                // 此处的下一例指未完成的下一列
                var list = MaterialCoreVM?.QueryPathListVM?.SampleList;
                var undoList = list?.Where(x => x.Status == "0")?.ToList();
                if (undoList?.Count > 0) // 存在未完成的，在未完成中浮动
                {
                    var current = MaterialCoreVM?.QueryPathListVM?.SelectedModel;
                    if (current != null)
                    {
                        if (current.Status == "0") // 当前选中的是未完成
                        {

                            var curIndex = undoList.IndexOf(current); // 当前位置
                            var moveIndex = 0;
                            if (curIndex == undoList.Count - 1)
                            {
                                moveIndex = 0;
                            }
                            else
                            {
                                moveIndex = curIndex + 1;
                            }
                            MaterialCoreVM.QueryPathListVM.SelectedModel = undoList.ElementAt(moveIndex);
                        }
                        else // 当前选中的是已完成
                        {
                            MaterialCoreVM.QueryPathListVM.SelectedModel = undoList?.FirstOrDefault();
                        }
                    }
                }
                else // 全部完成后，取完成后的第一条
                {
                    MaterialCoreVM.QueryPathListVM.SelectedModel = list?.FirstOrDefault();
                }
            }
        }
        /// <summary>
        /// 移动选择的位置
        /// </summary>
        /// <param name="step"></param>
        private void MoveSelectSamp(int step)
        {
            var list = MaterialCoreVM?.QueryPathListVM?.SampleList;
            if (list?.Count > 0)
            {
                var current = MaterialCoreVM?.QueryPathListVM?.SelectedModel;
                if (current != null)
                {
                    var curIndex = list.IndexOf(current); // 当前位置
                    int moveIndex = curIndex + step; // 下一步移动的位置
                    if (curIndex == 0 && moveIndex < 0) // 上一例到顶部，直接去最后
                    {
                        moveIndex = list.Count - 1;
                    }
                    if (curIndex == list.Count - 1 && moveIndex > list.Count - 1) // 下一例到底部，直接去顶部
                    {
                        moveIndex = 0;
                    }
                    MaterialCoreVM.QueryPathListVM.SelectedModel = list.ElementAt(moveIndex);
                }
                else
                {
                    MaterialCoreVM.QueryPathListVM.SelectedModel = list[0];
                }
            }
        }

        /// <summary>
        /// 报错样本 组织信息
        /// </summary>
        /// <returns></returns>
        private bool SaveSampData()
        {
            if (!string.IsNullOrEmpty(MaterialCoreVM.MaterialSpecVM.SelectedSamp?.SampleID) && !string.IsNullOrEmpty(MaterialCoreVM.MaterialSpecVM.SampTiss?.SampleID))
            {
                if (string.IsNullOrEmpty(MaterialCoreVM.MaterialSpecVM.SampTiss?.NakedEyes))
                {
                    ShowMessageBox("肉眼所见必填！", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = MaterialService.Instance.SaveSampleTissue(MaterialCoreVM.MaterialSpecVM.SampTiss);
                    if (result)
                    {
                        //MaterialCoreVM.MaterialSpecVM.QueryAllCommand?.Execute(null);
                        MaterialCoreVM.QueryPathListVM.QueryCommand.Execute(null);
                        return true;
                    }
                    else
                    {
                        ShowMessageBox("保存组织信息出错！", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
            return false;
        }
    }
}
