using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.Controls
{
    public class RegisterElement : InfoElement
    {
        /// <summary>
        ///  是否换行
        /// </summary>
        public static readonly DependencyProperty TitleWarpProperty = DependencyProperty.RegisterAttached(
            "TitleWarp", typeof(TextWrapping), typeof(RegisterElement), new FrameworkPropertyMetadata(TextWrapping.NoWrap, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetTitleWarp(DependencyObject element, TextWrapping value) => element.SetValue(TitleWarpProperty, value);

        public static TextWrapping GetTitleWarp(DependencyObject element) => (TextWrapping)element.GetValue(TitleWarpProperty);

        /// <summary>
        ///  输入的信息框高度
        /// </summary>
        public static readonly DependencyProperty InputControlHeightProperty = DependencyProperty.RegisterAttached(
            "InputControlHeight", typeof(double), typeof(RegisterElement), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.Inherits));

        public static void SetInputControlHeight(DependencyObject element, double value) => element.SetValue(InputControlHeightProperty, value);

        public static double GetInputControlHeight(DependencyObject element) => (double)element.GetValue(InputControlHeightProperty);
    }
}
