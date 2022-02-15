using DST.PIMS.Framework.ExtendContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// 接收状态转换器
    /// </summary>
    public class ReceiveStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string key = value?.ToString();
            var res = ExtendApiDict.Instance.ReceiveStatus?.FirstOrDefault(x => x.dictKey == key)?.dictValue;
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
