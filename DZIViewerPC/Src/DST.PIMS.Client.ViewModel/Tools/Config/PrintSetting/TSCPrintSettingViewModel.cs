using DST.Database.Model;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class TSCPrintSettingViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 配置实体
        /// </summary>

        [Notification]
        public PrintSetting Setting { get; set; } = new PrintSetting();
        /// <summary>
        /// 检验项目字典
        /// </summary>
        public List<ProductModel> ProductDict => ExtendApiDict.Instance.ProductDict;
        /// <summary>
        /// CheckComboBox加载完成需要重新应用模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CheckComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is HandyControl.Controls.CheckComboBox checkBox)
            {
                checkBox.OnApplyTemplate();
            }
        }
    }
}
