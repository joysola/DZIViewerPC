
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

namespace DST.PIMS.Framework.Controls
{
    /// <summary>
    /// TreeCheck.xaml 的交互逻辑
    /// </summary>
    public partial class TreeCheck : UserControl
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
                  nameof(ItemsSource), typeof(IEnumerable<TreeNode>), typeof(TreeCheck), new PropertyMetadata(default(IEnumerable<TreeNode>)));
        /// <summary>
        /// 数据源限定IEnumerable<TreeNode>
        /// </summary>
        public IEnumerable<TreeNode> ItemsSource
        {
            get => (IEnumerable<TreeNode>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public TreeCheck()
        {
            InitializeComponent();
        }
    }
}
