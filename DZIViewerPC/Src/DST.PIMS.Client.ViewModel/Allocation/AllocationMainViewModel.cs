using DST.ApiClient.Service;
using DST.Controls;
using DST.Controls.Base;
using DST.Database.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class AllocationMainViewModel : CustomBaseViewModel
    {
        [Notification]
        public AllocationReturnModel CurReturn { get; set; }

        [Notification]
        public AllocationQueryModel CurQueryModel { get; set; } = new AllocationQueryModel() { confirmStatus = 0, doctorView = 0, startIndex = 0 };

        [Notification]
        public AllocationModel CurSelectedModel { get; set; }

        public ICommand ViewerCommand { get; set; }

        public ICommand AutoTestCommand { get; set; }

        private bool isAutoTest = false;

        public AllocationMainViewModel()
        {

        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.ViewerCommand = new RelayCommand<object>(async data =>
            {
                if (this.CurSelectedModel != null)
                {
                    var urlListTask = Task.Run(() => AllocationService.Instance.ListScanImageUrlBySampleId(this.CurSelectedModel.sampleId));
                    var csTask = Task.Run(() => AllocationService.Instance.GetSampleDetail(this.CurSelectedModel.sampleId));
                    await Task.WhenAll(urlListTask, csTask);
                    List<string> urlList = urlListTask.Result;
                    var allSampDetail = csTask.Result;
                    if (urlList != null && urlList.Count >= 1 /*&& allSampDetail.CutScanVOList.Count > 1*/)
                    {
                        ShowContentWindowMessage msg = new ShowContentWindowMessage("ImgViewerManager", "看图");
                        msg.DesignHeight = 1200;
                        msg.DesignWidth = 1500;
                        msg.Args = new object[] { urlList, this.isAutoTest, allSampDetail };
                        Messenger.Default.Send(msg);
                    }
                    else
                    {
                        this.ShowMessageBox("未找到图片URL信息！", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning, null, true, 1000);
                    }
                }
            });

            this.AutoTestCommand = new RelayCommand<object>(data =>
            {
                this.isAutoTest = true;
                try
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        this.CurReturn.records.ToList().ForEach(x =>
                        {
                            this.CurSelectedModel = x;
                            this.ViewerCommand.Execute(null);
                        });
                    }
                }
                catch
                {
                }

                this.isAutoTest = false;
            });
        }

        public override void LoadData()
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                this.CurReturn = AllocationService.Instance.PageWaitReviewCellByCondition(this.CurQueryModel, this.PageModel.PageIndex, this.PageModel.PageSize);
                if (null != this.CurReturn)
                {
                    this.PageModel.TotalPage = this.CurReturn.pages.Value;
                    this.PageModel.TotalNum = this.CurReturn.total.Value;
                }
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        }

        public void Query_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.LoadData();
            }
        }
    }
}
