using DST.Database;
using DST.PIMS.Framework.ExtendContext;
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
    /// HPV结果转换器
    /// </summary>
    public class HpvResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hpvRes = (string)value;
            var val = ExtendApiDict.Instance.HPVResultDict?.FirstOrDefault(x => x.dictKey == hpvRes)?.dictValue;
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 鳞状上皮细胞转换器
    /// </summary>
    public class TagInfoResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hpvRes = (string)value;
            var val = ExtendApiDict.Instance.SampleTctResultDict?.FirstOrDefault(x => x.dictKey == hpvRes)?.dictValue;
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 鳞状上皮细胞转换器
    /// </summary>
    public class GlandularEpithelialCellResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hpvRes = (string)value;
            var val = ExtendApiDict.Instance.GlandularEpithelialCellResultDict?.FirstOrDefault(x => x.dictKey == hpvRes)?.dictValue;
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 鳞状上皮细胞转换器
    /// </summary>
    public class BiopsyResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hpvRes = (string)value;
            var val = ExtendApiDict.Instance.BiopsyFlagDict?.FirstOrDefault(x => x.dictKey == hpvRes)?.dictValue;
            return val;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 计算剩余宽度
    /// </summary>
    public class ExtraInfoWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var stackWidth = (double)values[0]; // stackpanel宽度
            var descWidth = (double)values[1]; // 描述文字宽度
            if (stackWidth == 0 || descWidth == 0)
            {
                return double.NaN;
            }
            else
            {
                return stackWidth - descWidth;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 进度百分比计算
    /// </summary>
    public class ExtraRationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var finish = (int)values[0];
            var total = (int)values[1];
            var ratio = total == 0 ? 0.0 : (double)finish / (double)total * 100;
            var result = double.Parse(ratio.ToString("0")); // 只取整数位
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 辅助信息控件的Margin转换器
    /// </summary>
    public class ExtraMarginConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new Thickness(0, 0, 0, 0);
            if (values != null && values.Length == 2 && !values.Contains(DependencyProperty.UnsetValue))
            {
                var menuH = (double)values[0];
                var topH = (double)values[1];
                result.Top = menuH + topH;
            }
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
