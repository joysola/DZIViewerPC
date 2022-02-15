using DST.PIMS.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;

namespace DST.PIMS.Framework.Controls.xx
{
    public class AnnoRect : AnnoBase
    {
        private const double thumb_w = 10;
        private const double thumb_c = 15;

        private Point AnnoPoint { get; set; }
        /// <summary>
        /// 主矩形
        /// </summary>
        public Rectangle MainRect { get; set; } = new Rectangle();

        public string ThumbImgUrl { get; set; }
        #region Thumbs
        public Thumb ThumbB { get; set; }

        public Thumb ThumbL { get; set; }

        public Thumb ThumbLB { get; set; }

        public Thumb ThumbLT { get; set; }

        public Thumb ThumbR { get; set; }

        public Thumb ThumbRB { get; set; }

        public Thumb ThumbRT { get; set; }

        public Thumb ThumbT { get; set; }

        public Thumb ThumbMove { get; set; }
        #endregion Thumbs
        /// <summary>
        /// 初始化标记
        /// </summary>
        protected override void InitAnno()
        {
            base.IsFinish = true;
            Point start = MsiToCanvas(base.CurStart);
            Point end = MsiToCanvas(base.CurEnd);
            MainRect.SetValue(Canvas.LeftProperty, start.X);
            MainRect.SetValue(Canvas.TopProperty, start.Y);
            MainRect.Width = end.X - start.X;
            MainRect.Height = end.Y - start.Y;
            //MainRect.StrokeThickness = base.StrokeThickness;
            MainRect.Stroke = base.BorderBrush;
            MainRect.MouseLeftButtonDown += Select_MouseDown;
            MainRect.MouseEnter += GotFocus;
            base.FiguresCanvas.Children.Add(MainRect);
            CreateThumb(); // 创建Thumbs
            IsActive(Visibility.Collapsed); // 默认不显示
            base.ObjList.Insert(0, this);
        }

        protected override void DeleteItem()
        {
            FiguresCanvas.Children.Remove(MainRect);
            //FiguresCanvas.Children.Remove(base.MTextBlock);
            FiguresCanvas.Children.Remove(ThumbB);
            FiguresCanvas.Children.Remove(ThumbL);
            FiguresCanvas.Children.Remove(ThumbLB);
            FiguresCanvas.Children.Remove(ThumbLT);
            FiguresCanvas.Children.Remove(ThumbR);
            FiguresCanvas.Children.Remove(ThumbMove);
            FiguresCanvas.Children.Remove(ThumbRT);
            FiguresCanvas.Children.Remove(ThumbT);
            FiguresCanvas.Children.Remove(ThumbRB);
            base.ObjList.Remove(this);
        }
        /// <summary>
        /// 创建Thumb
        /// </summary>
        protected override void CreateThumb()
        {
            if (ThumbB == null)
            {
                ThumbB = NewThumb(thumb_w, thumb_w,
                    MainRect.Width / 2.0 - thumb_w / 2.0, MainRect.Height - thumb_w / 2.0,
                    Direction.Bottom);
                ThumbL = NewThumb(thumb_w, thumb_w,
                    -thumb_w / 2.0, MainRect.Height / 2.0 - thumb_w / 2.0,
                    Direction.Left);
                ThumbLB = NewThumb(thumb_w, thumb_w,
                    -thumb_w / 2.0, MainRect.Height - thumb_w / 2.0,
                    Direction.LeftBottom);
                ThumbLT = NewThumb(thumb_w, thumb_w,
                    -thumb_w / 2.0, -thumb_w / 2.0,
                    Direction.LeftTop);
                ThumbR = NewThumb(thumb_w, thumb_w,
                    MainRect.Width - thumb_w / 2.0, MainRect.Height / 2.0 - thumb_w / 2.0,
                    Direction.Right);
                ThumbRB = NewThumb(thumb_w, thumb_w,
                    MainRect.Width - thumb_w / 2.0, MainRect.Height - thumb_w / 2.0,
                    Direction.RightBottom);
                ThumbRT = NewThumb(thumb_w, thumb_w,
                    MainRect.Width - thumb_w / 2.0, -thumb_w / 2.0,
                    Direction.RightTop);
                ThumbT = NewThumb(thumb_w, thumb_w,
                    MainRect.Width / 2.0 - thumb_w / 2.0, -thumb_w / 2.0,
                    Direction.Top);
                ThumbMove = NewThumb(thumb_c, thumb_c,
                    MainRect.Width / 2.0 - thumb_c / 2.0, MainRect.Height / 2.0 - thumb_c / 2.0,
                    Direction.Center);
            }
        }


        /// <summary>
        /// 更新显示
        /// </summary>
        public override void UpdateVisual()
        {
            if (ThumbMove != null)
            {
                double x = MsiToCanvas(base.CurStart).X;
                double y = MsiToCanvas(base.CurStart).Y;
                double x2 = MsiToCanvas(base.CurEnd).X;
                double y2 = MsiToCanvas(base.CurEnd).Y;
                double width = Math.Abs(x - x2);
                double height = Math.Abs(y - y2);
                x = Math.Min(x, x2);
                y = Math.Min(y, y2);
                UpdateThumbVisual(ThumbB, x + width / 2.0 - thumb_w / 2.0, y + height - thumb_w / 2.0);
                UpdateThumbVisual(ThumbL, x - thumb_w / 2.0, y + height / 2.0 - thumb_w / 2.0);
                UpdateThumbVisual(ThumbLB, x - thumb_w / 2.0, y + height - thumb_w / 2.0);
                UpdateThumbVisual(ThumbLT, x - thumb_w / 2.0, y - thumb_w / 2.0);
                UpdateThumbVisual(ThumbR, x + width - thumb_w / 2.0, y + height / 2.0 - thumb_w / 2.0);
                UpdateThumbVisual(ThumbRB, x + width - thumb_w / 2.0, y + height - thumb_w / 2.0);
                UpdateThumbVisual(ThumbRT, x + width - thumb_w / 2.0, y - thumb_w / 2.0);
                UpdateThumbVisual(ThumbT, x + width / 2.0 - thumb_w / 2.0, y - thumb_w / 2.0);
                UpdateThumbVisual(ThumbMove, x + width / 2.0 - thumb_c / 2.0, y + height / 2.0 - thumb_c / 2.0);

                MainRect.StrokeThickness = base.StrokeThickness;
                MainRect.Stroke = base.BorderBrush;
                MainRect.Width = width;
                MainRect.Height = height;
                Canvas.SetLeft(MainRect, x);
                Canvas.SetTop(MainRect, y);

                //MainRect.Visibility = base.isHidden;
            }
        }
        /// <summary>
        /// 创建Thumb
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="thumb_left"></param>
        /// <param name="thumb_top"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Thumb NewThumb(double width, double height, double thumb_left, double thumb_top, Direction direction)
        {
            var thumb = new Thumb { Height = height, Width = width };
            double left = (double)MainRect.GetValue(Canvas.LeftProperty);
            double top = (double)MainRect.GetValue(Canvas.TopProperty);
            thumb.SetValue(Canvas.LeftProperty, left + thumb_left);
            thumb.SetValue(Canvas.TopProperty, top + thumb_top);
            FiguresCanvas.Children.Add(thumb);
            thumb.DragDelta += (s, e) =>
            {
                ResetLocation(direction, e.HorizontalChange, e.VerticalChange);
            };
            thumb.DragCompleted += (s, e) =>
            {
                double x = Math.Min(base.CurStart.X, base.CurEnd.X);
                double x2 = Math.Max(base.CurStart.X, base.CurEnd.X);
                double y = Math.Min(base.CurStart.Y, base.CurEnd.Y);
                double y2 = Math.Max(base.CurStart.Y, base.CurEnd.Y);
                base.CurStart = new Point(x, y);
                base.CurEnd = new Point(x2, y2);
                base.OriStart = MsiToCanvas(base.CurStart);
                base.OriEnd = MsiToCanvas(base.CurEnd);
            };
            return thumb;
        }
        /// <summary>
        /// 更新Thumb位置
        /// </summary>
        /// <param name="thumb"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void UpdateThumbVisual(Thumb thumb, double left, double top)
        {
            thumb.SetValue(Canvas.LeftProperty, left);
            thumb.SetValue(Canvas.TopProperty, top);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            base.IsFinish = true;
            //CreateMTextBlock();
            CreateThumb();
            base.MSI.MouseLeftButtonDown -= MouseDown;
            MainRect.MouseLeftButtonDown += Select_MouseDown;
            MainRect.MouseEnter += GotFocus;
            base.MSI.MouseMove -= MouseMove;
            base.MSI.MouseUp -= MouseUp;
            FiguresCanvas.MouseUp -= MouseUp;
            Application.Current.MainWindow.MouseLeave -= MouseUp;
            Application.Current.MainWindow.MouseUp -= MouseUp;
            base.FinishFunc(this, e);
        }
        private void GotFocus(object sender, EventArgs e) { }
        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!base.IsDrawing)
            {
                MainRect = new Rectangle();
                //MainRect.Name = base.ControlName;
                Point position = e.GetPosition(FiguresCanvas);
                MainRect.SetValue(Canvas.LeftProperty, position.X);
                MainRect.SetValue(Canvas.TopProperty, position.Y);
                MainRect.Width = 0.0;
                MainRect.Height = 0.0;
                MainRect.StrokeThickness = base.StrokeThickness;
                MainRect.Stroke = base.BorderBrush;
                base.OriStart = position;
                base.OriEnd = position;
                AnnoPoint = position;
                base.CurStart = CanvasToMsi(position);
                base.CurEnd = CanvasToMsi(position);
                base.FiguresCanvas.Children.Add(MainRect);
                base.MSI.MouseMove += MouseMove;
                Application.Current.MainWindow.MouseLeave += MouseUp;
                Application.Current.MainWindow.MouseUp += MouseUp;
                //base.AnnotationName = Setting.Rectangle + (base.ObjList.Count + 1);
                //base.AnnotationDescription = "";
                base.ObjList.Insert(0, this);
                //UpdateCB();
                base.IsDrawing = true;
            }
        }

        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(FiguresCanvas);
            Point originEnd = new Point(Math.Max(AnnoPoint.X, position.X), Math.Max(AnnoPoint.Y, position.Y));
            Point originStart = new Point(Math.Min(AnnoPoint.X, position.X), Math.Min(AnnoPoint.Y, position.Y));
            MainRect.SetValue(Canvas.LeftProperty, originStart.X);
            MainRect.SetValue(Canvas.TopProperty, originStart.Y);
            MainRect.Width = originEnd.X - originStart.X;
            MainRect.Height = originEnd.Y - originStart.Y;
            base.OriStart = originStart;
            base.OriEnd = originEnd;
            base.CurStart = CanvasToMsi(base.OriStart);
            base.CurEnd = CanvasToMsi(base.OriEnd);
            //base.TextBlock_info = CalcMeasureInfo();
            //base.AnnoControl.Tbk.Text = null;
            //base.AnnoControl.Tbk.Text = CalcMeasureInfo();
        }
        /// <summary>
        /// 选中矩形事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect)
            {
                this.IsActive(Visibility.Visible);
                var unSelected = base.ObjList.Where(x => x is AnnoRect arect && arect.MainRect != rect);
                foreach (var item in unSelected)
                {
                    item.IsActive(Visibility.Collapsed);
                }
                //if (item.ControlName == rectangle.Name)
                //{
                //    item.IsActive(Visibility.Visible);
                //    base.AnnoControl.CB.SelectedIndex = base.ObjList.IndexOf(this);
                //}
            }
        }
        /// <summary>
        /// 是否显示Thumbs
        /// </summary>
        /// <param name="vis"></param>
        internal override void IsActive(Visibility vis)
        {
            if (ThumbMove != null)
            {
                ThumbB.Visibility = vis;
                ThumbLB.Visibility = vis;
                ThumbL.Visibility = vis;
                ThumbLT.Visibility = vis;
                ThumbR.Visibility = vis;
                ThumbRB.Visibility = vis;
                ThumbRT.Visibility = vis;
                ThumbT.Visibility = vis;
                ThumbMove.Visibility = vis;
            }
        }
    }
}
