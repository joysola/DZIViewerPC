using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DST.PIMS.Framework.Controls
{
    /// <summary>
    /// 报告视野信息实体
    /// </summary>
    public class SelectRprtImgRect
    {
        /// <summary>
        /// 报告视野矩形
        /// </summary>
        public Rectangle Rect { get; } = new Rectangle { Stroke = (Brush)Application.Current.TryFindResource("SuccessBrush"), StrokeThickness = 5 };
        /// <summary>
        /// 报告视野矩形的msi起点
        /// </summary>
        public Point MsiStratPoint { get; set; }
        /// <summary>
        /// 报告视野矩形的msi终点
        /// </summary>
        public Point MsiEndPoint { get; set; }

        public Point MsiCenterPoint => new Point((MsiStratPoint.X + MsiEndPoint.X) / 2, (MsiStratPoint.Y + MsiEndPoint.Y) / 2);
        /// <summary>
        /// 是否完成
        /// </summary>
        public bool HasFinished { get; set; }
        /// <summary>
        /// 是否开始
        /// </summary>
        public bool IsStarted { get; set; }
    }
}
