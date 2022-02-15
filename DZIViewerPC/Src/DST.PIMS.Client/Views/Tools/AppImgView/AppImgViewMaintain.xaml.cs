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
    /// AppImgViewMaintain.xaml 的交互逻辑
    /// </summary>
    public partial class AppImgViewMaintain : BaseUserControl
    {
        private AppImgViewMaintainViewModel appImgViewMaintainViewModel = null;

        public AppImgViewMaintain()
        {
            InitializeComponent();
            this.appImgViewMaintainViewModel = new AppImgViewMaintainViewModel();
            this.DataContext = this.appImgViewMaintainViewModel;
            this.Loaded += (s, e) => this.labTxt.Focus(); // 焦点集中于实验室编号文本框
            // 保存后，焦点集中于实验室编号文本框
            Messenger.Default.Register<object>(this, EnumMessageKey.AppFrmMaintainSaveFocus, data => this.labTxt.Focus());
        }
    }
}
