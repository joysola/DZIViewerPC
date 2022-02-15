using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DST.Controls.Converter
{
    /// <summary>
    /// 获取DataGrid的序号
    /// </summary>
    public class DataGridIndexConverter : IMultiValueConverter, IValueConverter
    {
        #region 复合绑定
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is DataGrid dataGrid && values[1] is DataGridRow row)
            {
                return (dataGrid.Items.IndexOf(row.DataContext) + 1).ToString();
            }
            return DependencyProperty.UnsetValue;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion 复合绑定

        #region 单一绑定
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridRow row)
            {
                return row.GetIndex() + 1;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion 单一绑定
    }
}
