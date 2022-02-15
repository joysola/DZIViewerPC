using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class HSJPrintSettingViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 配置实体
        /// </summary>
        [Notification]
        public PrintSetting Setting { get; set; } = new PrintSetting();
        /// <summary>
        /// 海世嘉打印类型
        /// </summary>
        public List<DictItem> HsjPrintTypeList => ExtendApiDict.Instance.HsjPrintType;
        /// <summary>
        /// 检验项目字典
        /// </summary>
        public List<ProductModel> ProductDict => ExtendApiDict.Instance.ProductDict;
        /// <summary>
        /// 选择模板文件
        /// </summary>
        public ICommand SelectTempFileCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择模板文件";
            dialog.Filter = "模板文件|*." + "EBT;*.EST";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == true)
            {
                Setting.HSJPrint.TemplateFile = dialog.FileName;
            }
        })).Value;
        /// <summary>
        /// 选择文件夹
        /// </summary>
        public ICommand SelectFilePathCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            var dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            DialogResult result = dilog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                Setting.HSJPrint.ScanDir = dilog.SelectedPath;
            }
        })).Value;
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
