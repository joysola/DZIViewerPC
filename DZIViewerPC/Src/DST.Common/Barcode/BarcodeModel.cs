using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Common.Barcode
{
    /// <summary>
    /// 条码信息模型
    /// </summary>
    internal class BarcodeModel
    {
        public int VirtKey;//虚拟码
        public int ScanCode;//扫描码
        public string KeyName;//键名
        public uint Ascll;//Ascll
        public char Chr;//字符
        public string OriginalChrs; //原始 字符
        public string OriginalAsciis;//原始 ASCII
        public string OriginalBarCode;  //原始数据条码
        public bool IsValid;//条码是否有效
        public DateTime Time;//扫描时间,
        public string BarCode;//条码信息   保存最终的条码
    }
}
