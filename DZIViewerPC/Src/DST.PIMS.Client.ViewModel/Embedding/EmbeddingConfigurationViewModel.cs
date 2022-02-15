using DST.ApiClient.Service;
using DST.Common.Helper;
using DST.Database.Model;
using DST.Database.Model.DictModel;
using DST.PIMS.Framework;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HSPrint;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class EmbeddingConfigurationViewModel : CustomBaseViewModel
    {


        [Notification]
        public List<DictItem> HsjPrintTypeList { get; set; } = new List<DictItem>();

        /// <summary>
        /// 打码机类型：包埋打码机，禁止修改属性名称
        /// </summary>
        [Notification]
        public string EmbeddingPrintType { get; set; } = "1";

        /// <summary>
        /// 模板文件，完整路径
        /// </summary>
        [Notification]
        public string EmbeddingTemplateFile { get; set; }

        /// <summary>
        /// 打码机扫描的文件夹
        /// </summary>
        [Notification]
        public string EmbeddingScanDir { get; set; }



      

        public EmbeddingConfigurationViewModel()
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
            this.EmbeddingPrintType = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.RegisterSection, "EmbeddingPrintType");
            this.EmbeddingScanDir = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.RegisterSection, "EmbeddingScanDir");
            this.EmbeddingTemplateFile = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.RegisterSection, "EmbeddingTemplateFile");
        }

        /// <summary>
        /// 保存本地数据
        /// </summary>
        public void SaveConfigur()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.RegisterSection, "EmbeddingPrintType", this.EmbeddingPrintType);
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.RegisterSection, "EmbeddingScanDir", this.EmbeddingScanDir);
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.RegisterSection, "EmbeddingTemplateFile", this.EmbeddingTemplateFile);

            HsjPrintManager.ResetPrinter(this.EmbeddingPrintType, this.EmbeddingScanDir, this.EmbeddingTemplateFile);
        }
    }
}
