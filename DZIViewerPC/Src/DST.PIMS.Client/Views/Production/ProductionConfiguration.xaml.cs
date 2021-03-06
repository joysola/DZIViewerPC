using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// ProductionConfiguration.xaml 的交互逻辑
    /// </summary>
    public partial class ProductionConfiguration : BaseUserControl
    {
        private ProductionConfigurationViewModel productionConfigurationViewModel = null;

        public ProductionConfiguration()
        {
            InitializeComponent();
            this.productionConfigurationViewModel = new ProductionConfigurationViewModel();
            this.DataContext = this.productionConfigurationViewModel;
        }

        private void Btn_SelectPath(object sender, RoutedEventArgs e)
        {
            var dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            DialogResult result = dilog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK || result == System.Windows.Forms.DialogResult.Yes)
            {
                this.productionConfigurationViewModel.ProductionScanDir = dilog.SelectedPath;
            }
        }

        private void Btn_SelectTemple(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择模板文件";
            dialog.Filter = "模板文件|*." + "EBT;*.EST";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                this.productionConfigurationViewModel.ProductionTemplateFile = dialog.FileName;
            }
        }
    }
}
