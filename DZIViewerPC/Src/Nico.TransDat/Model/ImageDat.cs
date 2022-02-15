using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nico.TransDat.Model
{
    internal class ImageDat
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
        /// 图片大小
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// 图片在总字节中的位置
        /// </summary>
        public int ByteIndex { get; set; }

        /// <summary>
        /// 图片二进制
        /// </summary>
        public byte[] ImageByte { get; set; }

        /// <summary>
        /// 非瓦片图文件名称。和LayIndex=-1时，才会使用到
        /// </summary>
        public string OtherFileName { get; set; }
    }
}
