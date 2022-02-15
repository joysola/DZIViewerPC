using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : BaseUserControl
    {
        public About()
        {
            InitializeComponent();
            this.DataContext = new AboutViewModel();
            this.Loaded += this.About_Loaded;
        }

        private void About_Loaded(object sender, RoutedEventArgs e)
        {
            // 版本号以Client程序集的版本号为主
            this.tbVersion.Text = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        }
    }
}
