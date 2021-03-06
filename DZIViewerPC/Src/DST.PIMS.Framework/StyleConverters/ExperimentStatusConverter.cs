using DST.Database;
using DST.PIMS.Framework.ExtendContext;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    public class ExperimentStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string statusCode = value?.ToString();
            var statusValue = ExtendApiDict.Instance.ExperimentStatusDict.FirstOrDefault(x => x.dictKey == statusCode)?.dictValue;
            return statusValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}