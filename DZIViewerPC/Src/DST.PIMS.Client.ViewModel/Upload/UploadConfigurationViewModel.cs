using DST.Common.Helper;
using DST.PIMS.Framework.ExtendContext;
using DST.PIMS.Framework.Model;
using GalaSoft.MvvmLight.Messaging;
using MVVMExtension;
using System.Collections.Generic;

namespace DST.PIMS.Client.ViewModel
{
    public class UploadConfigurationViewModel : CustomBaseViewModel
    {
        [Notification]
        public string UploadImageRootPath { get; set; }

        [Notification]
        public List<int> MaxUploadCountList { get; set; } = new List<int>();

        [Notification]
        public int MaxUploadCount { get; set; }

        public UploadConfigurationViewModel()
        {
            this.LoadConfigur();
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadConfigur()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            this.UploadImageRootPath = IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.Upload, "UploadImageRootPath");
            for(int i = 1; i <= 15; i++)
            {
                this.MaxUploadCountList.Add(i);
            }

            this.MaxUploadCount = int.Parse(IniHelper.CreateInstance(path).IniReadValue(IniSectionConst.Upload, "MaxUploadCount", "1"));
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
        /// 保存本地数据
        /// </summary>
        public void SaveConfigur()
        {
            string path = ExtendAppContext.Current.ConfigurationIniPath;
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.Upload, "UploadImageRootPath", this.UploadImageRootPath);
            IniHelper.CreateInstance(path).IniWriteValue(IniSectionConst.Upload, "MaxUploadCount", this.MaxUploadCount.ToString());

            // 刷新左侧的树形菜单
            Messenger.Default.Send<object>(null, EnumMessageKey.RefreshUploadTreeMenu);
        }
    }
}
