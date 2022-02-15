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
    /// PhysDistReceiptBarcode.xaml 的交互逻辑
    /// </summary>
    public partial class PhysDistReceiptBarcode : BaseUserControl
    {
        private PhysDistReceiptBarcodeViewModel physDistReceiptBarcodeViewModel = null;

        public PhysDistReceiptBarcode()
        {
            InitializeComponent();
            this.physDistReceiptBarcodeViewModel = new PhysDistReceiptBarcodeViewModel();
            this.DataContext = this.physDistReceiptBarcodeViewModel;
            this.Loaded += this.PhysDistReceiptBarcode_Loaded;
        }

        private void PhysDistReceiptBarcode_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbMailNo.Focus();
            Messenger.Default.Register<object>(this, EnumMessageKey.PhysDistReceiptBarcodeFocus, data =>
            {
                this.tbBarcode.Text = "";
                this.tbBarcode.Focus();
            });
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                this.tbBarcode.Focus();
            }
        }
    }
}
