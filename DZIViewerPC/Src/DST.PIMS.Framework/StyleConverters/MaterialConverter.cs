using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// 包埋盒打印状态
    /// </summary>
    public class EmbedPrintStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string printStaus)
            {
                var result = ExtendApiDict.Instance.EmbedPrintStatusDict?.FirstOrDefault(x => x.dictKey == printStaus);
                return result?.dictValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 根据取材类型是否显示延迟取材说明按钮
    /// </summary>
    public class MaterialDelayBtnVisiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = Visibility.Hidden;
            if (value is EnumMaterialType mType)
            {
                switch (mType)
                {
                    case EnumMaterialType.Delay:
                        result = Visibility.Visible;
                        break;
                    case EnumMaterialType.Remain:
                    case EnumMaterialType.Supply:
                    case EnumMaterialType.Completed:
                    default:
                        break;
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MaterialSampTissImgDrawStatus : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status=="1")
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
