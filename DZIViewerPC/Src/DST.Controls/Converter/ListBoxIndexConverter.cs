using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DST.Controls.Converter
{
    /// <summary>
    /// 计算listbox的item的序号
    /// </summary>
    public class ListBoxIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is ListBox listbox && values[1] is ListBoxItem item)
            {
                return (listbox.Items.IndexOf(item.DataContext) + 1).ToString();
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
