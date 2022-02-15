using DST.PIMS.Framework.ExtendContext;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    public class SexRadioBtnConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sexCode = value?.ToString();
            string par = parameter?.ToString();
            return sexCode == par;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (isChecked)
            {
                return parameter;
            }
            else
            {
                return ExtendApiDict.Instance.SexDict?.FirstOrDefault(x => x.dictKey!= parameter?.ToString())?.dictKey;
            }
        }
    }
}