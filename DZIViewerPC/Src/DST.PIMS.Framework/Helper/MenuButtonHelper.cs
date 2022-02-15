using DST.PIMS.Framework.Attributes;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DST.PIMS.Framework.Helper
{
    /// <summary>
    /// 菜单和按钮扩展
    /// </summary>
    public static class MenuButtonHelper
    {
        /// <summary>
        /// 获取枚举对应的 字段名称-枚举值 字典
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumType">枚举</param>
        /// <returns></returns>
        public static Dictionary<string, T> GetEnumNameValueDict<T>() where T : Enum
        {
            var allButtonDict = new Dictionary<string, T>(); // 结果
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>(); // 获取枚举的所有枚举值

            foreach (var item in enumValues)
            {
                var name = Enum.GetName(typeof(T), item); // 获取枚举名称
                var field = typeof(T).GetField(name);
                if (field != null)
                {
                    allButtonDict.Add(field?.Name, item);
                }
            }
            return allButtonDict;
        }
        /// <summary>
        /// 获取所有按钮
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<ToolButton> GetAllToolButtons()
        {
            var result = new List<ToolButton>();
            var enumValues = Enum.GetValues(typeof(EnumToolButtonType)).Cast<EnumToolButtonType>(); // 获取枚举的所有枚举值
            foreach (var item in enumValues)
            {
                var name = Enum.GetName(typeof(EnumToolButtonType), item); // 获取枚举名称
                var field = typeof(EnumToolButtonType).GetField(name);
                if (field != null)
                {
                    var toolBtnAttr = field.GetCustomAttribute<ToolButtonAttribute>(); // 获取对应的特性
                    if (toolBtnAttr != null)
                    {
                        var tooBtn = new ToolButton
                        {
                            Content = toolBtnAttr?.Content,
                            BackgroundImage = toolBtnAttr?.BackgroundImage,
                            BackgroundImageMouseOver = toolBtnAttr?.BackgroundImageMouseOver,
                            ToolBtnType = item,
                            EnumName = name,
                        };
                        result.Add(tooBtn);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 清空按钮包含的工作站
        /// </summary>
        /// <param name="toolButtons"></param>
        public static void ClearAllToolButtonIncludeWorks(IEnumerable<ToolButton> toolButtons)
        {
            foreach (var btn in toolButtons)
            {
                btn?.CanIncludedWorkstation.Clear();
            }
        }
    }
}
