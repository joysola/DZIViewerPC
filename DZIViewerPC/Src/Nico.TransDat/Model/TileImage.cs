using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nico.TransDat.Model
{
    /// <summary>
    /// 瓦片图，提供给第三方调用
    /// </summary>
    public class TileImage
    {
        /// <summary>
        /// 层序号，图片在第几层图，从1开始。如果 LayIndex = -1，则表示该文件是非瓦片图
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 大图的列序号
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 大图的行序号
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 图片二进制
        /// </summary>
        public byte[] ImageByte { get; set; }
    }
}
