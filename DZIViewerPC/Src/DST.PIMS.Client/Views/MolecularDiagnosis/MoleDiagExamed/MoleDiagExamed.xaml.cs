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
    /// MoleDiagExamed.xaml 的交互逻辑
    /// </summary>
    public partial class MoleDiagExamed : BaseUserControl
    {
        private MoleDiagExamedViewModel moleDiagExamedViewModel = null;

        public MoleDiagExamed()
        {
            InitializeComponent();
            this.moleDiagExamedViewModel = new MoleDiagExamedViewModel();
            this.DataContext = this.moleDiagExamedViewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
