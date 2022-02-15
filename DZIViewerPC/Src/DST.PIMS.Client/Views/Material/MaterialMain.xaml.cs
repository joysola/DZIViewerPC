using DST.Controls.Base;
using DST.PIMS.Client.ViewModel;

using DST.PIMS.Client.ViewModel.Test;
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
    /// MaterialMain.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialMain : BaseUserControl
    {
        private MaterialMainViewModel materialMainViewModel = null;
        public MaterialMain()
        {
            InitializeComponent();
            this.materialMainViewModel = new MaterialMainViewModel();
            this.DataContext = this.materialMainViewModel;
        }

    }
}
