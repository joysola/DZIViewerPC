using System;
using System.Globalization;
using System.Windows.Data;

namespace DST.PIMS.Framework.StyleConverters
{
    public class UploadStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "未上传";
            if (value != null)
            {
                var status = (int?)value;
                switch (status)
                {
                    case -1:
                        result = "上传失败";
                        break;
                    case 0:
                        result = "未上传";
                        break;
                    case 1:
                        result = "上传部分";
                        break;
                    case 2:
                        result = "正在上传";
                        break;
                    case 3:
                        result = "上传成功";
                        break;
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
}
