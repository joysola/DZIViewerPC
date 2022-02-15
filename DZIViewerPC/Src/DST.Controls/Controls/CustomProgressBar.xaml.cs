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

namespace DST.Controls
{
    /// <summary>
    /// CustomProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class CustomProgressBar : UserControl
    {
        /// <summary>
        /// 百分比
        /// </summary>
        public string Percent
        {
            get { return (string)GetValue(PercentProperty); }
            set { SetValue(PercentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentProperty =
            DependencyProperty.Register("Percent", typeof(string), typeof(CustomProgressBar), new PropertyMetadata("0.00%", Percent_PropertyChangedCallback));
        /// <summary>
        /// 百分比
        /// </summary>
        public string BtnContent
        {
            get { return (string)GetValue(BtnContentProperty); }
            set { SetValue(BtnContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BtnContentProperty =
            DependencyProperty.Register("BtnContent", typeof(string), typeof(CustomProgressBar), new PropertyMetadata("未上传",
            (d, e) =>
            {
                var cpb = d as CustomProgressBar;
                cpb.btnStatus.Content = e.NewValue;
            }));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(CustomProgressBar), new PropertyMetadata(0, Value_PropertyChangedCallback));

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(CustomProgressBar), new PropertyMetadata(100, MaxValue_PropertyChangedCallback));

        // 单个路由添加
        public static readonly RoutedEvent StatusChangedEvent = EventManager.RegisterRoutedEvent("StatusChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CustomProgressBar));
        public event RoutedEventHandler StatusChanged
        {
            add { AddHandler(StatusChangedEvent, value); }
            remove { RemoveHandler(StatusChangedEvent, value); }
        }

        // 另一种路由添加
        public static RoutedEvent PauseEvent;
        public event RoutedEventHandler Pause
        {
            add { AddHandler(PauseEvent, value); }
            remove { RemoveHandler(PauseEvent, value); }
        }

        public CustomProgressBar()
        {
            InitializeComponent();
        }

        static CustomProgressBar()
        {
            PauseEvent = EventManager.RegisterRoutedEvent("Pause", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(CustomProgressBar));
        }

        public void RaiseStatusEvent()
        {
            RoutedEventArgs routedEventArgs = new RoutedEventArgs(CustomProgressBar.StatusChangedEvent);
            this.RaiseEvent(routedEventArgs);//触发路由事件方法
        }

        public static void MaxValue_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                CustomProgressBar cpb = d as CustomProgressBar;
                cpb.probar.Maximum = double.Parse(e.NewValue.ToString());
            }
        }

        public static void Value_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                CustomProgressBar cpb = d as CustomProgressBar;
                cpb.probar.Value = double.Parse(e.NewValue.ToString());

                //if (cpb.probar.Value > 0 && cpb.probar.Value < cpb.MaxValue)
                //{
                //    cpb.btnStatus.Content = "上传中..";
                //}
                //else if (cpb.probar.Value == cpb.MaxValue)
                //{
                //    cpb.btnStatus.Content = "上传完成";
                //}
            }
        }

        public static void Percent_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                CustomProgressBar cpb = d as CustomProgressBar;
                cpb.probar.Value = double.Parse(e.NewValue.ToString());
                //cpb.tbPercent.Text = e.NewValue.ToString();
            }
        }

        private void BtnStatus_Click(object sender, RoutedEventArgs e)
        {
            this.RaiseStatusEvent();
        }
    }
}
