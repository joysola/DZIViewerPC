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
    /// 取材状态
    /// </summary>
    public class DrawMaterStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                var result = ExtendApiDict.Instance.DrawMaterStatusDict?.FirstOrDefault(x => x.dictKey == status);
                return result?.dictValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
