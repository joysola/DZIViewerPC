using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DST.Controls
{
    /// <summary>
    /// MenuComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class MenuComboBox : UserControl
    {
        private object locker = new object();

        /// <summary>
        /// 二级菜单数据源
        /// </summary>
        private IEnumerable menuItemSource = null;

        /// <summary>
        /// ComboBox 数据源
        /// </summary>
        public IEnumerable ItemSource
        {
            get { return (IEnumerable)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(IEnumerable), typeof(MenuComboBox), new PropertyMetadata(new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 显示的字段
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(MenuComboBox), new PropertyMetadata(new PropertyChangedCallback(PropertyChangedCallback)));

        /// <summary>
        /// 下拉框SelectedValuePath
        /// </summary>
        public string SelectedValuePath
        {
            get { return (string)GetValue(SelectedValuePathProperty); }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(MenuComboBox), new PropertyMetadata(null));

        /// <summary>
        /// 下拉框选中项
        /// </summary>
        public object SelectedValue
        {
            get { return (object)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(MenuComboBox), new PropertyMetadata(new PropertyChangedCallback(SelectedValue_PropertyChangedCallback)));

        /// <summary>
        /// 二级菜单数据源属性名称
        /// </summary>
        public string MenuItemSourcePath
        {
            get { return (string)GetValue(MenuItemSourcePathProperty); }
            set { SetValue(MenuItemSourcePathProperty, value); }
        }

        public static readonly DependencyProperty MenuItemSourcePathProperty =
            DependencyProperty.Register("MenuItemSourcePath", typeof(string), typeof(MenuComboBox), new PropertyMetadata(null));

        /// <summary>
        /// 显示的字段
        /// </summary>
        public string MenuDisplayMemberPath
        {
            get { return (string)GetValue(MenuDisplayMemberPathProperty); }
            set { SetValue(MenuDisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty MenuDisplayMemberPathProperty =
            DependencyProperty.Register("MenuDisplayMemberPath", typeof(string), typeof(MenuComboBox), new PropertyMetadata());

        public string MenuDisplayMemberValue
        {
            get { return (string)GetValue(MenuDisplayMemberValueProperty); }
            set { SetValue(MenuDisplayMemberValueProperty, value); }
        }

        public static readonly DependencyProperty MenuDisplayMemberValueProperty =
            DependencyProperty.Register("MenuDisplayMemberValue", typeof(string), typeof(MenuComboBox), new PropertyMetadata(null));

        /// <summary>
        /// 二级菜单的SelectedValuePath
        /// </summary>
        public string MenuSelectedValuePath
        {
            get { return (string)GetValue(MenuSelectedValuePathProperty); }
            set { SetValue(MenuSelectedValuePathProperty, value); }
        }

        public static readonly DependencyProperty MenuSelectedValuePathProperty =
            DependencyProperty.Register("MenuSelectedValuePath", typeof(string), typeof(MenuComboBox), new PropertyMetadata(null));

        /// <summary>
        /// 二级菜单选中的值
        /// </summary>
        public object MenuSelectedValue
        {
            get { return (object)GetValue(MenuSelectedValueProperty); }
            set 
            { 
                SetValue(MenuSelectedValueProperty, value);
                this.SetMenTextBlock();
            }
        }

        public static readonly DependencyProperty MenuSelectedValueProperty =
            DependencyProperty.Register("MenuSelectedValue", typeof(object), typeof(MenuComboBox), new PropertyMetadata(new PropertyChangedCallback(SelectedValue_PropertyChangedCallback)));

        /// <summary>
        /// 当二级菜单有数据时，必须选择其中的二级菜单，不能选择父菜单
        /// </summary>
        public bool IsMustMenu
        {
            get { return (bool)GetValue(IsMustMenuProperty); }
            set { SetValue(IsMustMenuProperty, value); }
        }

        public static readonly DependencyProperty IsMustMenuProperty =
            DependencyProperty.Register("IsMustMenu", typeof(bool), typeof(MenuComboBox), new PropertyMetadata(true));

        /// <summary>
        /// 二级菜单控件
        /// </summary>
        public MenuComboBox()
        {
            InitializeComponent();
            this.Loaded += MenuComboBox_Loaded;
        }

        private void MenuComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbMenu.SelectionChanged += this.CbMenu_SelectionChanged;
        }

        private List<object> originalItemSource = new List<object>();

        public static void SelectedValue_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuComboBox menbox = d as MenuComboBox;
            if (e.Property.Name == "SelectedValue")
            {
                menbox.cbMenu.SelectedItem = e.NewValue;
            }
            else if (e.Property.Name == "MenuSelectedValue")
            {
                menbox.MenuSelectedValue = e.NewValue;
            }
        }

        public static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuComboBox mencbox = d as MenuComboBox;
            if (e.Property.Name == "DisplayMemberPath")
            {
                mencbox.cbMenu.DisplayMemberPath = e.NewValue.ToString();
            }
            else if (e.Property.Name == "ItemSource")
            {
                if(e.NewValue == null)
                {
                    return;
                }

                foreach (var item in e.NewValue as IEnumerable)
                {
                    Type itemType = item.GetType();
                    PropertyInfo[] itemPors = itemType.GetProperties();

                    var newItem = Activator.CreateInstance(itemType);
                    PropertyInfo[] newItemPors = newItem.GetType().GetProperties();

                    foreach (PropertyInfo pi in itemPors)
                    {
                        object value = pi.GetValue(item, null);
                        PropertyInfo newPi = newItemPors.FirstOrDefault(x => x.Name.Equals(pi.Name));
                        if (newPi != null)
                        {
                            newPi.SetValue(newItem, value);
                        }
                    }

                    mencbox.originalItemSource.Add(newItem);
                }
            }
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            lock (this.locker)
            {
                if (string.IsNullOrEmpty(this.SelectedValuePath))
                {
                    this.SelectedValuePath = this.DisplayMemberPath;
                }

                if (!string.IsNullOrEmpty(this.MenuItemSourcePath))
                {
                    StackPanel sp = sender as StackPanel;
                    var curSelected = sp.DataContext;
                    for (int i = 0; i < sp.Children.Count; i++)
                    {
                        if (sp.Children[i] is TextBlock)
                        {
                            TextBlock tb = sp.Children[i] as TextBlock;
                            foreach (var item in this.ItemSource)
                            {
                                if (!item.Equals(curSelected))
                                {
                                    continue;
                                }

                                PropertyInfo menuPro = item.GetType().GetProperty(this.MenuItemSourcePath);
                                if (menuPro != null)
                                {
                                    object menuValue = menuPro.GetValue(item, null);
                                    if (menuValue != null && menuValue is IEnumerable)
                                    {
                                        this.menuItemSource = menuValue as IEnumerable;
                                    }
                                    else
                                    {
                                        this.menuItemSource = null;
                                    }
                                }
                                else
                                {
                                    this.menuItemSource = null;
                                }
                            }
                        }
                        else if (sp.Children[i] is Popup)
                        {
                            (sp.Children[i] as Popup).IsOpen = true;
                            ListBox lb = (sp.Children[i] as Popup).Child as ListBox;
                            lb.ItemsSource = this.menuItemSource;
                            lb.DisplayMemberPath = this.MenuDisplayMemberPath;
                        }
                    }
                }
                else
                {
                    this.menuItemSource = null;
                }
            }
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < (sender as StackPanel).Children.Count; i++)
            {
                if ((sender as StackPanel).Children[i] is Popup)
                {
                    ((sender as StackPanel).Children[i] as Popup).IsOpen = false;
                }
            }
        }

        private void Popup_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void Popup_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Popup).IsOpen = false;
        }

        private void LbChangeType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.MenuSelectedValue = null;
            this.cbMenu.SelectedIndex = -1;
            ListBox lb = sender as ListBox;
            lb.SelectionChanged -= this.LbChangeType_SelectionChanged;

            if (lb == null || lb.SelectedItem == null)
            {
                return;
            }

            object selectItem = lb.DataContext;
            this.SelectedValue = selectItem;
            this.MenuSelectedValue = lb.SelectedItem;
            this.cbMenu.SelectedItem = selectItem;
            lb.SelectedIndex = -1;
            lb.SelectionChanged += this.LbChangeType_SelectionChanged;
        }

        private void SetMenTextBlock()
        {
            object text = null;
            if (this.MenuSelectedValue != null)
            {
                PropertyInfo pi = this.MenuSelectedValue.GetType().GetProperty(this.MenuDisplayMemberPath);
                text = pi.GetValue(this.MenuSelectedValue);
            }

            TextBlock tb = this.cbMenu.Template.FindName("tbMenuDis", this.cbMenu) as TextBlock;
            if (tb != null)
            {
                tb.Visibility = null == text ? Visibility.Collapsed : Visibility.Visible;
                tb.Text = null == text ? "" : "/" + text.ToString();
            }
        }

        private void CbMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedValue = this.cbMenu.SelectedItem;
            if (this.SelectedValue != null)
            {
                // 转换原始数据
                PropertyInfo tm = this.SelectedValue.GetType().GetProperty(this.SelectedValuePath);
                foreach (var item in this.originalItemSource)
                {
                    PropertyInfo itemPi = item.GetType().GetProperty(this.SelectedValuePath);
                    if (itemPi != null && tm.GetValue(this.SelectedValue, null).Equals(itemPi.GetValue(item, null)))
                    {
                        this.SelectedValue = item;
                        break;
                    }
                }

                PropertyInfo pro = this.SelectedValue.GetType().GetProperty(this.MenuItemSourcePath);
                if (pro != null)
                {
                    IList value = pro.GetValue(this.SelectedValue, null) as IList;

                    if (value != null && value.Count > 0)
                    {
                        if (!this.IsMustMenu)
                        {
                            if (this.MenuSelectedValue != null)
                            {
                                bool isContain = false;
                                foreach (var item in value)
                                {
                                    if (this.MenuSelectedValue.Equals(item))
                                    {
                                        isContain = true;
                                        break;
                                    }
                                }

                                if (!isContain)
                                {
                                    this.MenuSelectedValue = null;
                                }
                            }
                        }
                        else
                        {
                            if (this.MenuSelectedValue == null)
                            {
                                this.cbMenu.SelectedItem = null;
                                this.SelectedValue = null;
                            }
                            else
                            {
                                bool isContain = false;
                                foreach (var item in value)
                                {
                                    if (this.MenuSelectedValue.Equals(item))
                                    {
                                        isContain = true;
                                        break;
                                    }
                                }

                                if (!isContain)
                                {
                                    this.cbMenu.SelectedItem = null;
                                    this.SelectedValue = null;
                                    this.MenuSelectedValue = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        this.MenuSelectedValue = null;
                    }
                }
            }
        }
    }
}
