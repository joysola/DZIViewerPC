using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Framework.Controls;
using DST.PIMS.Framework.Extensions;
using DST.PIMS.Framework.ScaleTileSource;
using Nico.DeepZoom;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using DST.PIMS.Framework.ExtendContext;
using System.ComponentModel;
using MVVMExtension;
using GalaSoft.MvvmLight.Messaging;
using DST.PIMS.Framework.Model;
using DST.Controls;
using System.Windows.Media.Effects;
using System.Windows.Controls.Primitives;
using DST.Common.Helper;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ImageViewer.xaml 的交互逻辑
    /// </summary>
    [NotifyAspect]
    public partial class ImageViewer : BaseUserControl
    {

        //public static readonly RoutedEvent AddAnnoEvent = EventManager.RegisterRoutedEvent(nameof(AddAnno), RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ImageViewer));
        //public static readonly RoutedEvent UpdateAnnoEvent = EventManager.RegisterRoutedEvent(nameof(UpdateAnno), RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ImageViewer));
        //public static readonly RoutedEvent DeleteAnnoEvent = EventManager.RegisterRoutedEvent(nameof(DeleteAnno), RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(ImageViewer));


        private Point lastMouseDownPos;
        private Point lastMousePos;
        private double MaxScaleZoom = 40.0;
        private bool isDrag;
        private ContentWindow _contentWindow; // 承载的父窗体
        private readonly Dictionary<Popup, bool> _popShowDict = new Dictionary<Popup, bool>(); // pop的状态
        private List<Popup> _pops = new List<Popup>(); // 所有popups集合

        //public event RoutedEventHandler AddAnno
        //{
        //    add { AddHandler(AddAnnoEvent, value); }
        //    remove { RemoveHandler(AddAnnoEvent, value); }
        //}
        //public event RoutedEventHandler UpdateAnno
        //{
        //    add { AddHandler(UpdateAnnoEvent, value); }
        //    remove { RemoveHandler(AddAnnoEvent, value); }
        //}
        //public event RoutedEventHandler DeleteAnno
        //{
        //    add { AddHandler(DeleteAnnoEvent, value); }
        //    remove { RemoveHandler(DeleteAnnoEvent, value); }
        //}
        /// <summary>
        /// 报告选择信息实体
        /// </summary>
        private SelectRprtImgRect selectRprtImg { get; } = new SelectRprtImgRect();
        //private ImageViewerViewModel imageViewerViewModel = null;
        //private AnnoBase _curAnnoBase;
        //public double Curscale { get; set; }
        //private MultiScaleImage msi = null;

        /// <summary>
        /// 标记元素集合
        /// </summary>
        public AnnoInfo AnnoInfos
        {
            get => (AnnoInfo)GetValue(AnnoInfosProperty);
            set => SetValue(AnnoInfosProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnnoInfos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnnoInfosProperty =
            DependencyProperty.Register(nameof(AnnoInfos), typeof(AnnoInfo), typeof(ImageViewer), new PropertyMetadata(null));

        /// <summary>
        /// 标记元素集合
        /// </summary>
        public ShaderEffect MSIEffect
        {
            get => (ShaderEffect)GetValue(MSIEffectProperty);
            set => SetValue(MSIEffectProperty, value);
        }

        // Using a DependencyProperty as the backing store for MSIEffect.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MSIEffectProperty =
            DependencyProperty.Register(nameof(MSIEffect), typeof(ShaderEffect), typeof(ImageViewer), new PropertyMetadata(null));

        /// <summary>
        /// 当前选中的标记
        /// </summary>
        public AnnoBase CurAnnoBase
        {
            get => (AnnoBase)GetValue(CurAnnoBaseProperty);
            set => SetValue(CurAnnoBaseProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnnoInfos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurAnnoBaseProperty =
            DependencyProperty.Register(nameof(CurAnnoBase), typeof(AnnoBase), typeof(ImageViewer), new PropertyMetadata(null, (d, p) =>
            {
                if (d is ImageViewer viewer && p.NewValue is AnnoBase annoBase)
                {
                    //var msi = viewer.msi;
                    var left = Canvas.GetLeft(annoBase);
                    var top = Canvas.GetTop(annoBase);
                    var curPoint = new Point(left + annoBase.ActualWidth / 2, top + annoBase.ActualHeight / 2); // 当前标记中心在canvas中的位置；

                    viewer.LocateZoomImgbyPoint(curPoint); // 定位缩放
                }
            }));

        public static DependencyProperty EditAnnoTargetCommandProperty = DependencyProperty.Register(nameof(EditAnnoTargetCommand), typeof(ICommand), typeof(ImageViewer));

        /// <summary>
        /// 标记框编辑命令
        /// </summary>
        public ICommand EditAnnoTargetCommand
        {
            get => (ICommand)GetValue(EditAnnoTargetCommandProperty);
            set => SetValue(EditAnnoTargetCommandProperty, value);
        }

        /// <summary>
        /// 当前倍率
        /// </summary>
        public double Curscale { get; set; } // 触发NotifyPropertyChanged


        public ImageViewer()
        {
            
            InitializeComponent();
            //this.imageViewerViewModel = new ImageViewerViewModel();
            //this.DataContext = this.imageViewerViewModel;
            this.Loaded += this.ImageViewer_Loaded;
            this.Unloaded += (s, e) => this.Dispose();
            // 切换父层popup时，关闭本身
            this.IsVisibleChanged += (o, e) =>
            {
                if (e.NewValue is bool isnewVis && isnewVis == false && e.OldValue is bool isoldVis && isoldVis == true)
                {
                    CloseAllPopups();
                    if (this.selectRprtImg.IsStarted)
                    {
                        SelectReportImg();
                    }
                }
            };
            this.msi.Visibility = Visibility.Visible;
            this.RegisterMessenger();
        }

        private void RegisterMessenger()
        {
            // 报告视野定位缩放
            Messenger.Default.Register<Point>(this, EnumMessageKey.LocateRptImgViewer, msiPoint =>
            {
                var canPoint = msiPoint.MsiToCanvas(msi, MSIContext.SlideZoom);
                LocateZoomImgbyPoint(canPoint);
            });
            // 刷新标记框
            Messenger.Default.Register<object>(this, EnumMessageKey.RefreshImgViewerAnnos, data =>
            {
                // 只有可视的才有坐标集合
                RefreshImgAnnos();
            });
            // 关闭Popup
            Messenger.Default.Register<object>(this, EnumMessageKey.CloseImgViewerPopups, data =>
            {
                CloseAllPopups();
            });
        }
        #region 刷新或初始化标记
        /// <summary>
        /// 刷新标记框
        /// </summary>
        private void RefreshImgAnnos()
        {
            if (DataContext is ImgVwViewModel viewModel)
            {
                // 1. 删除标记框
                foreach (var anno in this.AnnoInfos.AnnoList)
                {
                    anno.DeleteItem();
                }
                // 2. 清空集合
                this.AnnoInfos.AnnoList.Clear();
                // 3. 重新绘制加入
                foreach (var imgAnno in viewModel?.Annos)
                {
                    var annoRect2 = new AnnoRect(Bg, msi, AnnoInfos.AnnoList.ToList(), MSIContext.SlideZoom, MSIContext.Calibration)
                    {
                        Id = imgAnno.Id,
                        CurStart = imgAnno.CurStart,
                        CurEnd = imgAnno.CurEnd,
                        Anno_Name = imgAnno.Anno_Name, // 名称
                        Description = imgAnno.Description, // 描述
                        ThumbBrush = (Brush)Application.Current.TryFindResource("PrimaryBrush"),
                        //ThumbUrl = viewModel.ImgPathList[0].SampleImgUrl, //aif.ThumbUrl;
                        ThumbImg = imgAnno.ThumbImg,
                    };
                    AnnoInfos.AnnoList.Add(annoRect2);

                    //annoRect2.BorderBrush = new SolidColorBrush(Colors.Green);
                    //annoRect2.SetParam(Bg, msi, AnnoList, 1, 0.24);
                }
                ReDrawAnnos();
            }
        }

        /// <summary>
        ///  重画标记
        /// </summary>
        private void ReDrawAnnos()
        {
            foreach (var anno in AnnoInfos.AnnoList)
            {
                anno.UpdateVisual();
            }
        }
        #endregion 刷新或初始化标记

        /// <summary>
        /// 定位缩放核心算法
        /// </summary>
        /// <param name="curPoint"></param>
        private void LocateZoomImgbyPoint(Point curPoint)
        {
            double x = msi.ZoomableCanvas.Offset.X + curPoint.X - msi.ActualWidth * 0.5; // 当前标记中点减去 msi控件一半距离，即canvas到新的位置的msi控件距离
            double y = msi.ZoomableCanvas.Offset.Y + curPoint.Y - msi.ActualHeight * 0.5;
            msi.ZoomableCanvas.Offset = new Point(x, y);
            msi.ZoomableCanvas.ApplyAnimationClock(ZoomableCanvas.OffsetProperty, null);
            var centerPoint = new Point(msi.ActualWidth / 2, msi.ActualHeight / 2);
            this.ZoomRatio(40.0, centerPoint); // 缩放
        }
        private void ImageViewer_Loaded(object sender, RoutedEventArgs e)
        {
            this.AnnoInfos = new AnnoInfo(); // 初始化，防止使用同一个数据源
            _contentWindow = Application.Current.Windows?.OfType<ContentWindow>()?.FirstOrDefault(/*x => x.Content == this*/); // 获取承载的window，用于记录key事件
            if (_contentWindow != null)
            {
                _contentWindow.PreviewKeyUp += ImgView_KeyUp;
                _contentWindow.Activated += ContentWindow_Activated;
                _contentWindow.Deactivated += ContentWindow_Deactivated;
            }
            _pops = this.FindChildren<Popup>(); // 获取此控件所有popups

            ZoomableCanvas.Refresh += ZoomableCanvas_Refresh;
            this.Bg.Children.Add(selectRprtImg.Rect);
            //this.InitMultiScaleImage(this.imageViewerViewModel.ImageBasePath);
            this.Dispatcher.InvokeAsync(() => this.msi.Effect = (ShaderEffect)this.mainGrid.TryFindResource("shader"));
        }



        public override void Dispose()
        {
            base.Dispose();
            if (this.msi != null)
            {
                if (_contentWindow != null)
                {
                    _contentWindow.PreviewKeyUp -= ImgView_KeyUp;
                    _contentWindow.Activated -= ContentWindow_Activated;
                    _contentWindow.Deactivated -= ContentWindow_Deactivated;
                }
                this.msi.MouseLeftButtonDown -= Msi_MouseLeftButtonDown;
                this.msi.MouseMove -= Msi_MouseMove;
                this.msi.MouseLeftButtonUp -= Msi_MouseLeftButtonUp;
                this.msi.MouseWheel -= Msi_MouseWheel;
                this.Bg.PreviewMouseLeftButtonDown -= Bg_MouseLeftButtonDown;
                this.Bg.PreviewMouseRightButtonDown -= Bg_PreviewMouseRightButtonDown;
                this.Bg.PreviewMouseRightButtonUp -= Bg_PreviewMouseRightButtonUp; ;
                ZoomableCanvas.Refresh -= ZoomableCanvas_Refresh;
                (this.msi.Source as NineLayerFromMinioScaleTileSource)?.Dispose();
                this.Bg.Children.Remove(this.msi);
                this.msi = null;
                GC.Collect();
                GC.Collect();
                GC.Collect();
            }
        }

        #region 标记框操作
        /// <summary>
        /// 删除标记框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteAnnoTarget();
            }
        }
        /// <summary>
        /// 删除标记框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnnoDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteAnnoTarget();
            this.AnnoMenuPop.IsOpen = false;
        }
        /// <summary>
        /// 删除标记框方法
        /// </summary>
        private void DeleteAnnoTarget()
        {
            if (AnnoInfos.Target != null)
            {
                AnnoInfos.Target.DeleteItem();
                AnnoInfos.AnnoList.Remove(AnnoInfos.Target);
                AnnoInfos.Target = null;
            }
        }
        /// <summary>
        /// 编辑标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnnoEdit_Click(object sender, RoutedEventArgs e)
        {
            this.AnnoMenuPop.IsOpen = false;
            EditAnnoTargetCommand?.Execute(AnnoInfos.Target); // 新增后触发
        }
        /// <summary>
        /// 更新Pop位置
        /// </summary>
        private void ReLocateAnnoEditPop()
        {
            if (AnnoInfos.Target != null)
            {
                this.AnnoMenuPop.IsOpen = false;
                this.AnnoMenuPop.IsOpen = true;
            }
        }

        #endregion 标记框操作

        #region Bg Events
        /// <summary>
        /// 标记框绘制完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bg_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var newAnno = AnnoInfos.NewAnno;
            if (AnnoInfos.Target == null && AnnoInfos.IsDrawing && newAnno != null) // 画标记
            {
                this.Bg.PreviewMouseMove -= Bg_PreviewMouseMove;
                AnnoInfos.IsDrawing = false;
                AnnoInfos.NewAnno = null;
                if (newAnno.ActualWidth <= 5 || newAnno.ActualHeight <= 5) // 防止随意点击后新增矩形
                {
                    newAnno.DeleteItem();
                    return;
                }
                AnnoInfos.AnnoList.Add(newAnno);
                //AnnoInfos.Target = null;

                //if (msi.Source is NineLayerFromLocalScaleTileSource tileSource)
                //{
                //    newAnno.ThumbImg = await tileSource.SelectThumbImg(newAnno.CurCenter);
                //}
                // 添加描述
                EditAnnoTargetCommand?.Execute(newAnno);
                // 如果取消了，则删除此标记框
                if (newAnno.IsCanceled)
                {
                    newAnno.DeleteItem();
                    AnnoInfos.AnnoList.Remove(newAnno);
                }
            }
            else if (AnnoInfos.Target != null) // 选中标记框后右击弹出菜单
            {
                ReLocateAnnoEditPop();
            }
        }
        /// <summary>
        /// 开始绘制标记框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bg_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (AnnoInfos.Target == null && !AnnoInfos.IsDrawing && AnnoInfos.NewAnno == null)
            {
                AnnoInfos.IsDrawing = true;
                Point position = e.GetPosition(this.Bg);
                AnnoInfos.OriginPoint = position;
                var annoRect = new AnnoRect(Bg, msi, AnnoInfos.AnnoList.ToList(), MSIContext.SlideZoom, MSIContext.Calibration);
                annoRect.OriStart = position;
                annoRect.OriEnd = annoRect.OriStart;

                annoRect.CurStart = annoRect.CanvasToMsi(position);
                annoRect.CurEnd = annoRect.CurStart;
                annoRect.ThumbBrush = (Brush)Application.Current.TryFindResource("PrimaryBrush");

                AnnoInfos.NewAnno = annoRect;
                this.Bg.PreviewMouseMove += Bg_PreviewMouseMove;
            }

        }
        /// <summary>
        /// 绘制标记框ing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bg_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (AnnoInfos.NewAnno != null)
            {
                var annobase = AnnoInfos.NewAnno;
                var originPoint = AnnoInfos.OriginPoint;
                Point position = e.GetPosition(this.Bg);
                Point originEnd = new Point(Math.Max(originPoint.Value.X, position.X), Math.Max(originPoint.Value.Y, position.Y));
                Point originStart = new Point(Math.Min(originPoint.Value.X, position.X), Math.Min(originPoint.Value.Y, position.Y));
                annobase.SetValue(Canvas.LeftProperty, originStart.X);
                annobase.SetValue(Canvas.TopProperty, originStart.Y);
                annobase.Width = originEnd.X - originStart.X;
                annobase.Height = originEnd.Y - originStart.Y;
                annobase.OriStart = originStart;
                annobase.OriEnd = originEnd;
                annobase.CurStart = annobase.CanvasToMsi(annobase.OriStart);
                annobase.CurEnd = annobase.CanvasToMsi(annobase.OriEnd);
            }
            //else if (isSelectReportImg)
            //{
            //    Point canPoint = e.GetPosition(this.Bg);
            //    var msiPoint = canPoint.CanvasToMsi(msi, MSIContext.SlideZoom);
            //    var x = msiPoint.X / 256;
            //    var y = msiPoint.Y / 256;
            //    var tileSource = msi.Source as NineLayerFromMinioScaleTileSource;
            //    var xx = tileSource.GetTilesAtLevel(msi._spatialSource.CurrentLevel);
            //    //msi._spatialSource.CurrentLevel = msi._spatialSource.CurrentLevel - 1;
            //}

        }

        /// <summary>
        /// 用以判断Bg右击后是否命中标记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectRprtImg.IsStarted) // 选取主图时，不需要命中测试
            {
                return;
            }
            var point = e.GetPosition(this.Bg);
            var list = new List<AnnoBase>(); // 存储命中的标记
            VisualTreeHelper.HitTest((Visual)sender,
                // 过滤委托
                tagObj =>
                {
                    if (tagObj is MultiScaleImage) // 跳过瓦片
                    {
                        return HitTestFilterBehavior.ContinueSkipSelfAndChildren;
                    }
                    else if (tagObj is AnnoBase anno) // 只过滤标记
                    {
                        return HitTestFilterBehavior.ContinueSkipChildren; // 加入命中
                    }
                    else
                    {
                        return HitTestFilterBehavior.ContinueSkipSelf; // 跳过非标记
                    }
                },
                // 返回命中结果委托
                hitres =>
                {
                    if (hitres.VisualHit is AnnoBase anno)
                    {
                        list.Add(anno);
                    }
                    return HitTestResultBehavior.Continue;
                }, new PointHitTestParameters(point));
            // 多个命中标记
            AnnoBase curNearAnno = null; // 离鼠标点最近的矩形
            if (list.Count > 1)
            {
                double pointDis = 0.0; // 离矩形的左上的距离

                list.ForEach(anno =>
                {
                    Point leftTop = new Point(Canvas.GetLeft(anno), Canvas.GetTop(anno));
                    Point rightBottom = new Point(leftTop.X + anno.Width, leftTop.Y + anno.Height);
                    if (point.X <= rightBottom.X && point.X >= leftTop.X && point.Y <= rightBottom.Y && point.Y >= leftTop.Y)
                    {
                        double tmpDis = Math.Pow(point.X - leftTop.X, 2) + Math.Pow(point.Y - leftTop.Y, 2);
                        if (pointDis == 0 || pointDis > tmpDis)
                        {
                            pointDis = tmpDis;
                            curNearAnno = anno;
                        }
                    }
                });
                //AnnoInfos.Target = curNearAnno;
            }
            else if (list.Count == 1)
            {
                curNearAnno = list[0];
                //AnnoInfos.Target = list[0];
                this.AnnoListPop.IsOpen = false; // 选中标记框后关闭切片列表
            }
            else
            {
                curNearAnno = null;
            }
            AnnoInfos.Target = curNearAnno;

            if (AnnoMenuPop.IsOpen == true)
            {
                AnnoMenuPop.IsOpen = false;
            }
            await Task.Delay(10);
        }
        #endregion Bg Events

        #region Msi Events
        /// <summary>
        /// 加载完成并Refresh完成后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Msi_Ini(object sender, RoutedEventArgs e)
        {
            if (this.msi != null && this.msi.Source != null)
            {
                RefreshImgAnnos(); // 刷新标记框
                double num = Math.Min(this.msi._itemsControl.ActualWidth / this.msi.Source.ImageSize.Width, this.msi._itemsControl.ActualHeight / this.msi.Source.ImageSize.Height);
                msi.ZoomableCanvas.Scale = num;
                // 0.57895351358564218 是个估算的定值
                msi.ZoomableCanvas.Offset = new Point(this.msi.Source.ImageSize.Width * 0.5 * num * MSIContext.ImgScrnParam - msi.ZoomableCanvas.ActualWidth * 0.5, this.msi.Source.ImageSize.Height * 0.5 * num * MSIContext.ImgScrnParam - msi.ZoomableCanvas.ActualHeight * 0.5);

            }
        }
        private void Msi_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isDrag = false;
        }

        private void Msi_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectRprtImg.IsStarted) // 选取主图时，不需要拖动
            {
                return;
            }
            Keyboard.Focus(this);
            this.lastMouseDownPos = e.GetPosition(msi);
            this.isDrag = true;

        }

        private void Msi_MouseMove(object sender, MouseEventArgs e)
        {
            lastMousePos = e.GetPosition(msi);

            if (isDrag)
            {
                double x = msi.ZoomableCanvas.Offset.X + (lastMouseDownPos.X - lastMousePos.X);
                double y = msi.ZoomableCanvas.Offset.Y + (lastMouseDownPos.Y - lastMousePos.Y);
                msi.ZoomableCanvas.Offset = new Point(x, y);
                msi.ZoomableCanvas.ApplyAnimationClock(ZoomableCanvas.OffsetProperty, null);
                lastMouseDownPos = lastMousePos;
                Console.WriteLine("msi.ZoomableCanvas.Offset=" + msi.ZoomableCanvas.Offset.X + "," + msi.ZoomableCanvas.Offset.Y);
            }
        }

        int previewTime = 0;
        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        private void Msi_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (this.msi._spatialSource.CurrentLevel == 9 && e.Delta < 0) // 9层以下不显示
            {
                return;
            }
            if (this.msi._spatialSource.CurrentLevel == 16 && e.Delta > 0 && this.Curscale > MSIContext.MaxScale) // 最大层数默认16
            {
                return;
            }
            if (this.previewTime != 0)
            {
                int ts = e.Timestamp - this.previewTime;
                this.previewTime = e.Timestamp;
            }
            Point position = e.GetPosition(this.Bg); // 直接获取在Bg中的位置，不用再转换...
            //Point position = e.GetPosition(this.canBaseGrid); // 以canBaseGrid为基准（此Grid和Canvas位置重合尺寸相等）

            double tmpScale = this.Curscale;
            double splitScale = this.msi._spatialSource.CurrentLevel <= 12 ? 0.068745 : 0.968745;
            tmpScale = (e.Delta <= 0) ? (tmpScale - splitScale) : (tmpScale + splitScale);
            ZoomRatio(tmpScale, position);
        }
        #endregion Msi Events

        #region ZoomableCanvas
        private void ZoomableCanvas_Refresh(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Refresh();
        }


        /// <summary>
        /// msi刷新
        /// </summary>
        private void Refresh()
        {
            if (this.msi.ZoomableCanvas == null)
            {
                return;
            }

            this.Curscale = msi.ZoomableCanvas.Scale * this.MaxScaleZoom;

            //if (isFirst && this.msi.ZoomableCanvas.Offset.X != 0)
            //{
            //    isFirst = false;
            //    double num = Math.Min(this.msi._itemsControl.ActualWidth / this.msi.Source.ImageSize.Width, this.msi._itemsControl.ActualHeight / this.msi.Source.ImageSize.Height);
            //    msi.ZoomableCanvas.Scale = num;
            //    //msi.ZoomableCanvas.Offset = new Point(this.msi.Source.ImageSize.Width * 0.5 * num - msi.ZoomableCanvas.ActualWidth * 0.5, this.msi.Source.ImageSize.Height * 0.5 * num - msi.ZoomableCanvas.ActualHeight * 0.5);
            //    //this.msi.ZoomableCanvas.Scale * this.msi.Source.ImageSize.Width * 0.61141
            //    // 0.57895351358564218 是个估算的定值
            //    msi.ZoomableCanvas.Offset = new Point(this.msi.Source.ImageSize.Width * 0.5 * num * MSIContext.ImgScrnParam - msi.ZoomableCanvas.ActualWidth * 0.5, this.msi.Source.ImageSize.Height * 0.5 * num * MSIContext.ImgScrnParam - msi.ZoomableCanvas.ActualHeight * 0.5);
            //    //this.Bg.Margin = new Thickness(0, -(ActualWidth - ActualHeight) / 2, 0, 0);
            //}
            navmap.UpdateThumbnailRect();
            ReDrawAnnos(); // 重画标记
            ReDrawSelectRprtImg();
            ReLocateAnnoEditPop();
        }

        /// <summary>
        /// 缩放函数
        /// </summary>
        /// <param name="zoom_ratio"></param>
        /// <param name="point"></param>
        public void ZoomRatio(double zoom_ratio, Point point)
        {
            if (zoom_ratio < 0)
            {
                return;
            }
            Point point3 = msi.ElementToLogicalPoint(point); // Bg自身的点所以直接使用无需转换
            // 参照物旋转，中心是baseGrid的中心
            //Point point = new Point(x, y);
            //Point center = new Point(this.Bg.ActualWidth / 2, this.Bg.ActualWidth / 2);
            //Point pointM = point.GetRotatePoint(center, this.rotater.Value);
            //Point point3 = msi.ElementToLogicalPoint(pointM);
            msi.ZoomAboutLogicalPoint(zoom_ratio / Curscale, point3.X, point3.Y);
            Curscale = zoom_ratio;
        }
        #endregion ZoomableCanvas


        #region 报告视野相关
        /// <summary>
        /// 选取报告视野
        /// </summary>
        public void SelectReportImg()
        {
            selectRprtImg.IsStarted = !selectRprtImg.IsStarted;
            this.Dispatcher.InvokeAsync(() =>
            {
                foreach (var anno in AnnoInfos.AnnoList)
                {
                    anno.Visibility = selectRprtImg.IsStarted ? Visibility.Hidden : Visibility.Visible;
                }
            });
            if (selectRprtImg.IsStarted)
            {
                selectRprtImg.HasFinished = false;
                selectRprtImg.Rect.Visibility = Visibility.Visible;
                this.Bg.PreviewMouseMove += Bg_SelectRprtImgPreviewMouseMove;
                this.Bg.PreviewMouseLeftButtonUp += Bg_PreviewMouseLeftButtonUp;
            }
            else
            {
                selectRprtImg.Rect.Visibility = Visibility.Hidden;
                this.Bg.PreviewMouseMove -= Bg_SelectRprtImgPreviewMouseMove;
                this.Bg.PreviewMouseLeftButtonUp -= Bg_PreviewMouseLeftButtonUp;
            }
        }
        /// <summary>
        /// 重画报告视野矩形
        /// </summary>
        private void ReDrawSelectRprtImg()
        {
            var canStartPoint = selectRprtImg.MsiStratPoint.MsiToCanvas(msi);
            var canEndPoint = selectRprtImg.MsiEndPoint.MsiToCanvas(msi);

            Canvas.SetLeft(selectRprtImg.Rect, canStartPoint.X);
            Canvas.SetTop(selectRprtImg.Rect, canStartPoint.Y);
            selectRprtImg.Rect.Width = canEndPoint.X - canStartPoint.X;
            selectRprtImg.Rect.Height = canEndPoint.Y - canStartPoint.Y;
        }
        /// <summary>
        /// 报告视野矩形移动事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bg_SelectRprtImgPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!selectRprtImg.HasFinished)
            {
                Point point = e.GetPosition(this.Bg);
                var msiPoint = point.CanvasToMsi(msi);

                var startMsiPoint = new Point(msiPoint.X - MSIContext.RprtImgSize / 2, msiPoint.Y - MSIContext.RprtImgSize / 2);
                var endMsiPoint = new Point(msiPoint.X + MSIContext.RprtImgSize / 2, msiPoint.Y + MSIContext.RprtImgSize / 2);
                //var startMsiPoint = new Point(msiPoint.X - 398 , msiPoint.Y - 398);
                //var endMsiPoint = new Point(msiPoint.X + 398 , msiPoint.Y + 398 );
                selectRprtImg.MsiStratPoint = startMsiPoint;
                selectRprtImg.MsiEndPoint = endMsiPoint;

                var startCanPoint = startMsiPoint.MsiToCanvas(msi);
                var endCanPoint = endMsiPoint.MsiToCanvas(msi);

                Canvas.SetLeft(selectRprtImg.Rect, startCanPoint.X);
                Canvas.SetTop(selectRprtImg.Rect, startCanPoint.Y);
                selectRprtImg.Rect.Width = endCanPoint.X - startCanPoint.X;
                selectRprtImg.Rect.Height = endCanPoint.Y - startCanPoint.Y;
            }
        }
        /// <summary>
        /// 报告视野完成事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bg_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectRprtImg.IsStarted)
            {
                var result = ConfirmMessageBox.Show("提示", "是否要确认选择此处为报告视野主图？", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    selectRprtImg.HasFinished = true;
                    this.Bg.PreviewMouseMove -= Bg_SelectRprtImgPreviewMouseMove;
                    this.Bg.PreviewMouseLeftButtonUp -= Bg_PreviewMouseLeftButtonUp;
                    var canPoint2 = selectRprtImg.MsiCenterPoint.MsiToCanvas(msi, MSIContext.SlideZoom);

                    //Point canPoint = e.GetPosition(this.Bg);
                    //var msiPoint = canPoint.CanvasToMsi(msi, MSIContext.SlideZoom);
                    //var x = msiPoint.X / 256;
                    //var y = msiPoint.Y / 256;
                    //Console.WriteLine($"8:{x},{y}");
                    //Console.WriteLine($"7:{x / 2},{y / 2}");
                    //await tileSource.SelectReportImg((int)x / 2, (int)y / 2).ConfigureAwait(false);
                    var tileSource = msi.Source as NineLayerFromMinioScaleTileSource;
                    await tileSource.SelectReportImg(selectRprtImg.MsiCenterPoint, 8);
                }
                else
                {
                    SelectReportImg();
                }
            }
        }
        #endregion 报告视野相关

        #region 控件功能开关
        /// <summary>
        /// 设置总开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImgFuncBtn_Click(object sender, RoutedEventArgs e)
        {
            ImgFuncPop.IsOpen = !ImgFuncPop.IsOpen;
        }
        /// <summary>
        /// 报告视野开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectReportImg_Click(object sender, RoutedEventArgs e)
        {
            SelectReportImg();
        }
        /// <summary>
        /// 白平衡开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhiteBalance_Click(object sender, RoutedEventArgs e)
        {
            WBPop.IsOpen = !WBPop.IsOpen;
        }
        /// <summary>
        /// 导航开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            navmap.Visibility = navmap.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }
        /// <summary>
        /// 旋转开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RotateBtn_Click(object sender, RoutedEventArgs e)
        {
            rotater.Visibility = rotater.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }
        /// <summary>
        /// 标尺开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RulerBtn_Click(object sender, RoutedEventArgs e)
        {
            scaleRuler.Visibility = scaleRuler.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }
        /// <summary>
        /// 打开标记列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnnoList_Click(object sender, RoutedEventArgs e)
        {
            this.AnnoListPop.IsOpen = !this.AnnoListPop.IsOpen;
        }
        /// <summary>
        /// 关闭所有pop
        /// </summary>
        private void CloseAllPopups()
        {
            //ImgFuncPop.IsOpen = false;
            //WBPop.IsOpen = false;
            _pops?.ForEach(p => p.IsOpen = false);
        }
        /// <summary>
        /// 承载窗体未激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentWindow_Deactivated(object sender, EventArgs e)
        {
            _popShowDict.Clear();
            foreach (var pop in _pops)
            {
                _popShowDict.Add(pop, pop.IsOpen); // 记录当前pop状态
                pop.IsOpen = false; // 暂时关闭
            }
        }
        /// <summary>
        /// 承载窗体激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentWindow_Activated(object sender, EventArgs e)
        {
            foreach (var pop in _popShowDict.Keys)
            {
                pop.IsOpen = _popShowDict[pop]; // 还原pop状态
            }
        }
        #endregion 控件功能开关

        private void CloseAllPopups(object sender, MouseButtonEventArgs e)
        {
            CloseAllPopups();
        }


    }
}
