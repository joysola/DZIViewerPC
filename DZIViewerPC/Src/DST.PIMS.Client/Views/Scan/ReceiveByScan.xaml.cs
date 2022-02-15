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
    /// PhysDistPreTreatBarcode.xaml 的交互逻辑
    /// </summary>
    public partial class ReceiveByScan : BaseUserControl
    {
        private ReceiveByScanViewModel receiveByScanViewModel = null;

        public ReceiveByScan()
        {
            InitializeComponent();
            this.receiveByScanViewModel = new ReceiveByScanViewModel();
            this.DataContext = this.receiveByScanViewModel;
            this.tbBarcode.Focus();
            this.RegisterCommand();
        }

        private void RegisterCommand()
        {
            Messenger.Default.Register<bool>(this, EnumMessageKey.ResetFocus, data =>
            {
                if (!data)
                {
                    this.dgInspection.Focus();
                }
                else
                {
                    this.tbBarcode.Focus();
                }
            });
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
