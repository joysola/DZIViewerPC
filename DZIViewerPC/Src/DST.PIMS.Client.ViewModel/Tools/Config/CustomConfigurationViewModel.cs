using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Nico.Common;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class CustomConfigurationViewModel : CustomBaseViewModel
    {
        public PrintSettingViewModel RegisterPrintVM { get; } = new PrintSettingViewModel(IniSectionConst.RegisterSection);
        public PrintSettingViewModel MaterialPrintVM { get; } = new PrintSettingViewModel(IniSectionConst.MaterialSection);
        public PrintSettingViewModel EmbedVM { get; } = new PrintSettingViewModel(IniSectionConst.EmbedSection);
        public PrintSettingViewModel ProdPrintVM { get; } = new PrintSettingViewModel(IniSectionConst.ProductionSection, true);
        public PrintSettingViewModel PhysDistPrintVM { get; } = new PrintSettingViewModel(IniSectionConst.PhysDistSection);
        public TSCBarcodeSettingViewModel TSCBarcodeVM { get; set; } = new TSCBarcodeSettingViewModel(IniSectionConst.MolecularDiagnosis);
        public TSCBarcodeSettingViewModel RegisterTSCBarcodeVM { get; set; } = new TSCBarcodeSettingViewModel(IniSectionConst.ProdBarcodeSection);
        /// <summary>
        /// 保存
        /// </summary>
        public ICommand SaveConfigurationCommand { get; set; }
        /// <summary>
        /// 打印配置命令保存集合
        /// </summary>
        public List<ICommand> SavePrintSettingCommand = new List<ICommand>();

        public CustomConfigurationViewModel()
        {
            SavePrintSettingCommand.Clear();
            SavePrintSettingCommand.Add(RegisterPrintVM.SaveConfigCommand);
            SavePrintSettingCommand.Add(MaterialPrintVM.SaveConfigCommand);
            SavePrintSettingCommand.Add(ProdPrintVM.SaveConfigCommand);
            SavePrintSettingCommand.Add(PhysDistPrintVM.SaveConfigCommand);
            SavePrintSettingCommand.Add(TSCBarcodeVM.SaveConfigCommand);
            SavePrintSettingCommand.Add(RegisterTSCBarcodeVM.SaveConfigCommand);
            SavePrintSettingCommand.Add(EmbedVM.SaveConfigCommand);
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            this.SaveConfigurationCommand = new RelayCommand<object>(data =>
            {
                Logger.Info("开始保存配置");
                try
                {
                    Messenger.Default.Send<string>("", EnumMessageKey.SaveConfiguration);
                    SavePrintSettingCommand.ForEach(command => command.Execute(null));
                }
                catch (Exception ex)
                {
                    Logger.Error("保存配置失败！", ex);
                }
                Logger.Info("保存配置完成");
                this.CloseContentWindow();
            });
        }
    }
}
