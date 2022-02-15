using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using HandyControl.Controls;
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
    public class PrintProdCCBConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is List<string> products && values[1] is CheckComboBox box)
            {
                var selectedItems = ExtendApiDict.Instance.ProductDict?.Where(x => products.Contains(x.id)).ToList();
                if (selectedItems?.Count > 0)
                {
                    foreach (var item in selectedItems)
                    {
                        box.SelectedItems.Add(item);
                    }
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    //    box.Focus();
                    //    box.IsDropDownOpen = true;
                    //    box.IsDropDownOpen = false;
                    //});
                }
                return products;
            }
            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
