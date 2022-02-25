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
    /// RatioCtl.xaml 的交互逻辑
    /// </summary>
    public partial class RatioCtl : UserControl
    {
        public static readonly RoutedEvent CurScaleChangeEvent = EventManager.RegisterRoutedEvent(nameof(CurScaleChange), RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(RatioCtl));

        public event RoutedEventHandler CurScaleChange
        {
            add { AddHandler(CurScaleChangeEvent, value); }
            remove { RemoveHandler(CurScaleChangeEvent, value); }
        }
        /// <summary>
        /// 比例尺
        /// </summary>
        public double Curscale
        {
            get => (double)GetValue(CurscaleProperty);
            set => SetValue(CurscaleProperty, value);
        }

        // Using a DependencyProperty as the backing store for CellWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurscaleProperty =
            DependencyProperty.Register(nameof(Curscale), typeof(double), typeof(RatioCtl), new PropertyMetadata(0.0));

        public RatioCtl()
        {
            InitializeComponent();
        }


        private void BtnRatio_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is string ratio &&double.TryParse(ratio, out double scale))
            {
                //Curscale = scale;
                RaiseEvent(new RoutedEventArgs(CurScaleChangeEvent, scale));
            }
        }
    }
}
