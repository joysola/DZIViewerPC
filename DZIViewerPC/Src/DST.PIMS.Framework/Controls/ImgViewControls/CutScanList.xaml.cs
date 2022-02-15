using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// CutScanList.xaml 的交互逻辑
    /// </summary>
    public partial class CutScanList : UserControl
    {
        #region properties
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
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(CutScanList), new PropertyMetadata(default(IEnumerable)));
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
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(CutScanList), new PropertyMetadata(null));

        /// <summary>
        /// 复选项目
        /// </summary>
        public IList SelectedItemList
        {
            get => (IList)GetValue(SelectedItemListProperty);
            set => SetValue(SelectedItemListProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemListProperty =
            DependencyProperty.Register(nameof(SelectedItemList), typeof(IList), typeof(CutScanList), new PropertyMetadata(default(IList)));
        /// <summary>
        /// 展示的图片地址属性名
        /// </summary>
        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        // Using a DependencyProperty as the backing store for DisplayMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(CutScanList), new PropertyMetadata(null));
        /// <summary>
        /// 是否正在分屏
        /// </summary>
        public bool IsSpliting
        {
            get => (bool)GetValue(IsSplitingProperty);
            set => SetValue(IsSplitingProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsSpliting.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSplitingProperty =
            DependencyProperty.Register(nameof(IsSpliting), typeof(bool), typeof(CutScanList), new PropertyMetadata(false));
        #endregion properties

        #region events
        /// <summary>
        ///   分屏事件
        /// </summary>
        public static readonly RoutedEvent StartSplitScreenEvent =
            EventManager.RegisterRoutedEvent(nameof(StartSplitScreen), RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(CutScanList));

        /// <summary>
        ///    分屏事件
        /// </summary>
        public event EventHandler StartSplitScreen
        {
            add => AddHandler(StartSplitScreenEvent, value);
            remove => RemoveHandler(StartSplitScreenEvent, value);
        }
        /// <summary>
        ///   取消分屏事件
        /// </summary>
        public static readonly RoutedEvent CancelScreenEvent =
            EventManager.RegisterRoutedEvent(nameof(CancelScreen), RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(CutScanList));

        /// <summary>
        ///     取消分屏事件
        /// </summary>
        public event EventHandler CancelScreen
        {
            add => AddHandler(CancelScreenEvent, value);
            remove => RemoveHandler(CancelScreenEvent, value);
        }
        #endregion events
        public CutScanList()
        {
            InitializeComponent();
        }
        /// <summary>
        /// checkbox勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox box)
            {
                if (SelectedItemList?.Count == 2) // 只能2分屏
                {
                    box.IsChecked = false;
                    // 防止checkbox内部path对象，在ischecked为false不能回到初始状态
                    if (box.Template?.FindName("path", box) is Path path && Application.Current.TryFindResource("TextIconBrush") is SolidColorBrush brush)
                    {
                        path.Stroke = brush;
                    }
                }
                else
                {
                    SelectedItemList?.Add(box.DataContext);
                }
            }
        }
        /// <summary>
        /// checkbox取消勾选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CB_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox box)
            {
                //var xx = this.lbImgs.ItemContainerGenerator.ContainerFromItem(box.DataContext);
                SelectedItemList?.Remove(box.DataContext);
            }
        }
        /// <summary>
        /// 分屏浏览按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button splitBtn)
            {
                if (splitBtn.Visibility == Visibility.Visible)
                {
                    this.splitBtn.Visibility = Visibility.Collapsed;
                    this.CancelBtn.Visibility = Visibility.Visible;
                    this.OkBtn.Visibility = Visibility.Visible;
                    ShowCheckboxVis(Visibility.Visible);
                    IsSpliting = true;
                }
            }
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button cancelBtn)
            {
                if (cancelBtn.Visibility == Visibility.Visible)
                {
                    this.splitBtn.Visibility = Visibility.Visible;
                    this.CancelBtn.Visibility = Visibility.Collapsed;
                    this.OkBtn.Visibility = Visibility.Collapsed;
                    ShowCheckboxVis(Visibility.Collapsed);
                    IsSpliting = false;
                    SelectedItemList?.Clear(); // 取消分屏后，清空选中项
                    RaiseEvent(new RoutedEventArgs(CancelScreenEvent, this));
                }
            }
        }
        /// <summary>
        /// 确定分屏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItemList?.Count == 2)
            {
                RaiseEvent(new RoutedEventArgs(StartSplitScreenEvent, this));
            }
        }
        /// <summary>
        /// 是否显示checkbox
        /// </summary>
        /// <param name="visibility"></param>
        private void ShowCheckboxVis(Visibility visibility)
        {
            foreach (var item in ItemsSource)
            {
                if (this.lbImgs.ItemContainerGenerator.ContainerFromItem(item) is ListBoxItem listItem)
                {
                    var content = listItem.Template?.FindName("cb", listItem);
                    //if (content is FrameworkElement fe && fe.FindName("cb") is CheckBox cb)
                    if (content is CheckBox cb)
                    {
                        cb.IsChecked = false;
                        cb.Visibility = visibility;
                    }
                }
            }
        }
    }
}
