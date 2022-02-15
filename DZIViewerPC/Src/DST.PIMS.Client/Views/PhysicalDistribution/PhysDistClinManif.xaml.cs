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
    /// PhysDistClinManif.xaml 的交互逻辑
    /// </summary>
    public partial class PhysDistClinManif : BaseUserControl
    {
        private PhysDistClinManifViewModel physDistClinManifViewModel = null;

        public PhysDistClinManif()
        {
            InitializeComponent();
            this.physDistClinManifViewModel = new PhysDistClinManifViewModel();
            this.DataContext = this.physDistClinManifViewModel;
            this.tbName.Focus();
            this.RegisterCommand();
        }

        private void RegisterCommand()
        {
            Messenger.Default.Register<bool>(this, EnumMessageKey.ResetFocus, data =>
            {
                this.tbCliDia.Focus();
                if (!string.IsNullOrEmpty(this.tbCliDia.Text))
                {
                    this.tbCliDia.SelectionStart = this.tbCliDia.Text.Length;
                }
            });
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
