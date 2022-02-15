using DST.Common.Expressions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// 缩略图图片转换器
    /// </summary>
    public class ThumbsListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is object model && values[1] is string propName)
            {
                if (ReflectionExpression.Singleton.GetPropValueFunc(model, propName) is string url)// 根据属性名称获取对应属性的值，即url
                {
                    var img = new BitmapImage { CacheOption = BitmapCacheOption.OnLoad, CreateOptions = BitmapCreateOptions.IgnoreImageCache };
                    img.BeginInit();
                    img.UriSource = new Uri(url);
                    img.EndInit();
                    return img;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
