using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DST.Common.Extensions
{
    public static class HandyControlExtensions
    {

        /// <summary>
        /// 获取所有需要验证的HC控件
        /// </summary>
        /// <param name="depObj"></param>
        /// <returns></returns>
        private static IEnumerable<DependencyObject> FindVerifyHCCtls(DependencyObject depObj) //where T : HandyControl.Controls.IDataInput
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    // VerifyFunc设置了 或者 InfoElement的necessary设置了
                    if (child != null && child is IDataInput ii && !(child is WatermarkTextBox)
                        && (ii.VerifyFunc != null || InfoElement.GetNecessary(child)))
                    {
                        yield return child;
                    }

                    foreach (DependencyObject childOfChild in FindVerifyHCCtls(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有需要验证的HC（IDataInput）类型控件
        /// </summary>
        /// <param name="depObj"></param>
        /// <returns></returns>
        public static IEnumerable<IDataInput> GetAllVerifyHCCtls(this DependencyObject depObj)
        {
            return FindVerifyHCCtls(depObj).Select(x => x as IDataInput);
        }

        /// <summary>
        /// hc控件集合 验证是否全部满足条件
        /// </summary>
        /// <param name="inputs">hc控件集合</param>
        /// <returns></returns>
        public static bool VerifyHCCtlsData(this IEnumerable<IDataInput> inputs)
        {
            foreach (var hc in inputs)
            {
                if (!hc.VerifyData())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
