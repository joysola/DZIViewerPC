using DST.PIMS.Framework.Extensions;
using Nico.DeepZoom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// AnnoRect.xaml 的交互逻辑
    /// </summary>
    public partial class AnnoRect : AnnoBase
    {
        private const double thumb_w = 10;
        private const double thumb_c = 15;

        public int CornerWidth
        {
            get => (int)GetValue(CornerWidthProperty);
            set => SetValue(CornerWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for CornerWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerWidthProperty =
            DependencyProperty.Register(nameof(CornerWidth), typeof(int), typeof(AnnoRect), new PropertyMetadata((int)thumb_w));

        public Brush ThumbBrush
        {
            get => (Brush)GetValue(ThumbBrushProperty);
            set => SetValue(ThumbBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for ThumbBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbBrushProperty =
            DependencyProperty.Register(nameof(ThumbBrush), typeof(Brush), typeof(AnnoRect), new PropertyMetadata(new SolidColorBrush(Colors.Red)));



        public AnnoRect(Canvas canvas, MultiScaleImage msi, List<AnnoBase> objectlist, int slideZoom, double calibration)
            : base(canvas, msi, objectlist, slideZoom, calibration)
        {
            InitializeComponent();
            // 缩小矩形的大小，以适应8个thumb
            double actualMargin = Math.Abs((CornerWidth - StrokeThickness) / 2.0);
            this.MainRect.Margin = new Thickness(actualMargin);
        }
        /// <summary>
        /// 初始化标记
        /// </summary>
        protected override void InitAnno()
        {
            base.IsFinish = true;
            Point start = this.MsiToCanvas(base.CurStart);
            Point end = this.MsiToCanvas(base.CurEnd);

            this.SetValue(Canvas.LeftProperty, start.X);
            this.SetValue(Canvas.TopProperty, start.Y);
            this.Width = end.X - start.X;
            this.Height = end.Y - start.Y;
            base.FiguresCanvas.Children.Add(this);
            ThumbVisibility = Visibility.Collapsed; // 默认不显示
            BorderBrush = new SolidColorBrush(Colors.Red);
            base.ObjList.Insert(0, this);

        }


        public override void UpdateVisual()
        {
            double x = this.MsiToCanvas(base.CurStart).X;
            double y = this.MsiToCanvas(base.CurStart).Y;
            double x2 = this.MsiToCanvas(base.CurEnd).X;
            double y2 = this.MsiToCanvas(base.CurEnd).Y;
            double width = Math.Abs(x - x2);
            double height = Math.Abs(y - y2);
            x = Math.Min(x, x2);
            y = Math.Min(y, y2);
            //MainRect.StrokeThickness = base.StrokeThickness;
            //MainRect.Stroke = base.BorderBrush;
            //MainRect.Width = width;
            //MainRect.Height = height;
            //Canvas.SetLeft(MainRect, x);
            //Canvas.SetTop(MainRect, y);


            this.Width = width;
            this.Height = height;
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
        }

        public override void RegisterThumbEvent()
        {
            CustomThumbs.ForEach(thumb =>
            {
                thumb.DragCompleted += DragCompleted;
                thumb.DragDelta += ResetLocation;
            });
            ThumbVisibility = Visibility.Visible;
        }

        public override void UnRegisterThumbEvent()
        {
            CustomThumbs.ForEach(thumb =>
            {
                thumb.DragCompleted -= DragCompleted;
                thumb.DragDelta -= ResetLocation;
            });
            ThumbVisibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 删除自身
        /// </summary>
        public override void DeleteItem()
        {
            this.UnRegisterThumbEvent();
            this.FiguresCanvas.Children.Remove(this);
            this.ObjList.Remove(this);
        }
        /// <summary>
        /// 创建Thumb
        /// </summary>
        /// <summary>
        /// 选中矩形事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect)
            {
                IsSelected = true;
                //IsActive(Visibility.Visible);
                //var unSelected = base.ObjList.Where(x => x is AnnoRect arect && arect.MainRect != rect);
                //foreach (var item in unSelected)
                //{
                //    item.IsActive(Visibility.Collapsed);
                //}
                //if (item.ControlName == rectangle.Name)
                //{
                //    item.IsActive(Visibility.Visible);
                //    base.AnnoControl.CB.SelectedIndex = base.ObjList.IndexOf(this);
                //}
            }
        }
        /// <summary>
        /// thumb移动完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragCompleted(object sender, DragCompletedEventArgs e)
        {
            double x = Math.Min(base.CurStart.X, base.CurEnd.X);
            double x2 = Math.Max(base.CurStart.X, base.CurEnd.X);
            double y = Math.Min(base.CurStart.Y, base.CurEnd.Y);
            double y2 = Math.Max(base.CurStart.Y, base.CurEnd.Y);
            base.CurStart = new Point(x, y);
            base.CurEnd = new Point(x2, y2);
            base.OriStart = this.MsiToCanvas(base.CurStart);
            base.OriEnd = this.MsiToCanvas(base.CurEnd);
        }
        /// <summary>
        /// Thumb移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetLocation(object sender, DragDeltaEventArgs e)
        {
            if (sender is CustomThumb customThumb)
            {
                ResetLocation(customThumb.DragDirection, e.HorizontalChange, e.VerticalChange);
            }
        }

        private void RectMouseEnter(object sender, MouseEventArgs e)
        {
            //IsActive(Visibility.Visible);
        }

        private void RectMouseLeave(object sender, MouseEventArgs e)
        {
            //IsActive(Visibility.Collapsed);
        }
    }
}
