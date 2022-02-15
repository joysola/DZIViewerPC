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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DST.PIMS.Client.Views
{
    /// <summary>
    /// PhysDistSupplementBarcode.xaml 的交互逻辑
    /// </summary>
    public partial class PhysDistSupplementBarcode : BaseUserControl
    {
        private PhysDistSupplementBarcodeViewModel physDistSupplementBarcodeViewModel = null;

        public PhysDistSupplementBarcode()
        {
            InitializeComponent();
            this.physDistSupplementBarcodeViewModel = new PhysDistSupplementBarcodeViewModel();
            this.DataContext = this.physDistSupplementBarcodeViewModel;
            this.tbBarcode.Focus();
        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && !string.IsNullOrEmpty(this.tbBarcode.Text.Trim()))
            {
                this.physDistSupplementBarcodeViewModel.PrintBarcodeCommand.Execute(this.tbBarcode.Text.Trim());
                this.tbBarcode.Text = "";
            }
        }
    }
}
