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
    /// Rotater.xaml 的交互逻辑
    /// </summary>
    public partial class Rotater : UserControl
    {
        /// <summary>
        /// Slider的value
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(Rotater), new PropertyMetadata(0.0));

        public Rotater()
        {
            InitializeComponent();
        }

        private void Start_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => Value = 0;

        private void End_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) => Value = 360;
    }
}
