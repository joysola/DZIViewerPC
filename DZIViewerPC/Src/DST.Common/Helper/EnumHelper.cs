using System;
using System.ComponentModel;
using System.Reflection;

namespace DST.Common.Helper
{
    /// <summary>
    /// 枚举帮助类，获取Description信息
    /// </summary>
    public class EnumHelper
    {
        public static string GetEnumDesc(Enum e)
        {
            FieldInfo enumFileInfo = e.GetType().GetField(e.ToString());
            if (enumFileInfo == null)
            {
                return "";
            }

            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])enumFileInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
            {
                return EnumAttributes[0].Description;
            }

            return e.ToString();
        }
    }
}
