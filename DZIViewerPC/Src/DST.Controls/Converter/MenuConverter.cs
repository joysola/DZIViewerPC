using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DST.Controls.Converter
{
    public class MenuConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values[0] != null && values[1] != null && !string.IsNullOrEmpty(values[1].ToString()))
            {
                PropertyInfo pi = values[0].GetType().GetProperty(values[1].ToString());
                if (pi != null)
                {
                    object value = pi.GetValue(values[0], null);
                    return value;
                }
            }

            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
