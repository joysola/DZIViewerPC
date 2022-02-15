using DST.Database;
using DST.PIMS.Framework.ExtendContext;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    public class CheckProjectStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string checkCode = value?.ToString();
            var checkValue = ExtendApiDict.Instance.CheckProjectStatusDict.FirstOrDefault(x => x.dictKey == checkCode)?.dictValue;
            return checkValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}