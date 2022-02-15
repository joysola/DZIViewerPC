using DST.Common.Helper;
using MVVMExtension;
using Nico.DeepZoom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DST.PIMS.Framework.Controls
{
    [NotifyAspect]
    public class AnnoBase : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _thumbUrl;
        private string _anno_Name;
        private string _description;
        public MultiScaleImage MSI { get; set; }

        public Canvas FiguresCanvas { get; set; }

        public double Zoom { get; set; }
        /// <summary>
        /// 倍率 40、20、10、8、5、4、3、2、1
        /// </summary>
        public int SlideZoom { get; set; }

        public double Calibration { get; set; }

        public Visibility IsHidden { get; set; }

        //public Brush BorderBrush { get; set; } = new SolidColorBrush(Colors.Red);

        public Brush FontColor { get; set; }



        /// <summary>
        /// 缩略图url地址
        /// </summary>
        public string ThumbUrl
        {
            get => _thumbUrl;
            set
            {
                _thumbUrl = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ThumbUrl))); // 触发NotifyPropertyChanged
            }
        }
        //public double StrokeThickness { get; set; } = 5;

        /// <summary>
        /// 标记框线条宽度
        /// </summary>
        public double StrokeThickness
        {
            get => (double)GetValue(StrokeThicknessProperty);
            set => SetValue(StrokeThicknessProperty, value);
        }
        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(AnnoBase), new PropertyMetadata(5.0));

        public Visibility ThumbVisibility
        {
            get => (Visibility)GetValue(ThumbVisibilityProperty);
            set => SetValue(ThumbVisibilityProperty, value);
        }

        // Using a DependencyProperty as the backing store for ThumbVisibility.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbVisibilityProperty =
            DependencyProperty.Register(nameof(ThumbVisibility), typeof(Visibility), typeof(AnnoBase), new PropertyMetadata(Visibility.Collapsed));
        /// <summary>
        /// 控件具有的Thumbs
        /// </summary>
        protected List<CustomThumb> CustomThumbs { get; set; } = new List<CustomThumb>();

        /// <summary>
        /// 左上起点
        /// </summary>
        public Point CurStart { get; set; }
        /// <summary>
        /// 右下终点
        /// </summary>
        public Point CurEnd { get; set; }

        public Point OriStart { get; set; }

        public Point OriEnd { get; set; }

        public Point CurCenter => new Point((CurStart.X + CurEnd.X) / 2, (CurStart.Y + CurEnd.Y) / 2);

        public bool IsFinish { get; set; }

        #region 数据保存相关字段

        /// <summary>
        /// 标记备注
        /// </summary>
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description))); // 触发NotifyPropertyChanged
            }
        }
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标记名称
        /// </summary>
        public string Anno_Name
        {
            get => _anno_Name;
            set
            {
                _anno_Name = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Anno_Name)));
            }
        }
        /// <summary>
        /// 标记缩略图
        /// </summary>
        public byte[] ThumbImg { get; set; }
        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCanceled { get; set; }
        #endregion 数据保存相关字段

        /// <summary>
        /// 是否正在绘制
        /// </summary>
        public bool IsDrawing { get; set; }
        /// <summary>
        /// 是否被选中
        /// </summary>
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    this.ThumbVisibility = Visibility.Visible;
                    ObjList?.Where(anno => anno != this)?.ToList()?.ForEach(anno => anno.ThumbVisibility = Visibility.Collapsed);
                }
                else
                {
                    this.ThumbVisibility = Visibility.Collapsed;
                }
            }
        }

        public List<AnnoBase> ObjList { get; set; }
        //public PointCollection PointCollection { get; set; }

        public event MouseEventHandler FinishEvent;


        public AnnoBase(Canvas canvas, MultiScaleImage msi, List<AnnoBase> objectlist, int slideZoom, double calibration)
        {
            this.FiguresCanvas = canvas;
            this.MSI = msi;
            this.ObjList = objectlist;
            this.SlideZoom = slideZoom;
            if (msi.ZoomableCanvas != null)
            {
                this.Zoom = msi.ZoomableCanvas.Scale * slideZoom;
            }
            this.Calibration = calibration;

            InitAnno();

            this.Loaded += (s, e) =>
            {
                CustomThumbs = this.FindChildren<CustomThumb>();
            };
        }
        /// <summary>
        /// 更新标记显示
        /// </summary>
        public virtual void UpdateVisual() { }
        internal virtual void IsActive(Visibility vis) { }

        protected virtual void ResetLocation(Direction direction, double offsetX, double offsetY)
        {
            double num = MSI.ZoomableCanvas.Scale * SlideZoom;
            Point currentStart = CurStart;
            Point currentEnd = CurEnd;
            switch (direction)
            {
                case Direction.LeftTop:
                    CurStart = new Point(CurStart.X + offsetX / num, CurStart.Y + offsetY / num);
                    break;
                case Direction.Top:
                    CurStart = new Point(CurStart.X, CurStart.Y + offsetY / num);
                    break;
                case Direction.Left:
                    CurStart = new Point(CurStart.X + offsetX / num, CurStart.Y);
                    break;
                case Direction.RightTop:
                    CurStart = new Point(CurStart.X, CurStart.Y + offsetY / num);
                    CurEnd = new Point(CurEnd.X + offsetX / num, CurEnd.Y);
                    break;
                case Direction.Right:
                    CurEnd = new Point(CurEnd.X + offsetX / num, CurEnd.Y);
                    break;
                case Direction.RightBottom:
                    CurEnd = new Point(CurEnd.X + offsetX / num, CurEnd.Y + offsetY / num);
                    break;
                case Direction.Bottom:
                    CurEnd = new Point(CurEnd.X, CurEnd.Y + offsetY / num);
                    break;
                case Direction.LeftBottom:
                    CurStart = new Point(CurStart.X + offsetX / num, CurStart.Y);
                    CurEnd = new Point(CurEnd.X, CurEnd.Y + offsetY / num);
                    break;
                case Direction.Center:
                    CurStart = new Point(CurStart.X + offsetX / num, CurStart.Y + offsetY / num);
                    CurEnd = new Point(CurEnd.X + offsetX / num, CurEnd.Y + offsetY / num);
                    break;
            }
            //if (AnnotationType == AnnotationType.Line || AnnotationType == AnnotationType.Arrow)
            //{
            //    UpdateVisual();
            //    UpadteTextBlock();
            //    return;
            //}
            //if (AnnotationType == AnnotationType.Remark)
            //{
            //    UpdateVisual();
            //    return;
            //}
            bool flag = false;
            double num2 = CurStart.X - CurEnd.X;
            double num3 = currentStart.X - currentEnd.X;
            double num4 = CurStart.Y - CurEnd.Y;
            double num5 = currentStart.Y - currentEnd.Y;
            if (num2 * num3 > 0.0 && num4 * num5 > 0.0 && num2 * num4 > 50.0 / num)
            {
                flag = false;
            }
            else
            {
                flag = true;
                CurStart = currentStart;
                CurEnd = currentEnd;
            }
            if (!flag)
            {
                UpdateVisual();
                //UpadteTextBlock();
            }
        }
        /// <summary>
        /// 标记控件Thumb方法注册
        /// </summary>
        public virtual void RegisterThumbEvent() { }
        /// <summary>
        /// 标记控件Thumb方法解除注册
        /// </summary>
        public virtual void UnRegisterThumbEvent() { }
        public virtual void FinishFunc(object sender, MouseEventArgs e)
        {
            if (this.FinishEvent != null)
            {
                //Setting.isAnnoChange = true;
                this.FinishEvent(sender, e);
            }
        }
        /// <summary>
        /// 初始化标记
        /// </summary>
        protected virtual void InitAnno() { }
        public virtual void DeleteItem() { }

        /// <summary>
        /// 用户自定义控件重写命中方法
        /// </summary>
        /// <param name="hitTestParameters"></param>
        /// <returns></returns>
        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            // hitTestParameters.HitPoint是该控件中的坐标，需要转换到嵌入的Canvas的坐标
            var point = TranslatePoint(hitTestParameters.HitPoint, this.FiguresCanvas);
            return new PointHitTestResult(this, point);
        }

    }
}
