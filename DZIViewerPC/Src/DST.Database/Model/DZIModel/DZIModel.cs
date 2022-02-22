using GalaSoft.MvvmLight;
using MVVMExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
   
    public class DZIModel
    {
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 单文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 数据文件头的长度
        /// </summary>
        public int TitleDataLength { get; set; }
        /// <summary>
        /// 瓦片信息
        /// </summary>
        public Dictionary<string, DZITileInfo> TileInfo { get; set; } = new Dictionary<string, DZITileInfo>();
    }
}
