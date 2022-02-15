using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// 旋转控件的宽度设置
    /// </summary>
    public class RotaterWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3 && values[0] is double totalWidth && values[1] is double stratWidth && values[2] is double endWidth
                && totalWidth > 0 && stratWidth > 0 && endWidth > 0)
            {
                return totalWidth - stratWidth - endWidth;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
