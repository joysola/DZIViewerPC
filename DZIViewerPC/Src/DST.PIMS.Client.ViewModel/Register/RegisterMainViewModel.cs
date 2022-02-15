using DST.ApiClient.Service;
using DST.Common.Extensions;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Extensions;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.PIMS.Client.ViewModel
{
    public class RegisterMainViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 登记信息
        /// </summary>
        public RegisterSampleViewModel RegSampViewModel { get; set; } = new RegisterSampleViewModel();
        /// <summary>
        /// 查询信息
        /// </summary>
        public RegisterQueryViewModel RegQueryViewModel { get; set; } = new RegisterQueryViewModel();
        /// <summary>
        /// 一维码配置
        /// </summary>
        private TSCBarCodeSetting BarcodeSetting { get; set; }
        public RegisterMainViewModel()
        {
            PrintLabelManager.Singleton.InitPrintManager(IniSectionConst.RegisterSection);
            RegQueryViewModel.QueryRegSampleCommand = RegSampViewModel.QueryCommand; // 转存RegisterSampleViewModel的查询命令
            RegQueryViewModel.QueryCommand?.Execute(null); // 第一次查询
            BarcodeSetting = PrintSetHelper.GetTSCBarCodeSetting(IniSectionConst.ProdBarcodeSection);
        }


        /// <summary>
        /// 重置
        /// </summary>
        protected override void Reset()
        {
            ShowMessageBox("确定要重置数据吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    ResetData();
                }
            });
        }
        /// <summary>
        /// 保存
        /// </summary>
        protected override void Save()
        {
            Action saveAction = () =>
            {
                if (RegSampViewModel.AppViewModel.CheckSaveData())
                {
                    SaveAppFrmData().NoWarning();
                }
            };

            if (RegSampViewModel.IsExisted)
            {
                ShowMessageBox("该病例已保存，是否覆盖保存，请确认？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        saveAction();
                    }
                });
            }
            else
            {
                saveAction();
            }
        }
        /// <summary>
        /// 保存&打印
        /// </summary>
        protected override void SavePrint()
        {
            Action savePrintAction = async () =>
             {
                 if (RegSampViewModel.AppViewModel.CheckSaveData())
                 {
                     var result = await SaveAppFrmData();
                     if (result?.Count > 0)
                     {
                         Task.Run(() =>
                         {
                             //TSCPrintManager.Instance.Print(result);
                             PrintLabelManager.Singleton.Print(result);
                         }).NoWarning();
                     }
                 }
             };

            if (RegSampViewModel.IsExisted)
            {
                ShowMessageBox("该病例已保存，是否覆盖保存，请确认？", MessageBoxButton.OKCancel, MessageBoxImage.Question, res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        savePrintAction();
                    }
                });
            }
            else
            {
                savePrintAction();
            }
        }
        /// <summary>
        /// 补打条码
        /// </summary>
        protected override void ReprintCode()
        {
            if (!string.IsNullOrEmpty(RegSampViewModel.AppViewModel.AppModel?.PathID))
            {
                Task.Run(() =>
                {
                    // 根据病理id，补打条码
                    var result = RegisterService.Instance.GetAppFrmPrintCodeList(RegSampViewModel.AppViewModel.AppModel?.PathID);
                    if (result?.Count > 0)
                    {
                        //TSCPrintManager.Instance.Print(result);
                        PrintLabelManager.Singleton.Print(result);
                    }
                });
            }
        }
        /// <summary>
        /// 登记查询
        /// </summary>
        protected override void QueryRegistration()
        {
            var msg = new ShowContentWindowMessage("RegisterAllQuery", "登记查询");
            msg.DesignWidth = 1300;
            msg.DesignHeight = 800;
            //msg.Args = new object[] { type };
            //msg.CallBackCommand = command;
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 批量外送
        /// </summary>
        protected override void BatchSendInsp()
        {
            var msg = new ShowContentWindowMessage("RegisterDelivery", "批量外送");
            msg.DesignWidth = 1300;
            msg.DesignHeight = 800;
            msg.CallBackCommand = new RelayCommand<bool>(res =>
            {
                if (res)
                {
                    RegQueryViewModel.QueryCommand.Execute(null); // 刷新登记列表
                }
            });
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 送检确认
        /// </summary>
        protected override void CheckSendInsp()
        {
            var msg = new ShowContentWindowMessage("PhysDistPreTreat", "送检确认");
            msg.DesignWidth = 1800;
            msg.DesignHeight = 800;
            msg.CallBackCommand = new RelayCommand(() =>
            {
                RegQueryViewModel.QueryCommand.Execute(null); // 刷新登记列表
            });
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 取消登记
        /// </summary>
        protected override void WithdrawRegistration()
        {
            ShowMessageBox("确定要取消登记吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, async res =>
             {
                 if (res == MessageBoxResult.OK)
                 {
                     var samp = RegSampViewModel.AppViewModel.AppModel?.PathSampInfoList?.FirstOrDefault(x => x.InspectStatus == "1");
                     if (samp != null)
                     {
                         ShowMessageBox("存在已经送检的项目，无法取消登记！", MessageBoxButton.OK, MessageBoxImage.Warning);
                         return;
                     }
                     var result = await Task.Run(() => RegisterService.Instance.DeletePathInfo(RegSampViewModel.AppViewModel.AppModel));
                     if (result)
                     {
                         ResetData();
                         RegQueryViewModel.QueryCommand.Execute(null); // 刷新登记列表
                     }
                     else
                     {
                         ShowMessageBox("取消登记失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                 }
             });
        }
        /// <summary>
        /// 重新取样关联
        /// </summary>
        protected override void ResampleConnect()
        {
            var msg = new ShowContentWindowMessage("RegReSample", " 重新取样列表");
            msg.DesignWidth = 1400;
            msg.DesignHeight = 800;
            msg.CallBackCommand = new RelayCommand(() =>
            {
                RegQueryViewModel.QueryCommand.Execute(null); // 刷新登记列表
            });
            Messenger.Default.Send(msg);
        }
        /// <summary>
        /// 申请单图像采集
        /// </summary>
        protected override void CollectAppFrmImage()
        {
            if (!string.IsNullOrEmpty(RegSampViewModel.AppViewModel.AppModel?.PathID))
            {
                var msg = new ShowContentWindowMessage("RequestDoc", "申请单采集");
                msg.DesignHeight = 600;
                msg.DesignWidth = 800;
                msg.Args = new object[] { "CanCapture", RegSampViewModel.AppViewModel.AppModel?.PathID };
                Messenger.Default.Send(msg);
            }
            else
            {
                ShowMessageBox("请选择一个患者", MessageBoxButton.OK, MessageBoxImage.Information, null, true);
            }
        }
        /// <summary>
        /// 保存&外送
        /// </summary>
        protected override void SaveSend()
        {
            ShowMessageBox("确定要保存并外送吗？", MessageBoxButton.OKCancel, MessageBoxImage.Question, async res =>
            {
                if (res == MessageBoxResult.OK)
                {
                    if (RegSampViewModel.AppViewModel.CheckSaveData())
                    {
                        var result = await Task.Run(() => ApplyFormService.Instance.SavePathandSend(RegSampViewModel.AppViewModel.AppModel));
                        if (result?.Count > 0)
                        {
                            Task.Run(() =>
                            {
                                TSCBarCodeManager.Singleton.Print(result,BarcodeSetting);
                                //var barcodes = result.Select(x => x.BarCode).ToList();
                                //TSCBarCodeManager.Singleton.Print(barcodes, BarcodeSetting);
                            }).NoWarning();
                            ShowMessageBox("保存成功！", MessageBoxButton.OK, MessageBoxImage.Information, null, true);
                        }
                    }
                }
            });

        }
        /// <summary>
        /// 重置定制
        /// </summary>
        private void ResetData()
        {
            RegSampViewModel.AppViewModel.AppModel = new ApplyFrmModel();
            //RegSampViewModel.AppViewModel.IsAdd = true; // 重置属于新增模式
            RegSampViewModel.AppViewModel.Permissions.SetPermissionIsEdit(true);// 重置属于新增模式
            RegSampViewModel.IsExisted = false; // 记录不存在
        }

        /// <summary>
        /// 保存申请单数据
        /// </summary>
        /// <returns></returns>
        private async Task<List<AppFrmPrintCode>> SaveAppFrmData()
        {
            var result = new List<AppFrmPrintCode>();
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                result = await Task.Run(() => ApplyFormService.Instance.SavePathInfo(RegSampViewModel.AppViewModel.AppModel));
                if (result?.Count > 0)
                {
                    ShowMessageBox("保存成功！", MessageBoxButton.OK, MessageBoxImage.Information, null, true);
                    //Reset();
                    RegQueryViewModel.QueryCommand.Execute(RegSampViewModel.IsExisted.ToString()); // 刷新登记列表
                }
                //else
                //{
                //    ShowMessageBox("保存申请单失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                //}
                return result;

            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }
    }
}
