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
    /// 传入 其他控件长度 以及绑定对象总长度
    /// </summary>
    public class CalculateLengthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double totalWidth && values[1] is double otherWidth && totalWidth > 0 && otherWidth > 0)
            {
                if (!totalWidth.Equals(double.NaN) && !totalWidth.Equals(double.NaN))
                {
                    return Math.Abs(totalWidth - otherWidth);
                }
            }
            return DependencyProperty.UnsetValue;
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 传入 固定长度 以及绑定对象总长度
    /// </summary>
    public class CalculateParamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is double totalLength && double.TryParse(parameter.ToString(), out double subLength) && totalLength > 0 && subLength > 0)
            {
                if (!totalLength.Equals(double.NaN) && !subLength.Equals(double.NaN))
                {
                    return Math.Abs(totalLength - subLength);
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 传入父控件 和 子控件名称，返回其差值高度
    /// </summary>
    public class CalcualteLengthbyTempCtlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Expander control && parameter is string subCtlName && !string.IsNullOrEmpty(subCtlName))
            {
                //var xx = LogicalTreeHelper.FindLogicalNode(control, subCtlName);
                var tempCtlObj = control.Template.FindName(subCtlName, control);
                if (tempCtlObj is FrameworkElement subCtl)
                {
                    return control.ActualHeight - subCtl.ActualHeight;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
