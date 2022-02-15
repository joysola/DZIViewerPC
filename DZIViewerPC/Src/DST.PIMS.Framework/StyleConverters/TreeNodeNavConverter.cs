using DST.Database.Model;
using DST.PIMS.Framework.Model;
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
    public class TreeNodeNavConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TreeNode node && node.Tag is MenuInfoModel)
            {
                if (node.Parent == null)
                {
                    return $"{node.Label}";
                }
                else
                {
                    return $"{node.Parent.Label} / {node.Label}";
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
