using DST.Database;
using DST.PIMS.Framework.ExtendContext;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// 标记医生1、2结果不一致content转换器
    /// </summary>
    public class FirSecTagContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string key = value?.ToString();
            var result = ExtendApiDict.Instance.SignResultDict?.FirstOrDefault(x => x.dictKey == key)?.dictValue;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    //public class FirSecTagResultConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        string res = value?.ToString();
    //        string para = parameter?.ToString();
    //        return res == para;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    /// <summary>
    /// 是否显示标记1、2医生结果
    /// </summary>
    public class FirSecTagVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tag = value?.ToString();
            return string.IsNullOrEmpty(tag) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
