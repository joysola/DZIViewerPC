using DST.ApiClient.Service;
using DST.Common.MinioHelper;
using DST.Controls;
using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using Nico.Common;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DST.PIMS.Client.ViewModel
{
    public class RequestDocViewModel : CustomBaseViewModel
    {
        private HttpListener htpLis = null;
        private Thread listenThread = null;
        private string pathologyId = string.Empty; // 病理ID
        private PathologyImage curSelectThumbnailImage = null;

        /// <summary>
        /// 是否需要拍照
        /// </summary>
        [Notification]
        public Visibility CanCapture { get; set; } = Visibility.Collapsed;
        /// <summary>
        /// 是否只读
        /// </summary>
        [Notification]
        public bool IsReadOnly { get; set; } = false;
        /// <summary>
        /// 选中的缩略图
        /// </summary>
        [Notification]
        public PathologyImage CurSelectThumbnailImage
        {
            get { return this.curSelectThumbnailImage; }
            set
            {
                this.curSelectThumbnailImage = value;
                //this.RaisePropertyChanged("CurSelectThumbnailImage");
                this.CurBitmapFrame = null;
                if (null != this.curSelectThumbnailImage && null != this.curSelectThumbnailImage.CurImage)
                {
                    this.CurBitmapFrame = BitmapFrame.Create(this.curSelectThumbnailImage.CurImage);
                }
            }
        }

        [Notification]
        public BitmapFrame CurBitmapFrame { get; set; }

        /// <summary>
        /// 缩略图列表
        /// </summary>
        [Notification]
        public ObservableCollection<PathologyImage> ThumbnailList { get; set; } = new ObservableCollection<PathologyImage>();
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand QueryCommand => new Lazy<RelayCommand>(() => new RelayCommand(async () =>
        {
            try
            {
                WhirlingControlManager.ShowWaitingForm();
                this.ThumbnailList.Clear();
                var result = ApplyFormService.Instance.ListByPathologyId(this.pathologyId);
                result?.ForEach(pp => ThumbnailList.Add(pp));
                var tmp = this.ThumbnailList.FirstOrDefault();
                this.CurSelectThumbnailImage = tmp;
                await Task.Delay(100);
                this.CurSelectThumbnailImage = tmp;
            }
            finally
            {
                WhirlingControlManager.CloseWaitingForm();
            }
        })).Value;
        /// <summary>
        /// 切换图片
        /// </summary>
        public ICommand ChangeImage { get; set; }

        /// <summary>
        /// 拍照命令
        /// </summary>
        public ICommand CaptureCommand { get; set; }

        /// <summary>
        /// 上传图片
        /// </summary>
        public ICommand UploadImageCommand { get; set; }

        public ICommand DeleteImageCommand { get; set; }


        public RequestDocViewModel()
        {
            // 接收更新pathologyId的消息
            Messenger.Default.Register<(bool isReadOnly, string pathID)>(this, EnumMessageKey.GeneralImg, async data =>
            {
                if (!string.IsNullOrEmpty(data.pathID))
                {
                    try
                    {
                        WhirlingControlManager.ShowWaitingForm();
                        this.IsReadOnly = data.isReadOnly;
                        this.pathologyId = data.pathID;
                        await Dispatcher.CurrentDispatcher.InvokeAsync(() => this.LoadData());
                    }
                    finally
                    {
                        WhirlingControlManager.CloseWaitingForm();
                    }
                }
            });
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.CaptureCommand = new RelayCommand<object>(data =>
            {
                this.StartCapture();
            });

            this.UploadImageCommand = new RelayCommand<object>(data =>
            {
                this.UploadImage();
            });

            this.DeleteImageCommand = new RelayCommand<object>(data =>
            {
                int index = this.ThumbnailList.IndexOf(this.CurSelectThumbnailImage);
                if (!ApplyFormService.Instance.RemoveById(this.CurSelectThumbnailImage.id))
                {
                    this.ShowMessageBox("删除申请单失败，请联系管理员！");
                }
                else
                {
                    this.ThumbnailList.Remove(this.CurSelectThumbnailImage);
                    if (this.ThumbnailList.Count > 0)
                    {
                        this.CurSelectThumbnailImage = this.ThumbnailList.Count > index ? this.ThumbnailList[index] : this.ThumbnailList[this.ThumbnailList.Count - 1];
                    }
                    else
                    {
                        this.CurSelectThumbnailImage = null;
                    }
                }
            });
        }

        /// <summary>
        /// 上传本地图片
        /// </summary>
        private void UploadImage()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "请选择图片";
            dialog.Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == true)
            {
                dialog.FileNames.ToList().ForEach(x =>
                {
                    this.UploadImgToMinio(x);
                });

                this.LoadData();
            }
        }

        private void UploadImgToMinio(string localImgPath)
        {
            string url = MinioHelper.Client.UploadFile(localImgPath, CommonConfiguration.BucketName, "request", null).Result;

            if (!string.IsNullOrEmpty(url))
            {
                PathologyImage newModel = new PathologyImage()
                {
                    applicationFormUrl = url,
                    pathologyId = this.pathologyId,
                };

                this.ThumbnailList.Add(newModel);
                ApplyFormService.Instance.SavePathologyImage(newModel);
            }
            else
            {
                this.ShowMessageBox($"上传图片 {localImgPath} 失败！");
            }
        }

        /// <summary>
        /// 启动拍照
        /// </summary>
        private void StartCapture()
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
                this.listenThread = new System.Threading.Thread(this.StartListener);
                this.listenThread.Start();
            }

            this.StartCaptureExe(exePath);
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
                        Logger.Info("接收高拍仪图片地址：" + imgPath);
                        Task.Run(() => { this.UpdateImageList(imgPath); });
                    }
                }
            }
        }

        private void UpdateImageList(string imgPath)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(System.Windows.Application.Current.Dispatcher));
                SynchronizationContext.Current.Post(pl =>
                {
                    string[] imgs = imgPath.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < imgs.Length; i++)
                    {
                        this.UploadImgToMinio(imgs[i]);
                    }

                    this.LoadData();
                }, null);
            });
        }

        /// <summary>
        /// 接收参数，判断是否可以拍照
        /// </summary>
        public override void OnViewLoaded()
        {
            base.OnViewLoaded();
            this.Result = this.ThumbnailList;
            if (this.Args != null)
            {
                this.CanCapture = this.Args[0].ToString().Equals("CanCapture") ? Visibility.Visible : Visibility.Collapsed;
                this.pathologyId = this.Args[1].ToString();
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void LoadData()
        {
            //this.ThumbnailList = null;
            //this.CurSelectThumbnailImage = null;
            if (!string.IsNullOrEmpty(this.pathologyId))
            {
                QueryCommand.Execute(null);
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
    }
}
