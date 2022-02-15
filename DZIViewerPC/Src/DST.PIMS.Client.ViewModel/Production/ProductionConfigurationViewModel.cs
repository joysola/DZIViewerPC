using DST.ApiClient.Service;
using DST.Common.Helper;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
using HSPrint;
using MVVMExtension;
using System.Collections.Generic;

namespace DST.PIMS.Client.ViewModel
{
    public class ProductionConfigurationViewModel : CustomBaseViewModel
    {
        [Notification]
        public List<DictItem> HsjPrintTypeList { get; set; } = new List<DictItem>();

        /// <summary>
        /// 打码机类型：包埋打码机，禁止修改属性名称
        /// </summary>
        [Notification]
        public string ProductionPrintType { get; set; } = "2";

        /// <summary>
        /// 模板文件，完整路径
        /// </summary>
        [Notification]
        public string ProductionTemplateFile { get; set; }

        /// <summary>
        /// 打码机扫描的文件夹
        /// </summary>
        [Notification]
        public string ProductionScanDir { get; set; }


        public ProductionConfigurationViewModel()
        {
            this.HsjPrintTypeList = DictService.Instance.GetHsjPrintType().Result;
            this.LoadConfigur();
        }

        protected override void RegisterCommand()
        {
            base.RegisterCommand();

            // 保存配置
            Messenger.Default.Register<string>(this, EnumMessageKey.SaveConfiguration, data =>
            {
                this.SaveConfigur();
            });

        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadConfigur()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            this.ProductionPrintType = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.ProductionSection, "ProductionPrintType");
            this.ProductionScanDir = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.ProductionSection, "ProductionScanDir");
            this.ProductionTemplateFile = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.ProductionSection, "ProductionTemplateFile");
        }

        /// <summary>
        /// 保存本地数据
        /// </summary>
        public void SaveConfigur()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.ProductionSection, "ProductionPrintType", this.ProductionPrintType);
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.ProductionSection, "ProductionScanDir", this.ProductionScanDir);
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.ProductionSection, "ProductionTemplateFile", this.ProductionTemplateFile);

            HsjPrintManager.ResetPrinter(this.ProductionPrintType, this.ProductionScanDir, this.ProductionTemplateFile);
        }
    }
}
