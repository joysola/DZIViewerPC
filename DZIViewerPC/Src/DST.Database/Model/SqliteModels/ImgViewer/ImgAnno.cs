using GalaSoft.MvvmLight;
using MVVMExtension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DST.Database.Model
{
    //[Table("IMG_Anno")]
    [NotifyAspect]
    public class Img_Anno : SqliteEntity
    {
        /// <summary>
        /// 切片名称
        /// </summary>
        public string Sample_Name { get; set; }
        /// <summary>
        /// 切片id
        /// </summary>
        [Required]
        public string Sample_Id { get; set; }
        /// <summary>
        /// 标记名称
        /// </summary>
        public string Anno_Name { get; set; }
        /// <summary>
        /// 标记内容
        /// </summary>
        public string Description { get; set; }
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
        /// 标记框颜色
        /// </summary>
        public string Color { get; set; } = "#326cf3";
        /// <summary>
        /// 是否事AI标记框
        /// </summary>
        public bool IsAI { get; set; }
        /// <summary>
        /// 标记缩略图
        /// </summary>
        public byte[] ThumbImg { get; set; }

        [NotMapped]
        public Point CurStart => new Point(Start_Point_X, Start_Point_Y);

        [NotMapped]
        public Point CurEnd => new Point(End_Point_X, End_Point_Y);
        /// <summary>
        /// 第九层的坐标
        /// </summary>
        [NotMapped]
        public (Point point1, Point point2) ManualRect => (CurStart, CurEnd);
        /// <summary>
        /// 矩形中心
        /// </summary>
        [NotMapped]
        public Point CenterPoint => new Point((ManualRect.point1.X + ManualRect.point2.X) / 2, (ManualRect.point1.Y + ManualRect.point2.Y) / 2);
    }
}
