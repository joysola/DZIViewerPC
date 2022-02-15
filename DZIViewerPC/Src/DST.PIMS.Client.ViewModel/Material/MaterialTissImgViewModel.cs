using DST.ApiClient.Service;
using DST.Common.MinioHelper;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DST.PIMS.Client.ViewModel
{
    public class MaterialTissImgViewModel : CustomBaseViewModel
    {
        private HttpListener htpLis = null;
        private Thread listenThread = null;
        private SampTissImgModel _curSelectThumbnailImage;
        private static readonly SemaphoreSlim locker = new SemaphoreSlim(1, 1);
        private static readonly object locker2 = new object();
        /// <summary>
        /// 所选样本
        /// </summary>
        private SampleModel SelectedSamp { get; set; }
        /// <summary>
        /// 展示图
        /// </summary>
        [Notification]
        public BitmapFrame CurBitmapFrame { get; set; }/* = BitmapFrame.Create(new Uri("pack://application:,,,/DST.Controls;component/Images/popup_icon02.png"));*/
        /// <summary>
        /// 缩略图列表
        /// </summary>
        [Notification]
        public ObservableCollection<SampTissImgModel> ThumbnailList { get; set; } = new ObservableCollection<SampTissImgModel>();
        /// <summary>
        /// 选中的缩略图
        /// </summary>
        [Notification]
        public SampTissImgModel CurSelectThumbnailImage
        {
            get => _curSelectThumbnailImage;
            set
            {
                _curSelectThumbnailImage = value;
                this.CurBitmapFrame = null;
                Messenger.Default.Send(true, EnumMessageKey.IntendtoRepairFreeze); // 触发contextmenu显示后关闭，以防止窗体冻结
                if (null != _curSelectThumbnailImage?.CurImage)
                {
                    this.CurBitmapFrame = BitmapFrame.Create(_curSelectThumbnailImage.CurImage);
                }
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand<SampleModel>>(() => new RelayCommand<SampleModel>(async data =>
        {
            if (!string.IsNullOrEmpty(data?.SampleID))
            {

                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    SelectedSamp = data;
                    ThumbnailList.Clear();
                    var result = MaterialService.Instance.GetSampTissImgList(SelectedSamp);
                    result?.ForEach(img => ThumbnailList.Add(img));
                  
                    await IntendtoRepairFreezeImg().ConfigureAwait(false);

                }
                finally
                {

                    WhirlingControlManager.CloseWaitingForm();
                }

            }
        })).Value;
        /// <summary>
        /// 拍照命令
        /// </summary>
        public ICommand CaptureCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            string exePath = Environment.CurrentDirectory + "\\Capture\\DST.Tools.Capture.exe";
            if (!File.Exists(exePath))
            {
                this.ShowMessageBox("未查找到拍照工具，无法拍照！");
                return;
            }

            if (this.htpLis == null)
            {
                // 启动监听
                this.listenThread = new Thread(this.StartListener);
                this.listenThread.Start();
            }

            this.StartCaptureExe(exePath);
        })).Value;
        /// <summary>
        /// 上传图片
        /// </summary>
        public ICommand UploadImageCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "请选择图片";
            dialog.Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    dialog.FileNames.ToList().ForEach(x =>
                    {
                        this.UploadImgToMinio(x);
                    });
                    //this.QueryCommand.Execute(SelectedSamp);

                    await IntendtoRepairFreezeImg();
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;
        /// <summary>
        /// 删除
        /// </summary>
        public ICommand DeleteImageCommand => new Lazy<RelayCommand<SampTissImgModel>>(() => new RelayCommand<SampTissImgModel>(data =>
        {
            if (!string.IsNullOrEmpty(data?.ID))
            {
                ShowMessageBox("是否要删除此图片？", MessageBoxButton.OKCancel, MessageBoxImage.Question, async res =>
                {
                    if (res == MessageBoxResult.OK)
                    {
                        try
                        {
                            WhirlingControlManager.ShowWaitingForm();
                            var result = MaterialService.Instance.DeleteSampTissImg(data);
                            if (result)
                            {
                                this.ThumbnailList.Remove(data);
                                await this.IntendtoRepairFreezeImg();
                            }
                            else
                            {
                                ShowMessageBox("删除失败！", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// 采图
        /// </summary>
        public ICommand DrawStatusCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            if (!string.IsNullOrEmpty(CurSelectThumbnailImage?.ID))
            {
                try
                {
                    WhirlingControlManager.ShowWaitingForm();
                    var result = MaterialService.Instance.UpdateReportDrawStatus(CurSelectThumbnailImage);
                    if (result)
                    {
                        CurSelectThumbnailImage.ReportDrawStatus = "1";
                        // 更新其他缩略图的采图状态（缓存更新）
                        foreach (var thumb in ThumbnailList)
                        {
                            if (thumb != CurSelectThumbnailImage)
                            {
                                thumb.ReportDrawStatus = "0";
                            }
                        }
                    }
                    else
                    {
                        ShowMessageBox("设置采图失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                finally
                {
                    WhirlingControlManager.CloseWaitingForm();
                }
            }
        })).Value;
        private void UpdateImageList(string imgPath)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));
                SynchronizationContext.Current.Post(pl =>
                {
                    if (!string.IsNullOrEmpty(SelectedSamp?.SampleID))
                    {
                        string[] imgs = imgPath.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < imgs.Length; i++)
                        {
                            this.UploadImgToMinio(imgs[i]);
                        }
                        this.QueryCommand.Execute(SelectedSamp);
                    }
                }, null);
            });
        }
        private void UploadImgToMinio(string localImgPath)
        {
            string url = MinioHelper.Client.UploadFile(localImgPath, CommonConfiguration.BucketName, "request", null).ConfigureAwait(false).GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(url))
            {
                SampTissImgModel newModel = new SampTissImgModel()
                {
                    GeneralImg = url,
                    SampleID = SelectedSamp?.SampleID,
                };

                var result = MaterialService.Instance.SaveSampTissImg(newModel);
                if (!string.IsNullOrEmpty(result?.ID))
                {
                    this.ThumbnailList.Add(result);
                }
                else
                {
                    ShowMessageBox("保存信息失败！", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                this.ShowMessageBox($"上传图片 {localImgPath} 失败！");
            }
        }
        /// <summary>
        /// 启动exe程序
        /// </summary>
        private void StartCaptureExe(string path)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = path;
            info.WindowStyle = ProcessWindowStyle.Minimized;
            Process.Start(info);
        }
        /// <summary>
        /// 启动监听服务，接收工具返回的拍照结果
        /// </summary>
        private void StartListener()
        {
            this.htpLis = new HttpListener();
            htpLis.Prefixes.Add("http://localhost:6300/");
            htpLis.Start();
            while (true)
            {
                HttpListenerContext context = htpLis.GetContext();
                HttpListenerRequest request = context.Request;

                HttpListenerResponse response = context.Response;
                using (Stream str = response.OutputStream)
                {
                    using (StreamWriter sw = new StreamWriter(str))
                    {
                        string key = context.Request.QueryString["ImagePath"];
                        byte[] c = Convert.FromBase64String(key);
                        string imgPath = System.Text.Encoding.UTF8.GetString(c);
                        Task.Run(() => { this.UpdateImageList(imgPath); });
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 卸载窗口，释放线程
        /// </summary>
        public override void OnViewUnLoaded()
        {
            base.OnViewUnLoaded();
            if (this.listenThread != null)
            {
                this.listenThread.Abort();
                this.listenThread = null;
            }

            if (this.htpLis != null)
            {
                this.htpLis.Stop();
                this.htpLis.Close();
                this.htpLis = null;
            }
        }
        /// <summary>
        /// 修复图像控件，加载图片后，没有立即显示的问题
        /// </summary>
        /// <returns></returns>
        private async Task IntendtoRepairFreezeImg()
        {
            SampTissImgModel temp = null;
            temp = ThumbnailList.FirstOrDefault(x => x.ReportDrawStatus == "1") ?? ThumbnailList.FirstOrDefault();
            CurSelectThumbnailImage = temp;
            await Task.Delay(400); // 使主图显示
            await Task.Yield();
            CurSelectThumbnailImage = temp;
        }
    }
}
