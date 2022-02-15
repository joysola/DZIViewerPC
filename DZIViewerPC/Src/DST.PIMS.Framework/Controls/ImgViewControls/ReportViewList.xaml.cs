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
    /// ReportViewList.xaml 的交互逻辑
    /// </summary>
    public partial class ReportViewList : UserControl
    {
        #region properties
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ReportViewList), new PropertyMetadata(default(IEnumerable)));

        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ReportViewList), new PropertyMetadata(default(object)));

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        // Using a DependencyProperty as the backing store for DisplayMemberPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(ReportViewList), new PropertyMetadata(null));
        #endregion properties

        #region events
        /// <summary>
        ///    定位事件
        /// </summary>
        public static readonly RoutedEvent LocateImgEvent =
            EventManager.RegisterRoutedEvent(nameof(LocateImg), RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(ReportViewList));
        /// <summary>
        ///     定位事件
        /// </summary>
        public event EventHandler LocateImg
        {
            add => AddHandler(LocateImgEvent, value);
            remove => RemoveHandler(LocateImgEvent, value);
        }

        /// <summary>
        ///    下载事件
        /// </summary>
        public static readonly RoutedEvent DownloadImgEvent =
            EventManager.RegisterRoutedEvent(nameof(DownloadImg), RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(ReportViewList));
        /// <summary>
        ///     下载事件
        /// </summary>
        public event EventHandler DownloadImg
        {
            add => AddHandler(DownloadImgEvent, value);
            remove => RemoveHandler(DownloadImgEvent, value);
        }

        /// <summary>
        ///    删除事件
        /// </summary>
        public static readonly RoutedEvent RemoveImgEvent =
            EventManager.RegisterRoutedEvent(nameof(RemoveImg), RoutingStrategy.Bubble,
                typeof(EventHandler), typeof(ReportViewList));
        /// <summary>
        ///     删除事件
        /// </summary>
        public event EventHandler RemoveImg
        {
            add => AddHandler(RemoveImgEvent, value);
            remove => RemoveHandler(RemoveImgEvent, value);
        }

        #endregion events

        public ReportViewList()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocateImg_Click(object sender, RoutedEventArgs e) => RaiseImgBtnEvent(LocateImgEvent, e);
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadImg_Click(object sender, RoutedEventArgs e) => RaiseImgBtnEvent(DownloadImgEvent, e);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveImg_Click(object sender, RoutedEventArgs e) => RaiseImgBtnEvent(RemoveImgEvent, e);

        /// <summary>
        /// 图片按钮触发事件方法
        /// </summary>
        /// <param name="routedEvent"></param>
        /// <param name="e"></param>
        private void RaiseImgBtnEvent(RoutedEvent routedEvent, RoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement button)
            {
                object clickedData = button.DataContext;
                if (lbImgs.ItemContainerGenerator.ContainerFromItem(clickedData) is ListBoxItem lbi) // 获取对应数据 对应的listitem
                {
                    lbi.IsSelected = true; // 选中它
                    RaiseEvent(new RoutedEventArgs(routedEvent));
                }
            }
        }
    }
}
