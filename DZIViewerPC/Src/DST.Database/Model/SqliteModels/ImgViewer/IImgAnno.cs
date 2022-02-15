using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST.Database.Model
{
    public interface IImgAnno
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 标记缩略图
        /// </summary>
        public byte[] ThumbImg { get; set; }
        /// <summary>
        /// 起点x
        /// </summary>
        public double Start_Point_X { get; set; }
        /// <summary>
        /// 起点y
        /// </summary>
        public double Start_Point_Y { get; set; }
        /// <summary>
        /// 结束x
        /// </summary>
        public double End_Point_X { get; set; }
        /// <summary>
        /// 结束y
        /// </summary>
        public double End_Point_Y { get; set; }
        /// <summary>
        /// 切片id
        /// </summary>
        public string Sample_Id { get; set; }
    }
}
