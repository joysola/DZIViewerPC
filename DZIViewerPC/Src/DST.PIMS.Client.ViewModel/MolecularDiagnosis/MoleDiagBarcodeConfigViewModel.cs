using DST.Common.Helper;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Expression = System.Linq.Expressions.Expression;
using System.Windows;

namespace DST.PIMS.Client.ViewModel
{
    public class MoleDiagBarcodeConfigViewModel : CustomBaseViewModel
    {
        private SettingModel _settingModel = null;
        
        /// <summary>
        /// 设置实体
        /// </summary>
        public SettingModel SettingModel
        {
            get => _settingModel;
            set { 
                _settingModel = value; 
                RaisePropertyChanged("SettingModel"); 
            }
        }
        /// <summary>
        /// 条码文字对齐方式字典
        /// </summary>
        public Dictionary<string, string> HumanReadDict { get; } = new Dictionary<string, string> { { "0", "无" }, { "1", "左对齐" }, { "2", "居中" }, { "3", "右对齐" } };
        
        /// <summary>
        /// 条码类型字典
        /// </summary>
        public Dictionary<string, string> CodeTypeDict { get; } = new Dictionary<string, string> {
            {"128" ,"128(推荐)"}, { "128M", "128M" }, { "EAN128", "EAN128" },{"EAN128M" ,"EAN128M"},
            {"25" ,"25(纯数字)"},{"25C" ,"25C(纯数字)"},{"25S" ,"25S(纯数字)"},
            {"TELEPEN" ,"TELEPEN"},{ "DPI","DPI(纯数字)"},{"DPL" ,"DPL(纯数字)"} };

        /// <summary>
        /// 保存配置
        /// </summary>
        public ICommand SaveCommand { get; set; }
        /// <summary>
        /// 重置
        /// </summary>
        public ICommand ResetCommand { get; set; }

        public MoleDiagBarcodeConfigViewModel()
        {
            this.ReadSetting();
        }

        private void ReadSetting()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            string strConfi = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.MolecularDiagnosis, "MoleDiagBarcodeConfig");
            if(string.IsNullOrEmpty(strConfi))
            {
                this.SettingModel = new SettingModel();
            }
            else
            {
                this.SettingModel = CopyObject.Deserialize<SettingModel>(strConfi);
            }
        }

        protected override void RegisterCommand()
        {
            // 保存
            this.SaveCommand = new RelayCommand(() =>
            {
                string path = ExtendAppContext.Current.ConfigurationIniPath;
                var settingStr = CopyObject.Serialize(this.SettingModel); // 配置字符串
                IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.MolecularDiagnosis, "MoleDiagBarcodeConfig", settingStr);
                Messenger.Default.Send(true, EnumMessageKey.CloseMoleDiagBarcodeConfig); // 关闭窗口
            });
            // 重置
            this.ResetCommand = new RelayCommand(() =>
            {
                this.ShowMessageBox("是否需要配置初始化？", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question,
                    res => 
                    {
                        if (res == MessageBoxResult.Yes || res == MessageBoxResult.OK)
                        {
                            SettingModel = new SettingModel();
                            this.SaveCommand.Execute(null);
                        }
                    });
            });
        }
    }
}
