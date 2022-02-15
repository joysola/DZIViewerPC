using DST.Common.Extensions;
using DST.Controls.Base;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
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
    /// PatInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AppFrm : BaseUserControl
    {
        public AppFrm()
        {
            InitializeComponent();
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
