using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nico.TransDat.Model
{
    /// <summary>
    /// dat 头文件索引信息，用于快速定位ImageDat的信息
    /// </summary>
    internal class ImageDatIndex
    {
        /// <summary>
        /// 层号, 小于等于5 层的数据，Level=5
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 最小的X，包含该值
        /// </summary>
        public int MinX { get; set; }

        /// <summary>
        /// 最大的X，包含该值
        /// </summary>
        public int MaxX { get; set; }

        /// <summary>
        /// ImageDat 所在的byte索引
        /// </summary>
        public int ByteIndex { get; set; }

        /// <summary>
        /// 该范围的ImageDat的大小
        /// </summary>
        public int Size { get; set; }
    }
}
