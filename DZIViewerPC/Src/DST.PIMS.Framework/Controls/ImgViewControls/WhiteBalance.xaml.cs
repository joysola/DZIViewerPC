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
    /// WhiteBalance.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteBalance : UserControl
    {
        private static readonly double red = 0.0;
        private static readonly double green = 0.0;
        private static readonly double blue = 0.0;
        private static readonly double brightness = 0.0;
        private static readonly double contrast = 1.0;
        private static readonly double gamma = 1.2;
        /// <summary>
        /// Red
        /// </summary>
        public double Red
        {
            get => (double)GetValue(RedProperty);
            set => SetValue(RedProperty, value);
        }

        // Using a DependencyProperty as the backing store for Red.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RedProperty =
            DependencyProperty.Register(nameof(Red), typeof(double), typeof(WhiteBalance), new PropertyMetadata(red));
        /// <summary>
        /// Green
        /// </summary>
        public double Green
        {
            get => (double)GetValue(GreenProperty);
            set => SetValue(GreenProperty, value);
        }

        // Using a DependencyProperty as the backing store for Green.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GreenProperty =
            DependencyProperty.Register(nameof(Green), typeof(double), typeof(WhiteBalance), new PropertyMetadata(green));
        /// <summary>
        /// Blue
        /// </summary>
        public double Blue
        {
            get => (double)GetValue(BlueProperty);
            set => SetValue(BlueProperty, value);
        }

        // Using a DependencyProperty as the backing store for Blue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BlueProperty =
            DependencyProperty.Register(nameof(Blue), typeof(double), typeof(WhiteBalance), new PropertyMetadata(blue));



        /// <summary>
        /// Brightness
        /// </summary>
        public double Brightness
        {
            get => (double)GetValue(BrightnessProperty);
            set => SetValue(BrightnessProperty, value);
        }

        // Using a DependencyProperty as the backing store for Brightness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrightnessProperty =
            DependencyProperty.Register(nameof(Brightness), typeof(double), typeof(WhiteBalance), new PropertyMetadata(brightness));


        /// <summary>
        /// Contrast
        /// </summary>
        public double Contrast
        {
            get => (double)GetValue(ContrastProperty);
            set => SetValue(ContrastProperty, value);
        }

        // Using a DependencyProperty as the backing store for Contrast.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContrastProperty =
            DependencyProperty.Register(nameof(Contrast), typeof(double), typeof(WhiteBalance), new PropertyMetadata(contrast));


        /// <summary>
        /// Gamma
        /// </summary>
        public double Gamma
        {
            get => (double)GetValue(GammaProperty);
            set => SetValue(GammaProperty, value);
        }

        // Using a DependencyProperty as the backing store for Gamma.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GammaProperty =
            DependencyProperty.Register(nameof(Gamma), typeof(double), typeof(WhiteBalance), new PropertyMetadata(gamma));



        /// <summary>
        /// IsOpen
        /// </summary>
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set => SetValue(IsOpenProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(WhiteBalance), new PropertyMetadata(false));

        public WhiteBalance()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Brightness = brightness;
            Contrast = contrast;
            Gamma = gamma;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }
    }
}
