using DST.Controls.Base;
using DST.PIMS.Framework.Model.Test;
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
    /// CytoDiagPathDiag.xaml 的交互逻辑
    /// </summary>
    public partial class CytoDiagPathDiag : BaseUserControl
    {
        public CytoDiagPathDiag()
        {
            InitializeComponent();
            this.RegisterMessenger();
            // 加载完成后模拟点击展开，以触发Expanded事件
            this.Loaded += (s, e) =>
            {
                this.expanderScope.IsExpanded = false;
                this.expanderScope.IsExpanded = true;
            };
        }

        private void RegisterMessenger()
        {
        }
        /// <summary>
        /// 用以强制更新ScrollView的高度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void expanderScope_Expanded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.InvokeAsync(() =>
            {
                var bindExp = this.scrollViewerScope.GetBindingExpression(/*HandyControl.Controls.ScrollViewer.*/HeightProperty); // 获取绑定表达式
                bindExp.UpdateTarget(); // 更新所绑定的属性
            });
        }
    }
}
