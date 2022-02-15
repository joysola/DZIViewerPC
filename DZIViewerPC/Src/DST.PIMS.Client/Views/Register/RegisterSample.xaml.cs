using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Client.ViewModel.Test;
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
    /// SampleRegisterPart.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterSample : BaseUserControl
    {
        //public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        //   "Title", typeof(string), typeof(RegisterSample), new PropertyMetadata("常规病理登记"));
        //public string Title
        //{
        //    get => (string)GetValue(TitleProperty);
        //    set => SetValue(TitleProperty, value);
        //}

        public RegisterSample()
        {
            InitializeComponent();
        }
    }
}
