using DST.Controls.Base;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Helper;
using Nico.Common;
using Nico.DeepZoom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DST.PIMS.Framework.Controls
{
    /// <summary>
    /// Navmap.xaml 的交互逻辑
    /// </summary>
    public partial class Navmap : UserControl
    {
        /// <summary>
        /// 是否是第一次加载
        /// </summary>
        private static bool IsFirstLoad { get; set; } = true;
        /// <summary>
        /// image图像的宽度
        /// </summary>
        private static double ThumbWidth { get; set; }
        /// <summary>
        /// image图像的高度
        /// </summary>
        private static double ThumbHeight { get; set; }
        /// <summary>
        /// 是否正在拖拽膜层
        /// </summary>
        private bool IsDragging { get; set; }
        /// <summary>
        /// 膜层的拖拽起点
        /// </summary>
        private Point DragStartPoint { get; set; }
        /// <summary>
        /// MSI控件
        /// </summary>
        public MultiScaleImage MSI
        {
            get => (MultiScaleImage)GetValue(MSIProperty);
            set => SetValue(MSIProperty, value);
        }

        // Using a DependencyProperty as the backing store for MSI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MSIProperty =
            DependencyProperty.Register(nameof(MSI), typeof(MultiScaleImage), typeof(Navmap), new PropertyMetadata(null));

        /// <summary>
        /// 缩略图图片源
        /// </summary>
        public byte[] ThumbnailSource
        {
            get => (byte[])GetValue(ThumbnailSourceProperty);
            set => SetValue(ThumbnailSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for ThumbnailSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbnailSourceProperty =
            DependencyProperty.Register(nameof(ThumbnailSource), typeof(byte[]), typeof(Navmap), new PropertyMetadata(null, async (d, p) =>
             {
                 if (d is Navmap navmap && p.NewValue is byte[] bytes)
                 {
                     BitmapImage image = new BitmapImage();
                     //byte[] bytes = null;

                     //if (uri.Scheme == "file") // 文件
                     //{
                     //    bytes = File.ReadAllBytes(uri.LocalPath);
                     //}
                     //else if (uri.Scheme == "https" || uri.Scheme == "http") // http请求
                     //{
                     //    bytes = await HttpClientHelper.DownFile(uri); // 下载缩略图文件
                     //}

                     if (bytes == null)
                     {
                         return;
                     }
                     using (var stream = new MemoryStream(bytes))
                     {
                         stream.Position = 0; // 不可少
                         image.BeginInit();
                         image.CreateOptions = BitmapCreateOptions.PreservePixelFormat; // 必须在BeginInit后设置
                         image.CacheOption = BitmapCacheOption.OnLoad;// 必须在BeginInit后设置
                         image.StreamSource = stream;
                         image.EndInit();
                     }
                     image.Freeze();
                     navmap.imgThumbnail.Source = image; // 填充图像元素
                     var imgWidth = image.PixelWidth; // 像素大小
                     var imgHeight = image.PixelHeight; // 像素大小

                     //if (IsFirstLoad) // 第一次加载
                     //{
                     ThumbWidth = imgWidth;
                     ThumbHeight = imgHeight;
                     IsFirstLoad = false;
                     //}
                     navmap.imgThumbnail.Width = imgWidth;
                     navmap.imgThumbnail.Height = imgHeight;

                     navmap.Width = ThumbWidth;
                     navmap.Height = ThumbHeight;
                     navmap.thumbnailCanvas.Width = ThumbWidth;
                     navmap.thumbnailCanvas.Height = ThumbHeight;

                     var geometry = new RectangleGeometry();
                     geometry.SetValue(RectangleGeometry.RectProperty, new Rect(0.0, 0.0, ThumbWidth + 2.0, ThumbHeight + 2.0));
                     navmap.thumbnailCanvas.Clip = geometry;
                     Rect thumbnailRect = navmap.GetThumbnailRect(ThumbWidth, ThumbHeight); // 获取msi对应与thumb中的矩形
                     navmap.DrawRect(thumbnailRect);
                 }
             }));

        public Navmap()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 获取视口对应的膜层矩形
        /// </summary>
        /// <param name="thumbImgWidth"></param>
        /// <param name="thumbImgHeight"></param>
        /// <returns></returns>
        private Rect GetThumbnailRect(double thumbImgWidth, double thumbImgHeight)
        {
            if (MSI.ZoomableCanvas == null || MSI.Source == null)
            {
                return default;
            }
            double msiCtlWidth = MSI.ActualWidth; // msi控件宽度
            double msiCtlHeight = MSI.ActualHeight;
            var offset = MSI.ZoomableCanvas.Offset;

            // 公式1
            // thumbCtlWidth / thumbImgWidth == msiCtlWidth / curMsiImgWidth
            // thumbCtlWidth = thumbImgWidth * (msiCtlWidth / curMsiImgWidth)
            var curMsiImgWidth = MSI.Source.ImageSize.Width * MSIContext.ImgScrnParam * MSI.ZoomableCanvas.Scale; // 当前的msi图像的像素
            double scale = msiCtlWidth / curMsiImgWidth;

            double thumbCtlWidth = thumbImgWidth * scale; // 获取对应thumb控件宽度（等比缩放msi控件大小）
            double thumbCtlHeight = msiCtlHeight / msiCtlWidth * thumbCtlWidth; // 

            // 公式2
            // thumbOffset / msiOffset == thumbCtl / msiCtlWidth;
            double thumbOffsetX = offset.X * thumbCtlWidth / msiCtlWidth; // 获取起点在thumb控件中的偏移量
            double thimbOffestY = offset.Y * thumbCtlHeight / msiCtlHeight;
            Point offsetPoint = new Point(thumbOffsetX, thimbOffestY);
            return new Rect(offsetPoint.X, offsetPoint.Y, thumbCtlWidth, thumbCtlHeight);
        }
        /// <summary>
        /// 同步调整膜层curRectView的位置
        /// </summary>
        /// <param name="rect"></param>
        private void DrawRect(Rect rect)
        {
            this.curRectView.Width = rect.Width;
            this.curRectView.Height = rect.Height;
            Canvas.SetLeft(this.curRectView, rect.X);
            Canvas.SetTop(this.curRectView, rect.Y);
        }
        /// <summary>
        /// 更新缩略图
        /// </summary>
        public void UpdateThumbnailRect()
        {
            if (MSI != null && !IsDragging && ThumbWidth > 0)
            {
                Rect thumbnailRect = GetThumbnailRect(ThumbWidth, ThumbHeight);
                ChangeRectLocation(thumbnailRect.X, thumbnailRect.Y, thumbnailRect.Width, thumbnailRect.Height);
            }
        }
        /// <summary>
        /// 更新膜层curRectView位置
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void ChangeRectLocation(double left, double top, double width, double height)
        {
            Canvas.SetLeft(this.curRectView, left + 1.0);
            Canvas.SetTop(this.curRectView, top + 1.0);
            this.curRectView.Width = width;
            this.curRectView.Height = height;
        }

        #region curRectView events
        /// <summary>
        /// 膜层左击，拖拽的开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rectView)
            {
                if (rectView.CaptureMouse())
                {
                    IsDragging = true;
                    DragStartPoint = e.GetPosition(null);
                }
                e.Handled = true;
            }
        }
        /// <summary>
        /// 膜层拖拽结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rectView)
            {
                IsDragging = false;
                rectView.ReleaseMouseCapture();
                e.Handled = true;
            }
        }
        /// <summary>
        /// curRectView矩形框（膜层）移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RectView_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Rectangle curRect)
            {
                if (IsDragging)
                {
                    double msiCtlWidth = MSI.ActualWidth;
                    double msiCtlHeight = MSI.ActualHeight;

                    Point position = e.GetPosition(null);
                    double width = position.X - DragStartPoint.X; // 鼠标x轴移动的距离
                    double height = position.Y - DragStartPoint.Y; // 鼠标y轴移动的距离
                    double newLeft = width + Canvas.GetLeft(curRect); // 移动后在canvas中新的像素起点x
                    double newTop = height + Canvas.GetTop(curRect);
                    Canvas.SetLeft(curRect, newLeft); // 设置curRectView矩形框（膜层）的新位置
                    Canvas.SetTop(curRect, newTop);
                    DragStartPoint = position; // 更新起点

                    // 公式 rectView的offset / rectView.ActualWidth == msi的offset / msiCtlWidth
                    Point offset = default;
                    offset.X = (newLeft - 1.0) / curRect.ActualWidth * msiCtlWidth;
                    offset.Y = (newTop - 1.0) / curRect.ActualHeight * msiCtlHeight;
                    //offset.Y = (newLeft - 1.0) / rectView.ActualWidth * msiCtlWidth - num3;

                    MSI.ZoomableCanvas.Offset = offset;
                    MSI.ZoomableCanvas.ApplyAnimationClock(ZoomableCanvas.OffsetProperty, null);
                }
            }
        }

        private void RectView_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Rectangle rectView)
            {
                IsDragging = false;
                rectView.ReleaseMouseCapture();
            }
        }
        #endregion curRectView events

        /// <summary>
        /// ThumbnailCanvas的鼠标左击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThumbnailCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                double msiCtlWidth = MSI.ActualWidth;
                double msiCtlHeight = MSI.ActualHeight;
                e.Handled = true;

                var position = e.GetPosition(canvas); // 鼠标点击的位置

                double pX = position.X - this.curRectView.ActualWidth / 2.0; // 最终位置，应该是将膜层的中心移到鼠标位置
                double pY = position.Y - this.curRectView.ActualHeight / 2.0;
                // 公式 thumbCtl中的点 / this.curRectView.ActualWidth == msiCtl中的点 / msiCtlWidth
                Point offset = default;
                offset.X = (pX - 1.0) / this.curRectView.ActualWidth * msiCtlWidth;
                offset.Y = (pY - 1.0) / this.curRectView.ActualHeight * msiCtlHeight;
                //offset.Y = (pY - 1.0) / this.curRectView.ActualWidth * msiCtlWidth;

                MSI.ZoomableCanvas.Offset = offset;
                MSI.ZoomableCanvas.ApplyAnimationClock(ZoomableCanvas.OffsetProperty, null);
                UpdateThumbnailRect();
                //ClearMemoryThread();
            }
        }
    }
}
