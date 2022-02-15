using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
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
    /// 登记的样本状态
    /// </summary>
    public class RegisterSampStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int status)
            {
                var result = ExtendApiDict.Instance.RegisterSampleStatusDict?.FirstOrDefault(x => x.dictKey == status.ToString());
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
    /// 登记检查项目是否必填
    /// </summary>
    public class RegisterProdNeceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<PathSampInfo> list)
            {
                return !(list.Count() > 0);
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 检查项目 是否可以编辑
    /// </summary>
    public class RegisterProdEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 4 && values[0] is ObservableCollection<AppFrmPermission> permissions && values[1] is string header && values[2] is PathSampInfo samp && values[3] is bool isPhysDist)
            {
                var isAdd = permissions?.GetPermission(header).IsEnable ?? false; // 是否可以编辑
                if (isAdd) // 新增和编辑可以修改
                {
                    if (isPhysDist) // 前处理不可以修改
                    {
                        return false;
                    }
                    else
                    {
                        if (samp.InspectStatus == "1") // 已送检不可以修改
                        {
                            return false;
                        }
                        else // 未送检可以修改
                        {
                            return true;
                        }
                    }

                }
                else // 查询不可以修改
                {
                    return false;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 送检部位 转换器
    /// </summary>
    public class RegisterSpecEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3 && values[0] is ObservableCollection<AppFrmPermission> permissions && values[1] is string header && values[2] is PathSampInfo samp)
            {
                var isAdd = permissions?.GetPermission(header).IsEnable ?? false; // 是否可以编辑
                if (isAdd) // 新增和编辑可以修改
                {
                    // 1.胃镜特殊处理,不允许新增、删除，只可以编辑
                    //var gast = ExtendApiDict.Instance.ProductDict?.FirstOrDefault(x => x.code == DSTCode.GastroscopeProdCode);
                    if (samp?.ProductID == DSTCode.GastroscopeProdID)
                    {
                        return false;
                    }
                    // 2.非胃镜
                    if (samp.InspectStatus == "1") // 已送检不可以修改
                    {
                        return false;
                    }
                    else // 未送检可以修改
                    {
                        return true;
                    }
                }
                else // 查询不可以修改
                {
                    return false;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 重新取样状态 转换器
    /// </summary>
    public class RegisterReSampStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                var result = ExtendApiDict.Instance.ReSampStatusDict?.FirstOrDefault(x => x.dictKey == status);
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
    /// 重新取样状态 转换器
    /// </summary>
    public class RegisterReSampResasonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                var result = ExtendApiDict.Instance.ReSampReasonDict?.FirstOrDefault(x => x.dictKey == status);
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
    /// 申请单 标本部位数量 是否可以修改（常规病理除了胃镜都可以修改）
    /// </summary>
    public class AppFrmSpecCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string prodcutID && DSTCode.ConvenPathList.Contains(prodcutID) && DSTCode.GastroscopeProdID != prodcutID)
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 送检样本名称 在常规病理下必填
    /// </summary>
    public class AppFrmInspecSampleNescssary : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string productID && DSTCode.ConvenPathList.Contains(productID))
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 重新取样关联，按钮的enable
    /// </summary>
    public class RegisterReSampStatusButtonEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                if (status == "0") // 未重新取样
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 申请单维护，前处理时，检查项目皆不可编辑
    /// </summary>
    public class AppFrmMaintainProductEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3 && values[0] is ObservableCollection<AppFrmPermission> permissions && values[1] is string header && values[2] is bool isPhysDist)
            {
                var isAdd = permissions?.GetPermission(header).IsEnable ?? false; // 是否可以编辑
                if (isAdd)
                {
                    if (isPhysDist)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
