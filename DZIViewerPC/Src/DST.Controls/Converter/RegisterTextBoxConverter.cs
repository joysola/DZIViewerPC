using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DST.Controls.Converter
{
    /// <summary>
    /// hc:TextBox登记框输入框的高度转换器（若设置了RegisterElement.InputControlHeight，则返回他，否则返回默认高度）
    /// </summary>
    public class RegisterTextBoxInputHeightConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] RegisterElement.InputControlHeight; values[1] 默认高度
            if (values.Length == 2 && values[1] is double height)
            {
                if (values[0] is double inputHeight && !inputHeight.Equals(double.NaN))
                {
                    return inputHeight;
                }

                return height;
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
