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
    /// ToolBar.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBar : BaseUserControl
    {
        private ToolBarViewModel toolBarViewModel = null;

        public ToolBar()
        {
            InitializeComponent();
            this.toolBarViewModel = new ToolBarViewModel();
            this.DataContext = this.toolBarViewModel;
        }


        private void LbChangeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.popupChange.IsOpen = false;
            this.toolBarViewModel.SwitchWorkstation(this.lbChangeType.SelectedItem);

        }

        private void BtnSwitchWorkstation_Click(object sender, RoutedEventArgs e)
        {
            this.popupChange.IsOpen = true;
        }

        private void LbMenuControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
