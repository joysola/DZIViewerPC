using DST.Controls.Base;
using System;
using System.Collections;
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

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ImgVisionScrollViewer.xaml 的交互逻辑
    /// </summary>
    public partial class ImgVisionScrollViewer : BaseUserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
         "ItemsSource", typeof(IEnumerable), typeof(ImgVisionScrollViewer), new PropertyMetadata(default(IEnumerable)));

        public IEnumerable ItemsSource
        {
            get => (string)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public ImgVisionScrollViewer()
        {
            InitializeComponent();
        }
    }
}
