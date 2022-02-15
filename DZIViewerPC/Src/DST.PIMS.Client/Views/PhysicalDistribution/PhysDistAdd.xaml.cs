using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// PhysDistAdd.xaml 的交互逻辑
    /// </summary>
    public partial class PhysDistAdd : BaseUserControl
    {
        private PhysDistAddViewModel physDistAddViewModel = null;

        public PhysDistAdd()
        {
            InitializeComponent();
            this.physDistAddViewModel = new PhysDistAddViewModel();
            this.DataContext = this.physDistAddViewModel;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.physDistAddViewModel.ShowHostitalDict();
            this.dgExpressPathology.Focus();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((sender as TextBox).Text.Trim().Length < 2)
            {
                Regex re = new Regex("[^0-9]+");
                e.Handled = re.IsMatch(e.Text);
            }
            else
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewTextInput_1(object sender, TextCompositionEventArgs e)
        {
            if ((sender as TextBox).Text.Trim().Length < 12)
            {
                Regex re = new Regex("[^0-9]+");
                e.Handled = re.IsMatch(e.Text);
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
