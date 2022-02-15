using DST.ApiClient.Service;
using DST.Common.Helper;
using DST.Common.MinioHelper;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class RepoSysMainViewModel : CustomBaseViewModel
    {
        private bool selectAll = false;

        /// <summary>
        /// 全选
        /// </summary>
        public bool SelectAll
        {
            get { return this.selectAll; }
            set
            {
                this.selectAll = value;
                this.RaisePropertyChanged("SelectAll");
                this.CurReportQueryReturn.records.ToList().ForEach(x =>
                {
                    x.IsSelected = value;
                });
            }
        }

        [Notification]
        public ReportQuery CurReportQuery { get; set; } = new ReportQuery();

        [Notification]
        public ReportQueryReturn CurReportQueryReturn { get; set; } = new ReportQueryReturn();

        public Report CurSelectedPeport { get; set; }

        public ICommand QueryCommand { get; set; }

        public ICommand DetailCommand { get; set; }

        public ICommand ReviewReportCommand { get; set; }

        public ICommand EmergencyCommand { get; set; }

        public ICommand ChargeBackCommand { get; set; }

        [Notification]
        public List<ProductModel> ProductDict { get; set; }

        [Notification]
        public List<DictItem> ReportStatusList { get; set; }

        public RepoSysMainViewModel()
        {
            this.ProductDict = ExtendApiDict.Instance.ProductDict;
            if(ExtendApiDict.Instance.ReportStatusList == null || ExtendApiDict.Instance.ReportStatusList.Count == 0)
            {
                ExtendApiDict.Instance.ReportStatusList = DictService.Instance.GetReportStatusList().Result;
            }
            this.ReportStatusList = ExtendApiDict.Instance.ReportStatusList;
        }

        public override void LoadData()
        {
            this.CurReportQueryReturn = ReportSystemService.Instance.PageByCurrentLoginHospital(this.CurReportQuery, this.PageModel.PageIndex, this.PageModel.PageSize);
            if (null != this.CurReportQueryReturn)
            {
                this.PageModel.TotalNum = this.CurReportQueryReturn.total.Value;
                this.PageModel.TotalPage = this.CurReportQueryReturn.pages.Value;
            }
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.QueryCommand = new RelayCommand<object>(data =>
            {
                this.PageModel.PageIndex = 1;
                this.LoadData();
            });

            this.DetailCommand = new RelayCommand<object>(data =>
            {
                if (this.CurSelectedPeport != null)
                {
                    var msg = new ShowContentWindowMessage("AppImgView", $"申请单查看");
                    msg.DesignWidth = 1200;
                    msg.DesignHeight = 900;
                    msg.Args = new object[] { this.CurSelectedPeport.laboratoryCode };
                    Messenger.Default.Send(msg);
                }
            });

            this.ReviewReportCommand = new RelayCommand<object>(data =>
            {
                if(this.CurSelectedPeport != null)
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        ReportUrl res = ReportSystemService.Instance.GetReportUrlBySampleId(this.CurSelectedPeport.id);
                        if (res != null)
                        {
                            string info = $"当前病例：{ this.CurSelectedPeport.pathologyCode }                 {this.CurSelectedPeport.patientName}      {ExtendApiDict.Instance.SexDict.FirstOrDefault(x => x.dictKey.Equals(this.CurSelectedPeport.patientSex)).dictValue}      {this.CurSelectedPeport.patientAge}岁                 科室：{this.CurSelectedPeport.dept}                 检查项目：{this.CurSelectedPeport.productName}";
                            ShowContentWindowMessage msg = new ShowContentWindowMessage("ReportShow", "报告预览");
                            msg.DesignHeight = 750;
                            msg.DesignWidth = 1000;
                            msg.Args = new object[] { info, res };
                            Messenger.Default.Send(msg);
                        }
                    }
                    catch
                    { }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });

            this.EmergencyCommand = new RelayCommand<object>(data =>
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    ShowContentWindowMessage msg = new ShowContentWindowMessage("ReportEmergency", "加测");
                    msg.DesignWidth = 1050;
                    msg.DesignHeight = 750;
                    msg.Args = new object[] { this.CurSelectedPeport };
                    msg.CallBackCommand = new RelayCommand<object>(res =>
                    {
                        this.LoadData();
                    });
                    Messenger.Default.Send(msg);
                }
                catch
                { }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            });

            this.ChargeBackCommand = new RelayCommand<Report>(data =>
            {
                this.BatchChargeBack(data);
            });
        }

        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.QueryCommand.Execute(null);
            }
        }

        /// <summary>
        /// 批量退单
        /// </summary>
        protected void BatchChargeBack(Report report)
        {
            if(report == null)
            {
                return;
            }

            ShowContentWindowMessage msg = new ShowContentWindowMessage("ReportChargeBack", "退单");
            msg.DesignHeight = 300;
            msg.DesignWidth = 550;
            msg.CallBackCommand = new RelayCommand<string>(data =>
            {
                if (!string.IsNullOrEmpty(data))
                {
                    ReportSystemService.Instance.ChargeBack(new ChargeBackModel() { sampleId = report.id, chargeBackCause = data });
                    this.QueryCommand.Execute(null);
                }
            });
            Messenger.Default.Send(msg);
        }

        /// <summary>
        /// 重新生成 报告
        /// </summary>
        protected override void ReGenerateReport()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (this.CurReportQueryReturn.records.FirstOrDefault(x => x.IsSelected) == null)
                {
                    this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }

                string msg = "";
                this.CurReportQueryReturn.records.Where(x => x.IsSelected && (!x.confirmStatus.HasValue || x.confirmStatus.Value == 0)).ToList().ForEach(x =>
                {
                    msg += $"实验室编号：{x.laboratoryCode} 还未确认，无法生成报告！ \r\n";
                });

                if(!string.IsNullOrEmpty(msg))
                {
                    this.ShowMessageBox(msg, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                    return;
                }

                ApiResponse<object> res = ReportSystemService.Instance.BuildReport(this.CurReportQueryReturn.records.Where(x => x.IsSelected).Select(x => x.id).ToList());
                if (res.Success)
                {
                    this.QueryCommand.Execute(null);
                }
                else
                {
                    WhirlingControlManager.CloseWaitingForm();
                    this.ShowMessageBox(res.Msg, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            catch
            { }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        protected override void BatchChargeback() 
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                if (this.CurReportQueryReturn.records.FirstOrDefault(x => x.IsSelected) == null)
                {
                    this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    return;
                }

                ShowContentWindowMessage msg = new ShowContentWindowMessage("ReportChargeBack", "批量退单");
                msg.DesignHeight = 300;
                msg.DesignWidth = 550;
                msg.CallBackCommand = new RelayCommand<string>(data =>
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        this.CurReportQueryReturn.records.Where(x => x.IsSelected).ToList().ForEach(t =>
                        {
                            ReportSystemService.Instance.ChargeBack(new ChargeBackModel() { sampleId = t.id, chargeBackCause = data });
                        });
                        
                        this.QueryCommand.Execute(null);
                    }
                });
             
                Messenger.Default.Send(msg);
            }
            catch
            { }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        /// <summary>
        /// 打印报告
        /// </summary>
        protected override void BatchPrintReport()
        {
            if(this.CurReportQueryReturn.records.FirstOrDefault(x => x.IsSelected) == null)
            {
                this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                return;
            }

            try
            {
                WhirlingControlManager.ShowWaitingForm();
                List<string> pdfList = new List<string>();
                string msg = "";
                this.CurReportQueryReturn.records.Where(x => x.IsSelected).ToList().ForEach(x =>
                {
                    ReportUrl res = ReportSystemService.Instance.GetReportUrlBySampleId(x.id);
                    if (null != res)
                    {
                        if (!string.IsNullOrEmpty(res.reportUrl))
                        {
                            res.LocalReportUrl = ExtendAppContext.Current.SystemTempPath + Path.GetFileName(res.reportUrl);
                            bool isSuc = MinioHelper.Client.DownloadFile(res.reportUrl, res.LocalReportUrl).Result;
                            pdfList.Add(res.LocalReportUrl);
                        }
                        else
                        {
                            msg += $"实验室编号：{x.laboratoryCode} 还未确认，无法打印报告！ \r\n";
                        }

                        if (!string.IsNullOrEmpty(res.reportUrlEnglish))
                        {
                            res.reportUrlEnglish = ExtendAppContext.Current.SystemTempPath + Path.GetFileName(res.reportUrlEnglish);
                            bool isSuc = MinioHelper.Client.DownloadFile(res.reportUrlEnglish, res.reportUrlEnglish).Result;
                            pdfList.Add(res.reportUrlEnglish);
                        }
                    }
                });

                string path = ExtendAppContext.Current.ConfigurationIniPath;
                string defaultPrintName = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.CommonConfig, "DefaultPrinterName");
                pdfList.ForEach(x =>
                {
                    bool re = PrintManager.Instance.PrintPDF(x, defaultPrintName).Result;
                });

                if(!string.IsNullOrEmpty(msg))
                {
                    this.ShowMessageBox(msg, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
            }
            catch(Exception ex)
            {
                WhirlingControlManager.CloseWaitingForm();
                this.ShowMessageBox("报告打印失败，请联系管理员");
                Logger.Error("报告打印失败：" + ex.Message);
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        /// <summary>
        /// 退回检验
        /// </summary>
        protected override void BackCheckoutBatch() 
        {
            if (this.CurReportQueryReturn.records.FirstOrDefault(x => x.IsSelected) == null)
            {
                this.ShowMessageBox("未选中数据，请重新选择！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                return;
            }

            this.ShowMessageBox("此操作将对当前样本进行重新实验处理，确认退回检验吗？", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question, result =>
            { 
                if(result == System.Windows.MessageBoxResult.Yes || result == System.Windows.MessageBoxResult.OK)
                {
                    bool res = ReportSystemService.Instance.BackCheckoutBatch(this.CurReportQueryReturn.records.Where(x => x.IsSelected).Select(x => x.id).ToList());
                    if (res)
                    {
                        this.QueryCommand.Execute(null);
                    }
                }
            });
        }
    }
}
