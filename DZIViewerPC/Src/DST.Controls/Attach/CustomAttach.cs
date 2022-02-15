using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DST.Controls
{
    public class CustomAttach
    {
        public static bool GetIsOnlyNumber(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsOnlyNumberProperty);
        }

        public static void SetIsOnlyNumber(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOnlyNumberProperty, value);
        }

        /// <summary>
        /// 文本框是否只接受数字
        /// </summary>
        public static readonly DependencyProperty IsOnlyNumberProperty =
            DependencyProperty.RegisterAttached("IsOnlyNumber", typeof(bool), typeof(CustomAttach), new PropertyMetadata(false,
                (s, e) =>
                {
                    if (s is TextBox textBox)
                    {
                        textBox.SetValue(InputMethod.IsInputMethodEnabledProperty, !(bool)e.NewValue);
                        textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                        if ((bool)e.NewValue)
                        {
                            textBox.PreviewTextInput += TextBox_PreviewTextInput;
                        }
                    }
                }));

        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }



        public static bool GetIsEndDateTime(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEndDateTimeProperty);
        }

        public static void SetIsEndDateTime(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEndDateTimeProperty, value);
        }

        /// <summary>
        /// 日期控件，是否为结束时间，如果是则默认给日期加上23:59:59
        /// </summary>
        public static readonly DependencyProperty IsEndDateTimeProperty =
            DependencyProperty.RegisterAttached("IsEndDateTime", typeof(bool), typeof(CustomAttach), new PropertyMetadata(false,
                (s, e) =>
                {
                    if (s is DatePicker datePicker)
                    {
                        datePicker.SelectedDateChanged -= DatePicker_SelectedDateChanged;
                        if ((bool)e.NewValue)
                        {
                            datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
                        }
                    }
                }));

        private static void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender is DatePicker dp && dp.SelectedDate.HasValue)
                {
                    dp.SelectedDateChanged -= DatePicker_SelectedDateChanged;
                    dp.SelectedDate = dp.SelectedDate.Value.AddDays(1.0).AddSeconds(-1.0);
                    dp.SelectedDateChanged += DatePicker_SelectedDateChanged;
                }
            }
            finally
            {

            }
        }
    }
}
