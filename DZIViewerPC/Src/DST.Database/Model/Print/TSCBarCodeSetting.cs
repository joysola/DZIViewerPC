using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    /// <summary>
    /// TSC条形码
    /// </summary>
    public class TSCBarCodeSetting : ObservableObject
    {
        /// <summary>
        /// 打印码号（BARCODE 条形码、QRCODE 二维码）
        /// </summary>
        [Notification]
        public string Code { get; set; } = "BARCODE";
        /// <summary>
        /// 起始x坐标
        /// </summary>
        [Notification] 
        public int X { get; set; } = 105;
        /// <summary>
        /// 起始x坐标
        /// </summary>
        [Notification] 
        public int X_Other { get; set; } = 315;
        /// <summary>
        /// 起始y坐标
        /// </summary>
        [Notification] 
        public int Y { get; set; } = 17;
        /// <summary>
        /// 字串型別
        /// </summary>
        [Notification] 
        public string CodeType { get; set; } = "93";
        /// <summary>
        /// 条码高度
        /// </summary>
        [Notification] 
        public int Height { get; set; } = 55;
        /// <summary>
        /// 条码文字对齐方式（0 无、1 左对齐、2 居中、3 右对齐）
        /// </summary>
        [Notification] 
        public string HumanReadable { get; set; } = "2";
        /// <summary>
        /// 旋转角度（顺时针）
        /// </summary>
        [Notification] 
        public int Rotation { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        [Notification] 
        public string Narrow { get; set; } = "1";
        /// <summary>
        /// 条码宽度
        /// </summary>
        [Notification] 
        public int Width { get; set; } = 1;
        /// <summary>
        /// 条码的对齐方式（0 默认左对齐、1 左对齐、2 居中、3 右对齐）
        /// </summary>
        [Notification] 
        public string Alignment { get; set; } = "2";
    }
}
