using DST.Controls.Base;
using DST.PIMS.Framework.Model;
using System;
using System.Collections;
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
    /// SysManageNav.xaml 的交互逻辑
    /// </summary>
    public partial class SysManageNav : BaseUserControl
    {

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
         "ItemsSource", typeof(IEnumerable<TreeNode>), typeof(SysManageNav), new PropertyMetadata(default(IEnumerable<TreeNode>)));
        /// <summary>
        /// 数据源树
        /// </summary>
        public IEnumerable<TreeNode> ItemsSource
        {
            get => (IEnumerable<TreeNode>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        "SelectedItem", typeof(TreeNode), typeof(SysManageNav), new PropertyMetadata(null));
        /// <summary>
        /// 选中项
        /// </summary>
        public TreeNode SelectedItem
        {
            get => (TreeNode)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public SysManageNav()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 左击菜单后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem treeViewItem)
            {
                SelectedItem = (TreeNode)treeViewItem.DataContext;
            }
        }
    }
}
