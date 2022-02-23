using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    [NotifyAspect]
    public class ImgViewFileInfo : ObservableObject
    {
        /// <summary>
        /// 切片二维码
        /// </summary>
        public byte[] QCodeImgUrl { get; set; }
        /// <summary>
        /// 样本缩略图路径
        /// </summary>
        public byte[] SampleImgUrl { get; set; }
        /// <summary>
        /// 文件夹本地完整路径
        /// </summary>
        public string LocalFilePath { get; set; }
        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string DicectoryName { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }
        /// <summary>
        /// 主要用于单文件
        /// </summary>
        public DZIModel DZI { get; set; }
    }
}
