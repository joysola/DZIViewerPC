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
    /// MoleDiagBarcode.xaml 的交互逻辑
    /// </summary>
    public partial class MoleDiagBarcode : BaseUserControl
    {
        private MoleDiagBarcodeViewModel moleDiagBarcodeViewModel = null;

        public MoleDiagBarcode()
        {
            InitializeComponent();
            this.RegisterCommand();
            this.moleDiagBarcodeViewModel = new MoleDiagBarcodeViewModel();
            this.DataContext = this.moleDiagBarcodeViewModel;
            this.Loaded += (sender, e) =>
            {
                this.codetxt.Focus();
                this.moleDiagBarcodeViewModel.Count2 = 0; // 防止badge不更新
            };
        }

        private void RegisterCommand()
        {
            Messenger.Default.Register<bool>(this, EnumMessageKey.CloseMoleDiagBarcodeConfig, data =>
            {
                this.DrawerRight.IsOpen = false;
            });
        }

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {
            this.DrawerRight.IsOpen = true;
        }
    }
}
