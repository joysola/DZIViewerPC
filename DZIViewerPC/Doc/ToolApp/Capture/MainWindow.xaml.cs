using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private int index = 0;
        /// <summary>
        /// 旋转角度：0:0， 1:90， 2:180， 3:270
        /// </summary>
        private int rotateIndex = 1;

        private bool isMouseDown = false;

        /// <summary>
        /// 图片存储路径
        /// </summary>
        private string baseImgPath = Environment.CurrentDirectory + "\\HanWangImages\\";

        private MainWindowViewModel mainWindowViewModel = null;

        public MainWindow()
        {
            InitializeComponent();
            this.Topmost = true;
            this.mainWindowViewModel = new MainWindowViewModel();
            this.DataContext = this.mainWindowViewModel;

            this.picBox.MouseDown += PicBox_MouseDown;
            this.picBox.MouseUp += PicBox_MouseUp;
            this.picBox.MouseMove += PicBox_MouseMove;
            this.picBox.MouseWheel += PicBox_MouseWheel;
            this.Loaded += this.MainWindow_Loaded;
            this.Closed += this.MainWindow_Closed;
            this.Closing += this.MainWindow_Closing;

            if (System.IO.Directory.Exists(this.baseImgPath))
            {
                System.IO.Directory.Delete(this.baseImgPath, true);
            }

            // new System.Threading.Thread(this.StartListener).Start();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(this.mainWindowViewModel.ThumbnailList.FirstOrDefault(x => x.IsChecked) == null)
            {
                var res = MessageBox.Show("未选中图像，确认关闭？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(res == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }

            string data = string.Join(";", this.mainWindowViewModel.ThumbnailList.Select(x => x.ImagePath).ToArray());
            var b = Encoding.UTF8.GetBytes(data);
            string dataSix = Convert.ToBase64String(b);
            this.HttpGet("http://localhost:6300/", $"ImagePath={dataSix}");
        }

        private void PicBox_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            HanWangCameraAPI.HWOnWheel(this.picBox.Handle, e.Delta);
        }

        private void PicBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                HanWangCameraAPI.HWOnMouseMoving(this.picBox.Handle, e.X, e.Y);
            }
        }

        private void PicBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.picBox.ContextMenuStrip.Show();
                this.ReleaseMouseCapture();
            }
            else
            {
                HanWangCameraAPI.HWOnLMouseRelease(this.picBox.Handle, e.X, e.Y);
            }

            this.isMouseDown = false;
        }

        private void PicBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.isMouseDown = true;
            HanWangCameraAPI.HWOnLMousePress(this.picBox.Handle, e.X, e.Y);
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            HanWangCameraAPI.HWCloseCamera(this.index);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化
            HanWangCameraAPI.HWInit();

            // 获取设备列表
            int count = HanWangCameraAPI.HWGetCameraCount();
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    IntPtr hwNameIntPtr = HanWangCameraAPI.HWGetCameraName(i);
                    string hwDevicesName = Marshal.PtrToStringAnsi(hwNameIntPtr);
                    this.cbCamera.Items.Add(hwDevicesName);
                    if(hwDevicesName.Equals("Q3200AF"))
                    {
                        this.index = i;
                    }
                }

                this.cbCamera.SelectedIndex = this.index;
            }

            // 连接视频
            HanWangCameraAPI.HWOpenCamera(index, this.picBox.Handle);

            this.RefreshResoluationi();

            // 自动旋转90都，垂直
            HanWangCameraAPI.HWSetRotate(index, this.rotateIndex);

            // 自动纠偏裁剪
            HanWangCameraAPI.HWSetMouseMode(index, 0);
            HanWangCameraAPI.HWEnableAutoRotate(index, true);

            this.RegisterPicBoxContextMenu();
        }

        private void RegisterPicBoxContextMenu()
        {
            System.Windows.Forms.ContextMenuStrip curMenu = new System.Windows.Forms.ContextMenuStrip();

            System.Windows.Forms.ToolStripMenuItem menuRout = new System.Windows.Forms.ToolStripMenuItem("旋转");
            menuRout.Click += MenuRout_Click;
            curMenu.Items.Add(menuRout);

            System.Windows.Forms.ToolStripMenuItem menuPorp = new System.Windows.Forms.ToolStripMenuItem("属性");
            menuPorp.Click += MenuPorp_Click; ;
            curMenu.Items.Add(menuPorp);

            System.Windows.Forms.ToolStripMenuItem menuAutoCut = new System.Windows.Forms.ToolStripMenuItem("裁剪");
            menuAutoCut.Click += MenuAutoCut_Click; ; ;
            curMenu.Items.Add(menuAutoCut);

            this.picBox.ContextMenuStrip = curMenu;
        }

        private void MenuAutoCut_Click(object sender, EventArgs e)
        {
            (sender as System.Windows.Forms.ToolStripMenuItem).Checked = !(sender as System.Windows.Forms.ToolStripMenuItem).Checked;
            if ((sender as System.Windows.Forms.ToolStripMenuItem).Checked)
            {
                HanWangCameraAPI.HWSetMouseMode(index, 0);
                HanWangCameraAPI.HWEnableAutoRotate(index, true);
            }
            else
            {
                HanWangCameraAPI.HWSetMouseMode(index, 0);
                HanWangCameraAPI.HWEnableAutoRotate(index, false);
            }
        }

        private void MenuPorp_Click(object sender, EventArgs e)
        {
            HanWangCameraAPI.HWShowVideoProp(this.index);
        }

        private void MenuRout_Click(object sender, EventArgs e)
        {
            if (this.rotateIndex == 3)
            {
                this.rotateIndex = 0;
            }
            else
            {
                this.rotateIndex++;
            }
            HanWangCameraAPI.HWSetRotate(this.index, this.rotateIndex);
        }

        /// <summary>
        /// 拍照
        /// </summary>
        private void Btn_Capture(object sender, RoutedEventArgs e)
        {
            System.IO.Directory.CreateDirectory(this.baseImgPath);
            string imgPath = this.baseImgPath + DateTime.Now.Ticks +".jpg";
            HanWangCameraAPI.HWCapture(this.index, imgPath);
            System.Threading.Thread.Sleep(500);
            ImageModel newModel = new ImageModel() 
            { 
                CurImage = this.BitmapToBitmapImage((Bitmap)System.Drawing.Image.FromFile(imgPath)), 
                ImagePath = imgPath,
                IsChecked = true
            };
            this.mainWindowViewModel.ThumbnailList.Add(newModel);
            this.lbImgs.SelectedIndex = this.lbImgs.Items.Count - 1;
        }

        public BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                BitmapImage bitImage = new BitmapImage();
                bitImage.BeginInit();
                bitImage.StreamSource = ms;
                bitImage.EndInit();
                return bitImage;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Btn_Rotate(object sender, RoutedEventArgs e)
        {
           
        }

        /// <summary>
        /// 打开属性设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Property(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 切换相机
        /// </summary>
        private void CbCamera_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idx = this.cbCamera.SelectedIndex;
            if (idx != this.index)
            {
                HanWangCameraAPI.HWCloseCamera(index);
                this.index = idx;
                HanWangCameraAPI.HWOpenCamera(index, this.picBox.Handle);

                this.RefreshResoluationi();
            }
        }

        private void RefreshResoluationi()
        {
            this.cbResoluation.Items.Clear();

            int count = HanWangCameraAPI.HWGetResCount(index);
            for (int i = 0; i < count; i++)
            {
                int width = HanWangCameraAPI.HWGetResWidth(index, i);
                int height = HanWangCameraAPI.HWGetResHeight(index, i);
                this.cbResoluation.Items.Add(width.ToString() + "x" + height.ToString());
            }
            this.cbResoluation.SelectedIndex = 0;
        }

        private void CbResoluation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int iRes = this.cbResoluation.SelectedIndex;
            HanWangCameraAPI.HWSetResolution(this.index, iRes);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var a = VisualTreeHelper.GetParent(sender as CheckBox);
            var aa = VisualTreeHelper.GetParent(a);
            if(aa is ListBoxItem)
            {
                this.lbImgs.SelectedItem = (ImageModel)(aa as ListBoxItem).DataContext;
            }
        }

        private void StartListener()
        {
            HttpListener htpLis = new HttpListener();
            htpLis.Prefixes.Add("http://localhost:6300/");
            htpLis.Start();
            while (true)
            {
                HttpListenerContext context = htpLis.GetContext();
                HttpListenerRequest request = context.Request;

                HttpListenerResponse response = context.Response;
                Stream str = response.OutputStream;
                StreamWriter sw = new StreamWriter(str);
                string key = context.Request.QueryString["ImagePath"];
            }
        }

        /// <summary>
        /// http get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        private void HttpGet(string url, string data)
        {
            try
            {
                this.Title += "--正在传送中，请稍后...";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (data == "" ? "" : ("?" + data)));
                request.Method = "Get";
                request.Timeout = 2000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream str = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(str, Encoding.UTF8))
                    {
                        string msg = sr.ReadToEnd();
                    }
                }
                response.Close();
                response.Dispose();
                response = null;
                request = null;
            }
            catch
            { 
            }
        }

        private void requestCompleted(IAsyncResult asyncResult)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                using (Stream str = request.GetResponse().GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(str, Encoding.UTF8))
                    {
                        string msg = sr.ReadToEnd();
                    }
                }
            }
            catch
            { 
            }
        }
    }
}
