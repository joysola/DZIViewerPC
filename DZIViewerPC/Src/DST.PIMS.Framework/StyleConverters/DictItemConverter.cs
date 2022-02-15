using DST.Database.Model.DictModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    /// <summary>
    /// 字典通用转换器
    /// </summary>
    public class DictItemConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values != null && values.Length == 2 && values[0] != null && values[1] != null)
            {
                string dictKey = values[1].ToString();
                DictItem dictItem = null;
                switch(values[0].ToString())
                {
                    case "ExpressStatusDict":
                        dictItem = ExtendContext.ExtendApiDict.Instance.ExpressStatusDict.FirstOrDefault(x => x.dictKey.Equals(dictKey));
                        break;

                    case "SexDict":
                        dictItem = ExtendContext.ExtendApiDict.Instance.SexDict.FirstOrDefault(x => x.dictKey.Equals(dictKey));
                        break;
                    case "activityTypeList":
                        dictItem = ExtendContext.ExtendApiDict.Instance.ActivityTypeDict.FirstOrDefault(x => x.dictKey.Equals(dictKey));
                        break;
                }

                if(dictItem != null)
                {
                    return dictItem.dictValue;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
