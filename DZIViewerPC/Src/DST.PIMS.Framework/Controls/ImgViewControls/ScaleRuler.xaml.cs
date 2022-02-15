using DST.PIMS.Framework.ExtendContext;
using System;
using System.Collections.Generic;
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
    /// ScaleRuler.xaml 的交互逻辑
    /// </summary>
    public partial class ScaleRuler : UserControl
    {
        private int MinRuleV = 200;
        /// <summary>
        /// 当前缩放参数
        /// </summary>
        public double Curscale
        {
            get => (double)GetValue(CurscaleProperty);
            set => SetValue(CurscaleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Curscale.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurscaleProperty =
            DependencyProperty.Register(nameof(Curscale), typeof(double), typeof(ScaleRuler), new PropertyMetadata(0.0, (d, p) =>
            {
                if (d is ScaleRuler scaleRuler && p.NewValue is double curscale)
                {
                    // 表示：1 / msi.ZoomableCanvas.Scale
                    var num = MSIContext.SlideZoom * 40 / (curscale * MSIContext.ImgScrnParam);
                    // num - num % 2.0 ：将num 变成偶数
                    double num2 = num < 2.0 ? 1.0 : num - num % 2.0;
                    //double num22 = (num <= 1.0) ? 1.0 : ((!(num > 1.0) || !(num < 2.0)) ? (num - num % 2.0) : 1.0);
                    double UM = 100.0 * num2; // 标尺代表的像素（100表示假设的控件宽度）
                    if (UM < 1000.0)
                    {
                        if (UM % 5.0 != 0.0)
                        {
                            UM += UM - UM % 5.0;
                        }
                    }
                    else
                    {
                        if (UM % 5000.0 != 0.0)
                        {
                            UM = UM >= 5000.0 ? UM + (UM / 5000.0 - UM % 5000.0) : 5000.0; // 减去不能整除5000的余数部分，为什么要加整除5000的商不理解
                        }
                    }
                    // UM / num 是控件宽度
                    double XS = UM / (num * MSIContext.Calibration);
                    var rulerInfo = scaleRuler.CheckLimtRule2(XS, UM); // 获取标尺尺寸信息
                    scaleRuler.UpRuleLayout2(rulerInfo.fcs, rulerInfo.fis); // 更新标尺信息
                    //scaleRuler.CheckLimtRule(ref XS, ref UM);
                    //scaleRuler.UpRuleLayout(rulerInfo.fcs, rulerInfo.fis);
                }
            }));
        /// <summary>
        /// 标尺控件等分刻度
        /// </summary>
        public double RulerWidth
        {
            get => (double)GetValue(RulerWidthProperty);
            set => SetValue(RulerWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for RulerWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RulerWidthProperty =
            DependencyProperty.Register(nameof(RulerWidth), typeof(double), typeof(ScaleRuler), new PropertyMetadata(0.0));
        /// <summary>
        /// 标尺实际长度
        /// </summary>
        public double CellWidth
        {
            get => (double)GetValue(CellWidthProperty);
            set => SetValue(CellWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for CellWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register(nameof(CellWidth), typeof(double), typeof(ScaleRuler), new PropertyMetadata(0.0));

        /// <summary>
        /// 获取合适的刻度尺信息
        /// </summary>
        /// <param name="ctlSize"></param>
        /// <param name="imgSize"></param>
        /// <returns></returns>
        private (double fcs, double fis) CheckLimtRule2(double ctlSize, double imgSize)
        {
            if (ctlSize >= (MinRuleV + MinRuleV / 2))
            {
                ctlSize /= 2.0;
                imgSize /= 2.0;
                if (ctlSize >= (MinRuleV + MinRuleV / 2))
                {
                    var result = CheckLimtRule2(ctlSize, imgSize);
                    ctlSize = result.fcs;
                    imgSize = result.fis;
                }
            }
            if (ctlSize < 150.0)
            {
                ctlSize *= 2.0;
                imgSize *= 2.0;
            }
            return (ctlSize, imgSize);
        }
        /// <summary>
        /// 更新刻度尺控件信息
        /// </summary>
        /// <param name="ctlSize"></param>
        /// <param name="imgSize"></param>
        private void UpRuleLayout2(double ctlSize, double imgSize)
        {
            double num; // 实际尺寸距离
            string empty; // 刻度单位
            if (imgSize >= 1000.0) // 判断单位
            {
                num = imgSize / 5000.0;
                empty = "mm";
            }
            else
            {
                num = imgSize / 5.0;
                empty = "μm";
            }
            CellWidth = num;
            RulerWidth = ctlSize / 5.0; // 刻度间隔
            unit.Content = empty;
        }

        public ScaleRuler()
        {
            InitializeComponent();
        }

        private void RuleThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {

        }
    }
}
