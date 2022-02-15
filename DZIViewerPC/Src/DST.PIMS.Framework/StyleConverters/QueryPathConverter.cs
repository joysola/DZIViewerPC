using DST.Database.Model;
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
    /// 染是否显示
    /// </summary>
    public class MarkerVisiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SampleModel sampleModel && !string.IsNullOrEmpty(sampleModel.ProductID)&& !string.IsNullOrEmpty(sampleModel.Markers) && sampleModel.ProductID == DSTCode.GastroscopeProdID)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
