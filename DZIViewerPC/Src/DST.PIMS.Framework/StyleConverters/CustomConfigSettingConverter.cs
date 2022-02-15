using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// TabControl 的 打印类型转换器
    /// </summary>
    public class PrintTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is string printType && values[1] is TabControl tabCtl)
            {
                if (tabCtl.Items?.Count > 0)
                {
                    foreach (TabItem item in tabCtl.Items)
                    {
                        if (item.Tag is string pType && pType == printType)
                        {
                            return item;
                        }
                    }
                }
            }
            return DependencyProperty.UnsetValue;
        }
       
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is TabItem item && item.Tag is string printType)
            {
                return new object[] { printType, item.Parent };
            }
            return null;
        }
    }
}
