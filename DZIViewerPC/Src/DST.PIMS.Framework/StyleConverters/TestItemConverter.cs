using System;
using System.Globalization;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    public class TestItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }

            string result = "";
            //if (ExtendApiDict.Instance.GlassSlideTestItemDDict.ContainsKey(value.ToString()))
            //{
            //    result = ExtendApiDict.Instance.GlassSlideTestItemDDict[value.ToString()];
            //}

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}