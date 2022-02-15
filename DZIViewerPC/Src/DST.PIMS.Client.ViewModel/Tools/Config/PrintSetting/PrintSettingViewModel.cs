using DST.Database.Model;
using DST.PIMS.Framework.Helper;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class PrintSettingViewModel : CustomBaseViewModel
    {

        public HSJPrintSettingViewModel HSJViewModel { get; set; } = new HSJPrintSettingViewModel();

        public TSCPrintSettingViewModel TSCViewModel { get; set; } = new TSCPrintSettingViewModel();
        /// <summary>
        /// 保存ini节名
        /// </summary>
        public string SectionName { get; set; }
        /// <summary>
        /// 打印配置实体
        /// </summary>
        [Notification]
        public PrintSetting Setting { get; set; } = new PrintSetting();
        /// <summary>
        /// 是否需要混合
        /// </summary>
        [Notification]
        public bool IsNeedMix { get; set; }
        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveConfigCommand => new Lazy<RelayCommand>(() => new RelayCommand(() =>
        {
            this.SaveConfigur();
        })).Value;

        public PrintSettingViewModel()
        {
        }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="sectionName">节点名</param>
        /// <param name="isNeedMix">是否混合打印</param>
        public PrintSettingViewModel(string sectionName, bool isNeedMix = false) : this()
        {
            SectionName = sectionName;
            IsNeedMix = isNeedMix;
            LoadConfigur();
        }


        //protected override void RegisterCommand()
        //{
        //    Messenger.Default.Register<string>(this, EnumMessageKey.SaveConfiguration, data =>
        //    {
        //        this.SaveConfigur();
        //    });
        //}

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadConfigur()
        {
            if (!string.IsNullOrEmpty(SectionName))
            {
                var result = PrintSetHelper.GetPrintSetting(SectionName);

                if (result != null)
                {
                    Setting = result;
                }
                else
                {
                    InitTSCSetting();
                }
                //HSJViewModel.HSJPrint = Setting.HSJPrint;
                //TSCViewModel.TSCPrint = Setting.TSCPrint;
                HSJViewModel.Setting = Setting;
                TSCViewModel.Setting = Setting;
            }
        }
        /// <summary>
        /// 保存本地数据
        /// </summary>
        private void SaveConfigur()
        {
            if (!string.IsNullOrEmpty(SectionName))
            {
                PrintSetHelper.SavePrintsetting(SectionName, Setting);

                //if (Setting.PrintType == nameof(HSJPrintSetting))
                //{
                //   HsjPrintManager.ResetPrinter(Setting.HSJPrint.PrintType, Setting.HSJPrint.ScanDir, Setting.HSJPrint.TemplateFile);
                //}
            }
        }
        /// <summary>
        /// 初始化的 工作站打印参数
        /// </summary>
        private void InitTSCSetting()
        {
            if (!string.IsNullOrEmpty(SectionName))
            {
                switch (SectionName)
                {
                    case IniSectionConst.PhysDistSection:
                        Setting.TSCPrint = new TSCPrintSetting { First_X = 20, Second_X = 200, Y = 5 };
                        break;
                    case IniSectionConst.RegisterSection:
                        Setting.TSCPrint = new TSCPrintSetting { First_X = 20, Second_X = 200, Y = 5 };
                        break;
                    case IniSectionConst.MaterialSection:
                        Setting.TSCPrint = new TSCPrintSetting { First_X = 20, Second_X = 200, Y = 5 };
                        break;
                    case IniSectionConst.ProductionSection:
                        Setting.TSCPrint = new TSCPrintSetting { First_X = 20, Second_X = 200, Y = 5 };
                        break;
                }
            }
        }
    }
}
