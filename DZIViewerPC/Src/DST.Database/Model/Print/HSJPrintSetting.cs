using DST.Common.Extensions;
using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// 海世嘉打印设置
    /// </summary>
    public class HSJPrintSetting : ObservableObject, ILabelPrint
    {
        /// <summary>
        /// 打码机类型：包埋打码机，禁止修改属性名称
        /// </summary>
        [Notification]
        public string PrintType { get; set; } = "1";

        /// <summary>
        /// 模板文件，完整路径
        /// </summary>
        [Notification]
        public string TemplateFile { get; set; }

        /// <summary>
        /// 打码机扫描的文件夹
        /// </summary>
        [Notification]
        public string ScanDir { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        [Notification]
        public List<string> Products { get; set; } = new List<string>();

       
        [Notification]
        public bool IsMix { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
           
        }
    }
}
