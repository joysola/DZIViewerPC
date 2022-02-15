using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
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
    /// CustomConfiguration.xaml 的交互逻辑
    /// </summary>
    public partial class CustomConfiguration : BaseUserControl
    {
        private CustomConfigurationViewModel customConfigurationViewModel = null;

        public CustomConfiguration()
        {
            InitializeComponent();
            this.customConfigurationViewModel = new CustomConfigurationViewModel();
            this.DataContext = this.customConfigurationViewModel;
        }

        private void Btn_Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
