using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    public class SignForMannerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string res = "";
            if(value != null)
            {
                //< !--0 = 人工，1 = 扫码-- >
                switch (value.ToString())
                {
                    case "0":
                        res = "人工";
                        break;
                    case "1":
                        res = "扫码";
                        break;
                }
            }

            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
