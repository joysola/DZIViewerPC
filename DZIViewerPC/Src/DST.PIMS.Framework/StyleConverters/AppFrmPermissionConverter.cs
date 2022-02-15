using DST.PIMS.Framework.Extensions;
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
    /// 是否可见
    /// </summary>
    public class AppFrmPermissionVisibleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is ObservableCollection<AppFrmPermission> permissions && values[1] is string header)
            {
                var pm = permissions.GetPermission(header);
                return pm?.Visible;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 是否可用
    /// </summary>
    public class AppFrmPermissionIsEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is ObservableCollection<AppFrmPermission> permissions && values[1] is string header)
            {
                var pm = permissions.GetPermission(header);
                return pm?.IsEnable;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 是否是展开的的
    /// </summary>
    public class AppFrmPermissionIsExpandedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is ObservableCollection<AppFrmPermission> permissions && values[1] is string header)
            {
                var pm = permissions.GetPermission(header);
                return pm?.IsExpanded;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
