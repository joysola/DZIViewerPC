using DST.Database;
using DST.PIMS.Framework.ExtendContext;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    public class SexNormalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexCode = value?.ToString();
            var sexValue = ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey == sexCode)?.dictValue;
            return sexValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class SexIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexCode = ((int?)value)?.ToString();
            if (!string.IsNullOrEmpty(sexCode) && ExtendApiDict.Instance.SexDict != null)
            {
                var sexValue = ExtendApiDict.Instance.SexDict.FirstOrDefault(x => x.dictKey == sexCode)?.dictValue;
                return sexValue;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}