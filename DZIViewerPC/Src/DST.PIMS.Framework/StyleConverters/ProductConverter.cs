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
    /// 标记物 转换字典
    /// </summary>
    public class MarkerConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length == 2 && values[0] is string marker && values[1] is List<SpecStainSetting> speStaSetList)
            {
                var result = speStaSetList?.FirstOrDefault(x => x.GroupKey == marker);
                return result?.GroupKeyName;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 加测是否显示 （0：为加测、1：加测）
    /// </summary>
    public class AddTestVisiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string addTest && !string.IsNullOrEmpty(addTest))
            {
                if (addTest == "1")
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
    /// <summary>
    /// 试剂转换器（单一标记物染色剂）
    /// </summary>
    public class ReagentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string marker)
            {
                var result = ExtendApiDict.Instance.ProdReagentDict?.FirstOrDefault(x => x.dictKey == marker);
                return result?.dictValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #region 医嘱相关
    /// <summary>
    /// 医嘱状态 转换器
    /// </summary>
    public class TechAdvSampStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status && !string.IsNullOrEmpty(status))
            {
                var result = ExtendApiDict.Instance.AdviceStatusDict?.FirstOrDefault(x => x.dictKey == status);
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
    /// 技术医嘱类型 转换器
    /// </summary>
    public class TechAdvTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status && !string.IsNullOrEmpty(status))
            {
                var result = ExtendApiDict.Instance.TechAdviceDict?.FirstOrDefault(x => x.dictKey == status);
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
    /// 特检医嘱类型 转换器
    /// </summary>
    public class DoctAdviceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string adviceType)
            {
                var result = ExtendApiDict.Instance.DoctAdviceDict?.FirstOrDefault(x => x.dictKey == adviceType);
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
    /// 所有医嘱类型 转换器
    /// </summary>
    public class AdviceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string adviceType)
            {
                var result = ExtendApiDict.Instance.AdviceDict?.FirstOrDefault(x => x.dictKey == adviceType);
                return result?.dictValue;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion 医嘱相关

}
