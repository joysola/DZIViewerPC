using DST.PIMS.Framework.ExtendContext;
using GalaSoft.MvvmLight.CommandWpf;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DST.PIMS.Client.ViewModel
{
    public class AboutViewModel : CustomBaseViewModel
    {
        /// <summary>
        /// 版本英文说明信息
        /// </summary>
        [Notification]
        public string VersionEngInfo { get; set; } = $"CopyRight © 2018 - {DateTime.Now.Year} DeepSight Ltd.";
        /// <summary>
        /// 确定
        /// </summary>
        public ICommand OKCommand => new Lazy<RelayCommand>(() => new RelayCommand(() => this.CloseContentWindow())).Value;

        /// <summary>
        /// 服务端版本
        /// </summary>
        [Notification]
        public string WebVersion { get; set; } = CommonConfiguration.ApiVersion + ".0.0.0";
    }
}
