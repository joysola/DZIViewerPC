using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nico.TransDat.Model
{
    /// <summary>
    /// Dat版本信息，包含版本号，图片最大宽度，最大高度
    /// </summary>
    public class DatVersionInfo
    {
        /// <summary>
        /// Dat文件版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Dat文件最大层图片总宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Dat文件最大层图片总高度
        /// </summary>
        public int Height { get; set; }
    }
}
