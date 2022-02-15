using DST.Common.Extensions;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class PrintSetting : ObservableObject
    {
        /// <summary>
        /// 打印类型
        /// </summary>
        [Notification]
        public string PrintType { get; set; }
        /// <summary>
        /// 海世嘉配置实体
        /// </summary>
        [Notification]
        public HSJPrintSetting HSJPrint { get; set; } = new HSJPrintSetting();
        /// <summary>
        /// TSC配置实体
        /// </summary>
        [Notification]
        public TSCPrintSetting TSCPrint { get; set; } = new TSCPrintSetting();
        /// <summary>
        /// TSC条形码
        /// </summary>
        [Notification]
        public TSCBarCodeSetting TSCBarCode { get; set; } = new TSCBarCodeSetting();
        /// <summary>
        /// 是否混合打印
        /// </summary>
        [Notification]
        public bool IsMix { get; set; }
        /// <summary>
        /// 是否是无焦点打印
        /// </summary>
        [Notification]
        public bool IsNoFocus { get; set; }
    }
}
