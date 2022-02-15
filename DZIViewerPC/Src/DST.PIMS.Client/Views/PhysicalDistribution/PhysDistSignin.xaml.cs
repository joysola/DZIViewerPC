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
    /// PhysDistSignin.xaml 的交互逻辑
    /// </summary>
    public partial class PhysDistSignin : BaseUserControl
    {
        private PhysDistSigninViewModel physDistSigninViewModel = null;

        public PhysDistSignin()
        {
            InitializeComponent();
            this.physDistSigninViewModel = new PhysDistSigninViewModel();
            this.DataContext = this.physDistSigninViewModel;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
