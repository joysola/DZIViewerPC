using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public class TSCPrintSetting : ObservableObject, ILabelPrint
    {
        ///// <summary>
        ///// 打印类型
        ///// </summary>
        //[Notification]
        //public string Code { get; set; }
        /// <summary>
        /// 第一个标签X轴起点
        /// </summary>
        [Notification]
        public int First_X { get; set; } = 30;
        /// <summary>
        /// 第二个标签X轴起点
        /// </summary>
        [Notification]
        public int Second_X { get; set; } = 200;
        /// <summary>
        /// Y轴起点
        /// </summary>
        [Notification]
        public int Y { get; set; } = 5;
        /// <summary>
        /// 检查项目id集合
        /// </summary>
        [Notification]
        public List<string> Products { get; set; } = new List<string> ();

        [Notification]
        public bool IsMix { get; set; }
    }
}
