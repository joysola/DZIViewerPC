using DST.Controls.Base;
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

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ImgVision.xaml 的交互逻辑
    /// </summary>
    public partial class ImgVision : BaseUserControl
    {

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
          "Url", typeof(string), typeof(ImgVision), new PropertyMetadata(null));

        public static readonly DependencyProperty DescProperty = DependencyProperty.Register(
         "Desc", typeof(string), typeof(ImgVision), new PropertyMetadata(string.Empty));
        /// <summary>
        /// 图片地址
        /// </summary>
        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }
        /// <summary>
        /// 采图描述
        /// </summary>
        public string Desc
        {
            get => (string)GetValue(DescProperty);
            set => SetValue(DescProperty, value);
        }
        public ImgVision()
        {
            InitializeComponent();
        }
    }
}
