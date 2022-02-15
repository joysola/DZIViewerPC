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
    /// AnnoList.xaml 的交互逻辑
    /// </summary>
    public partial class AnnoList : UserControl
    {
        /// <summary>
        /// 数据源
        /// </summary>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(AnnoList), new PropertyMetadata(default(IEnumerable)));

        /// <summary>
        /// 标记框编辑命令
        /// </summary>
        public ICommand EditAnnoCommand
        {
            get => (ICommand)GetValue(EditAnnoTargetCommandProperty);
            set => SetValue(EditAnnoTargetCommandProperty, value);
        }
        // Using a DependencyProperty as the backing store for EditAnnoCommand.  This enables animation, styling, binding, etc...
        public static DependencyProperty EditAnnoTargetCommandProperty = 
            DependencyProperty.Register(nameof(EditAnnoCommand), typeof(ICommand), typeof(AnnoList));
        
        /// <summary>
        /// 选中项
        /// </summary>
        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(AnnoList), new PropertyMetadata(null));
        /// <summary>
        /// 来自imgviewer的数据
        /// </summary>
        public AnnoInfo AnnoInfos
        {
            get => (AnnoInfo)GetValue(AnnoInfosProperty);
            set => SetValue(AnnoInfosProperty, value);
        }

        // Using a DependencyProperty as the backing store for AnnoInfos.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnnoInfosProperty =
            DependencyProperty.Register(nameof(AnnoInfos), typeof(AnnoInfo), typeof(AnnoList), new PropertyMetadata(new AnnoInfo()));

        public AnnoList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Delete(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is AnnoBase deleteItem)
            {
                deleteItem.DeleteItem();
                AnnoInfos?.AnnoList?.Remove(deleteItem);
            }
        }
    }
}
