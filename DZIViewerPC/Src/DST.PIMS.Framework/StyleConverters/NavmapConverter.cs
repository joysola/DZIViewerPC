using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// 水平线
    /// </summary>
    public class NavmapHLineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is double top && values[1] is double height)
            {
                //var top = Canvas.GetTop(rectangle);
                //var height = rectangle.Height;
                return height / 2 + top + 1.0;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 垂直线
    /// </summary>
    public class NavmapVLineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is double left && values[1] is double width)
            {
                //var top = Canvas.GetTop(rectangle);
                //var height = rectangle.Height;
                return width / 2 + left + 1.0;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
